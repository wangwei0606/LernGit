using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BestHTTP;
using System.IO;

public class BestHTTPDownloader : BestHTTPRequestBase
{
    static string TMP_FILE_EX = ".tmp";
    public string FIleName { get; protected set; }
    public long FileLength { get; protected set; }
    public long FileOffset { get; protected set; }
    public string FileCrc { get; protected set; }
    public int StreamFragmentSize { get; protected set; }
    public int RetryCount { get; protected set; }
    public bool IsRequestInProgress { get; protected set; }
    protected string _mTempDownloadFileName;
    protected Action<IAsyncTask, HttpLoadCode> _mOnFinishCallback = null;
    protected Action<int, int> _mOnSaveFile = null;
    protected FileStream _mFileStream;
    protected bool _isInitSize = false;
    protected int _mAllSize = 0;
    protected int _mSaveSize = 0;
    public BestHTTPDownloader()
    {
        IsRequestInProgress = false;
    }
    public bool Init(string url,string downloadAs,Action<IAsyncTask,HttpLoadCode> finishCallback)
    {
        if(CurrentRequest!=null)
        {
            UnityEngine.Debug.LogError("besthttpdownloader init currentrequest!=null");
            return false;
        }
        RetryCount = 0;
        FIleName = downloadAs;
        _mOnFinishCallback = finishCallback;
        _mTempDownloadFileName = downloadAs + TMP_FILE_EX;
        CurrentRequest = new HTTPRequest(new Uri(url), OnReceived);
        return true;
    }
    public void SetSaveFileProcess(Action<int,int> onSaveFile)
    {
        _mOnSaveFile = onSaveFile;
    }
    public void SetFileCrc(long fileLength,string fileCrc)
    {
        FileCrc = fileCrc;
        FileLength = fileLength;
    }
    public bool RequestDownload(int streamFramgentSize,bool needResume)
    {
        if(_mFileStream!=null)
        {
            UnityEngine.Debug.LogError("besthttpdownloader:send _mfilestream!=null");
            return false;
        }
        SupportResume = false;
        FileOffset = 0;
        StreamFragmentSize = streamFramgentSize;
        CurrentRequest.DisableCache = true;
        if(streamFramgentSize>0)
        {
            CurrentRequest.UseStreaming = true;
            CurrentRequest.StreamFragmentSize = streamFramgentSize;
            if(needResume)
            {
                FileInfo info = new FileInfo(_mTempDownloadFileName);
                if(info!=null && info.Exists==true)
                {
                    long startPos = info.Length;
                    if(FileLength>0&&FileLength==startPos)
                    {
                        OnDownLoadFinish(this, HttpLoadCode.eFileExists);
                        return true;
                    }
                    if(startPos>0)
                    {
                        FileOffset = startPos;
                        CurrentRequest.SetRangeHeader((int)startPos);
                        SupportResume = true;
                    }
                }
            }
            _mFileStream = new FileStream(_mTempDownloadFileName, FileMode.Append);
        }
        else
        {
            _mFileStream = new FileStream(_mTempDownloadFileName, FileMode.OpenOrCreate);
        }
        return Send();
    }
    public void Close()
    {
        if(CurrentRequest!=null)
        {
            CurrentRequest.Abort();
        }
        CurrentRequest = null;
        CloseFile();
    }
    public bool Retry(int MaxRetry)
    {
        if(RetryCount<MaxRetry)
        {
            ++RetryCount;
            if(RequestDownload(StreamFragmentSize,SupportResume))
            {
                return true;
            }
        }
        Close();
        return false;
    }
    protected override void OnResponseFinish(HttpLoadCode state, HTTPRequest request, HTTPResponse response)
    {
        CloseFile();
        if(state==HttpLoadCode.eDontSupportResume)
        {
            SupportResume = false;
            if(CurrentRequest.HasHeader("Range"))
            {
                CurrentRequest.RemoveHeader("Range");
            }
            if(File.Exists(_mTempDownloadFileName))
            {
                UnityEngine.Debug.LogError("besthttpdownloader edontsupportresume delete file" + _mTempDownloadFileName);
                File.Delete(_mTempDownloadFileName);
            }
        }
        IsRequestInProgress = false;
        OnDownLoadFinish(this, state);
    }
    protected override void OnResponseStream(HTTPResponse response)
    {
        if(!_isInitSize)
        {
            List<string> contentLengthHeaders = response.GetHeaderValues("content-length");
            if(contentLengthHeaders!=null)
            {
                _mAllSize = int.Parse(contentLengthHeaders[0]);
                _isInitSize = true;
            }
        }
        if(_mFileStream==null)
        {
            return;
        }
        List<byte[]> framents = response.GetStreamedFragments();
        if(framents!=null)
        {
            foreach(byte[] data in framents)
            {
                _mSaveSize += data.Length;
                _mFileStream.Write(data, 0, data.Length);
            }
            onSaveFile();
        }
    }
    protected void CloseFile()
    {
        if(_mFileStream!=null)
        {
            _mFileStream.Close();
        }
        _mFileStream = null;
    }
    private bool Send()
    {
        if(IsRequestInProgress)
        {
            return false;
        }
        IsRequestInProgress = true;
        CurrentRequest.Send();
        return true;
    }
    protected void RenameFile()
    {
        if(File.Exists(FIleName))
        {
            File.Delete(FIleName);
        }
        File.Move(_mTempDownloadFileName, FIleName);
    }
    virtual protected void OnDownLoadFinish(BestHTTPDownloader downloader,HttpLoadCode state)
    {
        if(state==HttpLoadCode.eDownloadOK&&FileLength>0)
        {
            FileInfo info = new FileInfo(_mTempDownloadFileName);
            if(info!=null &&info.Exists==true)
            {
                if(FileLength!=info.Length)
                {
                    state = HttpLoadCode.eFileLengthError;
                    File.Delete(_mTempDownloadFileName);
                }
            }
        }
        if(state==HttpLoadCode.eDownloadOK||state==HttpLoadCode.eFileExists)
        {
            RenameFile();
        }
        _mStateCode = (int)state;
        if(_mOnFinishCallback!=null)
        {
            _mOnFinishCallback(this, state);
        }
    }
    virtual protected void onSaveFile()
    {
        if(_mOnSaveFile!=null)
        {
            if(_mAllSize>0)
            {
                try
                {
                    _mOnSaveFile.Invoke(_mSaveSize, _mAllSize);
                }
                catch(Exception e)
                {
                    UnityEngine.Debug.LogError("noticesize error " + e.StackTrace);
                }
            }
        }
    }
    protected override void OnResponseWrite(HTTPResponse response)
    {
        if(_mFileStream==null)
        {
            return;
        }
        _mSaveSize += response.Data.Length;
        _mFileStream.Write(response.Data, 0, response.Data.Length);
        onSaveFile();
    }
}