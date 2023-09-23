using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UpdateCode
{
    private static int Initial = 10000;
    private static int Queued = Initial+1;
    private static int Processing = Initial + 2;
    private static int Finish = Initial + 3;
    private static int Error = Initial + 4;
    private static int Aborted = Initial + 5;
    private static int ConnectionTimedOut = Initial + 6;
    private static int TimeOut = Initial + 7;
    private static int LoadIntial = 20000;
    private static int eUnreachable = LoadIntial + 6;
    private static int eError = LoadIntial + 7;
    private static int eAborted = LoadIntial + 8;
    private static int eConnectionTimeOut = LoadIntial + 9;
    private static int eTimeOut = LoadIntial + 10;
    private static int eDontSupportResume = LoadIntial + 11;
    private static int eFileLengthError = LoadIntial + 12;
    private static int eCrcError = LoadIntial + 13;
    private static int eUncompressError = LoadIntial + 14;
    private static Dictionary<int, string> codeError = new Dictionary<int, string>() { };
    static UpdateCode()
    {
        //http 错误
        codeError.Add(Error, "当前网络不稳定哦>_<");
        codeError.Add(Aborted, "当前网络不稳定啊>_<");
        codeError.Add(ConnectionTimedOut, "当前网络很不稳定吖>_<");
        codeError.Add(TimeOut, "当前网络超级不稳定>_<");
        //下载错误
        codeError.Add(eUnreachable, "当前网络不稳定哦 ?_?");
        codeError.Add(eError, "当前网络不稳定哦 ?_?!");
        codeError.Add(eAborted, "当前网络不稳定哦 ?_?!!");
        codeError.Add(eConnectionTimeOut, "当前网络很不稳定吖 ?_?");
        codeError.Add(eTimeOut, "当前网络超级不稳定 ?_?");
        codeError.Add(eDontSupportResume, "资源服务器不支持断点续传 ?_?");
        codeError.Add(eFileLengthError, "网络不稳定，没有下载到完整文件 ?_?");
        codeError.Add(eCrcError, "网络不稳定,下载的文件不对 ?_?");
        codeError.Add(eUncompressError, "解压资源文件失败 ?_?");
    }
    public static string GetHttpError(int code)
    {
        if(codeError.ContainsKey(code))
        {
            return codeError[code];
        }
        return code.ToString();
    }
    public static string GetDownloadError(int code)
    {
        code = LoadIntial + code;
        if(codeError.ContainsKey(code))
        {
            return codeError[code];
        }
        return code.ToString();
    }
}
