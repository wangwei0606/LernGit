using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BestHTTP;
using System.Text.RegularExpressions;

public class HttpErrorCode
{
    public const int Default = 100000;
    public const int FinishError = 100003;
    public const int Error = 100004;
    public const int Aborted = 100006;
    public const int ConnectionTimeOut = 100007;
    public const int TimeOut = 100008;
}
public class WebTools
{
    public static void HttpReq(string url,HttpResult resultCall,double timeOut=20)
    {
        HTTPRequest req = new HTTPRequest(new Uri(url), HTTPMethods.Get,
                                        (re, resp) =>
                                        {
                                            int code = HttpErrorCode.Default + (int)re.State;
                                        bool isScuss = true;
                                            switch(re.State)
                                            {
                                                case HTTPRequestStates.Finished:
                                                    if(resp.IsSuccess)
                                                    {
                                                        string json = resp.DataAsText.Trim();
                                                        json.Replace("\\n", "");
                                                        json = Unicode2String(json);
                                                        isScuss = !json.Equals("null");
                                                        if(resultCall!=null)
                                                        {
                                                            resultCall(isScuss, code, json);
                                                        }
                                                    }
                                                    break;
                                                case HTTPRequestStates.Error:
                                                    break;
                                                case HTTPRequestStates.Aborted:
                                                    break;
                                                case HTTPRequestStates.ConnectionTimedOut:
                                                    break;
                                                case HTTPRequestStates.TimedOut:
                                                    break;
                                            }
                                            if(code>HttpErrorCode.FinishError)
                                            {
                                                resultCall(false, code, "");
                                            }
                                        });
        req.Timeout = TimeSpan.FromSeconds(timeOut);
        req.Send();
    }
    public static string Unicode2String(string source)
    {
        return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase).Replace(
             source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
    }
    public static void HttpPosReq(string url,HttpResult resultCall,double timeOut=20)
    {
        HTTPRequest req = new HTTPRequest(new Uri(url), HTTPMethods.Post,
                                        (re, resp) => 
                                        {
                                            int code = HttpErrorCode.Default + (int)re.State;
                                            bool isSuss = true;
                                            switch(re.State)
                                            {
                                                case HTTPRequestStates.Finished:
                                                    if(resp.IsSuccess)
                                                    {
                                                        string json = resp.DataAsText.Trim();
                                                        json.Replace("\\n", "");
                                                        json = Unicode2String(json);
                                                        isSuss = !json.Equals("null");
                                                        if(resultCall!=null)
                                                        {
                                                            resultCall(isSuss, code, json);
                                                        }
                                                    }
                                                    break;
                                                case HTTPRequestStates.Error:
                                                case HTTPRequestStates.Aborted:
                                                case HTTPRequestStates.ConnectionTimedOut:
                                                case HTTPRequestStates.TimedOut:
                                                    break;
                                            }
                                            if(code>HttpErrorCode.FinishError)
                                            {
                                                if(resultCall!=null)
                                                {
                                                    resultCall(false, code, "");
                                                }
                                            }
                                        });
        req.Timeout = TimeSpan.FromSeconds(timeOut);
        req.Send();
    }
    public static void HttpPosReq(string url,string paramJson,HttpResult resultCall,double timeOut=20)
    {
        HTTPRequest req = new HTTPRequest(new Uri(url), HTTPMethods.Post,
            (re, resp)=>
            {
                int code = HttpErrorCode.Default + (int)re.State;
                bool isScuss = true;
                switch(re.State)
                {
                    case HTTPRequestStates.Finished:
                        if(resp.IsSuccess)
                        {
                            string json = resp.DataAsText.Trim();
                            json.Replace("\\", "");
                            json = Unicode2String(json);
                            isScuss = !json.Equals("null");
                            if(resultCall!=null)
                            {
                                resultCall(isScuss, code, json);
                            }
                        }
                        break;
                    case HTTPRequestStates.Error:
                    case HTTPRequestStates.Aborted:
                    case HTTPRequestStates.ConnectionTimedOut:
                    case HTTPRequestStates.TimedOut:
                        break;
                }
                if(code>HttpErrorCode.FinishError)
                {
                    if(resultCall!=null)
                    {
                        resultCall(false, code, "");
                    }
                }
            });
        req.Timeout = TimeSpan.FromSeconds(timeOut);
        req.RawData = Encoding.UTF8.GetBytes(paramJson);
        req.Send();
    }
}
