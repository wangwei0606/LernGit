using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BestHTTP;

public abstract class BestHTTPRequestBase : IAsyncTask
{
    public HTTPRequest CurrentRequest { get; protected set; }
    public bool SupportResume { get; protected set; }
    protected Action<int, int, float> _mOnProgress = null;
    protected string _mCustomId;
    protected string _mLastError;
    protected int _mStateCode;
    public string LastError
    {
        get
        {
            return _mLastError;
        }
        set
        {
            _mLastError = value;
        }
    }
    public string CustomID
    {
        get
        {
            return _mCustomId;
        }
        set
        {
            _mCustomId = value;
        }
    }
    public int StateCode
    {
        get
        {
            return _mStateCode;
        }
        set
        {
            _mStateCode = value;
        }
    }
    abstract protected void OnResponseFinish(HttpLoadCode state, HTTPRequest request, HTTPResponse response);
    abstract protected void OnResponseStream(HTTPResponse response);
    abstract protected void OnResponseWrite(HTTPResponse response);
    public void SetProgress(Action<int, int, float> progressCallback)
    {
        _mOnProgress = progressCallback;
        if(_mOnProgress!=null)
        {
            CurrentRequest.OnProgress = OnDownLoadProgress;
        }
    }
    protected virtual void OnDownLoadProgress(HTTPRequest req,int downloaded,int length)
    {
        _mStateCode = (int)HttpLoadCode.eHttpProcessing;
        if(_mOnProgress!=null)
        {
            _mOnProgress(_mStateCode, HttpTipsCode.Download, (downloaded * 1.0f / length));
        }
    }
    private HttpLoadCode OnReceiveFinish(HTTPRequest request,HTTPResponse response)
    {
        if(response.StatusCode==416)
        {
            _mLastError = "Requested range not satisfiable";
            return HttpLoadCode.eDontSupportResume;
        }
        else if(response.StatusCode==200)
        {
            if(request.UseStreaming==false)
            {
                OnResponseWrite(response);
                return HttpLoadCode.eDownloadOK;
            }
            else
            {
                if(SupportResume==true)
                {
                    _mLastError = "partial content doesn't supported by the server,content can be downloaded as a whole";
                    return HttpLoadCode.eDontSupportResume;
                }
                else
                {
                    OnResponseStream(response);
                    return HttpLoadCode.eDownloadOK;
                }
            }
        }
        else if(response.StatusCode==206)
        {
            if(request.UseStreaming==false)
            {
                OnResponseWrite(response);
            }
            else
            {
                OnResponseStream(response);
            }
            return HttpLoadCode.eDownloadOK;
        }
        else
        {
            _mLastError = string.Format("response.statuscode:{0}", response.StatusCode);
            return HttpLoadCode.eError;
        }
    }
    virtual protected void OnReceived(HTTPRequest request,HTTPResponse response)
    {
        HttpLoadCode state = HttpLoadCode.eNull;
        switch(request.State)
        {
            case HTTPRequestStates.Finished:
                state = OnReceiveFinish(request, response);
                break;
            case HTTPRequestStates.Error:
                _mLastError = "request finish with error " + (request.Exception != null ? (request.Exception.Message + "\n" + request.Exception.StackTrace) : "No exception");
                state = HttpLoadCode.eError;
                break;
            case HTTPRequestStates.Aborted:
                _mLastError = "request aborted!";
                state = HttpLoadCode.eAborted;
                break;
            case HTTPRequestStates.ConnectionTimedOut:
                _mLastError = "connection timed out";
                state = HttpLoadCode.eConnectionTimedOut;
                break;
            case HTTPRequestStates.TimedOut:
                _mLastError = "connection timed out";
                state = HttpLoadCode.eTimedOut;
                break;
            default:
                if(response==null)
                {
                    state = HttpLoadCode.eUnreachable;
                    _mLastError = "host unreachable with error" + (request.Exception != null ? (request.Exception.Message + "\n" + request.Exception.StackTrace) : "no exception");
                    break;
                }
                else
                {
                    if(request.UseStreaming)
                    {
                        OnResponseStream(response);
                    }
                    return;
                }    
        }
    }
}
