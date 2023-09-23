using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BestHTTP;
using System.IO;

public class HttpLoadTask :BestHTTPDownloader,IUnCompressTask
{
    static int STREAM_FRAGMENT = 256 * 1024;
    public HttpLoadTask(string url,string path,Action<IAsyncTask,HttpLoadCode> onFinish,long fileLength=0,string fileCrc="")
    {
        Init(url, path, onFinish);
        SetFileCrc(fileLength, fileCrc);
    }
    private bool _mIsDecompression = false;
    private string _mUnZipPath;
    private float _mUnzipProcess = 0.0f;

    public float Process
    {
        get
        {
            return _mUnzipProcess;
        }

        set
        {
            _mUnzipProcess = value;
        }
    }

    public string getZipFileName()
    {
        return _mTempDownloadFileName;
    }

    public string getDestFoldName()
    {
        return _mUnZipPath;
    }

    public string getFileCrc()
    {
        return FileCrc;
    }

    public bool IsZip()
    {
        return _mIsDecompression;
    }

    public void OnFinishUnCompress(HttpLoadCode eCode)
    {
        OnUpdateFinish(eCode);
    }

    public void OnUnZipProcess(float process)
    {
        if(_mOnProgress!=null)
        {
            _mOnProgress((int)HttpLoadCode.eHttpProcessing, HttpTipsCode.UnZip, process * 0.5f + 0.5f);
        }
    }
    public void SetUnZipPath(string path)
    {
        if(!string.IsNullOrEmpty(path))
        {
            _mUnZipPath = path;
            _mIsDecompression = true;
        }
    }
    private void OnUpdateFinish(HttpLoadCode state)
    {

    }
    protected override void OnDownLoadProgress(HTTPRequest req, int downloaded, int length)
    {
        _mStateCode = (int)HttpLoadCode.eHttpProcessing;
        if(_mOnProgress!=null)
        {
            _mOnProgress(_mStateCode, HttpTipsCode.Download, _mIsDecompression ? (downloaded * 0.5f / length) : (downloaded * 1.0f / length));
        }
    }
    public void Download(string customId,int streamFragment,bool needResume,string UnZipPath,Action<int,int,float> onProgress,Action<int,int> onSaveFile)
    {
        if(!string.IsNullOrEmpty(UnZipPath))
        {
            _mUnZipPath = UnZipPath;
            _mIsDecompression = true;
        }
        _mCustomId = customId;
        SetProgress(onProgress);
        SetSaveFileProcess(onSaveFile);
        RequestDownload(streamFragment, needResume);

    }
    protected override void OnDownLoadFinish(BestHTTPDownloader downloader, HttpLoadCode state)
    {
        if(state==HttpLoadCode.eDownloadOK&&FileLength>0)
        {
            FileInfo info = new FileInfo(_mTempDownloadFileName);
            if(info!=null&&info.Exists==true)
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
            if(string.IsNullOrEmpty(FileCrc)&&_mIsDecompression==false)
            {
                OnUpdateFinish(HttpLoadCode.eOK);
            }
            else
            {
                FileCrcAndDecompression();  //解压缩
            }
        }
        else
        {
            OnUpdateFinish(state);
        }
    }
    private void FileCrcAndDecompression()
    {
        HttpLoadMgr.Instance.UncompressMgr.SendTask(this);
    }
}
