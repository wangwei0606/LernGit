using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Threading;

public class CrcVerty
{
    private static CrcVerty _mInstance = null;
    static SHA1CryptoServiceProvider osha1 = new SHA1CryptoServiceProvider();
    private CrcVerty() { }
    public static CrcVerty Instance()
    {
        if(_mInstance==null)
        {
            _mInstance = new CrcVerty();
        }
        return _mInstance;
    }
    public string GenHash(string file)
    {
        using (Stream s = File.OpenRead(file))
        {
            var hash = osha1.ComputeHash(s);
            var shash = Convert.ToBase64String(hash);
            return shash;
        }
    }
}
public class UncompressManager
{
    public class FinishTask
    {
        private FinishTask()
        {

        }
        public FinishTask(IUnCompressTask task,HttpLoadCode code)
        {
            mTask = task;
            mResultCode = code;
        }
        public IUnCompressTask mTask;
        public HttpLoadCode mResultCode;
    }
    private List<FinishTask> _mFinishTaskList;
    private List<IUnCompressTask> _mWaitTaskList;
    private Thread _mUnpackThread;
    private IUnCompressTask task = null;
    public void OnUpdate()
    {
        lock(_mFinishTaskList)
        {
            if(_mFinishTaskList.Count>0)
            {
                foreach(FinishTask eleTask in _mFinishTaskList)
                {
                    eleTask.mTask.OnFinishUnCompress(eleTask.mResultCode);
                }
                _mFinishTaskList.Clear();
            }
            if(task!=null)
            {
                task.OnUnZipProcess(task.Process);
            }
        }
    }
    public UncompressManager()
    {
        _mUnpackThread = new Thread(new ThreadStart(TheadFunc));
        _mWaitTaskList = new List<IUnCompressTask>();
        _mFinishTaskList = new List<FinishTask>();
    }
    public void Start()
    {
        if(_mUnpackThread!=null)
        {
            _mUnpackThread.Start();
        }
    }
    public void End()
    {
        try
        {
            if(_mUnpackThread!=null)
            {
                _mUnpackThread.Abort();
            }
        }
        catch(Exception e)
        {

        }
        _mUnpackThread = null;
        if(_mWaitTaskList!=null)
        {
            _mWaitTaskList.Clear();
        }
        if(_mFinishTaskList!=null)
        {
            _mFinishTaskList.Clear();
        }
    }
    private void TheadFunc()
    {
        while(true)
        {
            if(_mWaitTaskList.Count>0)
            {
                lock(_mWaitTaskList)
                {
                    task = _mWaitTaskList.ElementAt(0);
                    _mWaitTaskList.Remove(task);
                }
                if(task!=null)
                {
                    HttpLoadCode resultCode = HttpLoadCode.eOK;
                    do
                    {
                       if(CrcVerifyOp(task)==false)
                        {
                            resultCode = HttpLoadCode.eCrcError;
                            break;
                        }
                       if(task.IsZip()==false)
                        {
                            break;
                        }
                       if(UnPack(task)==false)
                        {
                            resultCode = HttpLoadCode.eUncompressError;
                            break;
                        }
                    }
                    while (false);
                    lock(_mFinishTaskList)
                    {
                        _mFinishTaskList.Add(new FinishTask(task, resultCode));
                        task = null;
                    }
                }
            }
            else
            {
                Thread.Sleep(100);
            }
        }
    }
    public void SendTask(IUnCompressTask task)
    {
        lock(_mWaitTaskList)
        {
            _mWaitTaskList.Add(task);
        }
    }
    private bool CrcVerifyOp(IUnCompressTask task)
    {
        string crc = task.getFileCrc();
        if(string.IsNullOrEmpty(crc))
        {
            return true;
        }
        string hash = CrcVerty.Instance().GenHash(task.getZipFileName());
        if(crc.CompareTo(hash)==0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool UnPack(IUnCompressTask task)
    {
        return UncompressUtils.UnZip(task.getZipFileName(), task.getDestFoldName(), (process) => { task.Process = process; });
    }
}