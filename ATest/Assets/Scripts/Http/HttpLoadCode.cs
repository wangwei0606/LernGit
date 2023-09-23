using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum HttpLoadCode
{
    eOK,
    eNull,
    eHttpProcessing,
    eFinish,
    eDownloadOK,
    eFileExists,
    eUnreachable,
    eError,
    eAborted,
    eConnectionTimedOut,
    eTimedOut,
    eDontSupportResume,
    eFileLengthError,
    eCrcError,
    eUncompressError
}
public class HttpTipsCode
{
    public const int Download = 20000;
    public const int UnZip = 20001;
}
