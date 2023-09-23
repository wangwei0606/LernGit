using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;
using System.Runtime.InteropServices;

internal interface IFileHandler
{
    bool IsFileInSide(string absFile);
    byte[] LoadInSideBytes(string absFile);
    bool IsFileExists(string wholeFile, string absFile);
    byte[] LoadFileBytes(string wholeFile, string absFile);
    string LoadFile(string wholeFile, string absFile);
}

internal class BaseFileHandler : IFileHandler
{
    public virtual bool IsFileExists(string wholeFile, string absFile)
    {
        if(!File.Exists(wholeFile))
        {
            wholeFile = Path.Combine(Application.streamingAssetsPath, absFile);
        }
        return File.Exists(wholeFile);
    }

    public virtual bool IsFileInSide(string absFile)
    {
        string wholeFile = Path.Combine(Application.streamingAssetsPath, absFile);
        return File.Exists(wholeFile);
    }

    public virtual string LoadFile(string wholeFile, string absFile)
    {
        if(!File.Exists(wholeFile))
        {
            wholeFile = Path.Combine(Application.streamingAssetsPath, absFile);
        }
        if(File.Exists(wholeFile))
        {
            using (StreamReader sr = File.OpenText(wholeFile))
            {
                return sr.ReadToEnd();
            }
        }
        else
        {
            return string.Empty;
        }
    }

    public virtual byte[] LoadFileBytes(string wholeFile, string absFile)
    {
        if(!File.Exists(wholeFile))
        {
            wholeFile = Path.Combine(Application.streamingAssetsPath, absFile);
        }
        if(File.Exists(wholeFile))
        {
            return File.ReadAllBytes(wholeFile);
        }
        else
        {
            return null;
        }
    }

    public virtual byte[] LoadInSideBytes(string absFile)
    {
        if(IsFileInSide(absFile))
        {
            string wholeFile = Path.Combine(Application.streamingAssetsPath, absFile);
            return File.ReadAllBytes(wholeFile);
        }
        return null;
    }
}

internal class AndroidFileHandler:BaseFileHandler
{
    private static AndroidJavaClass _current;
    static AndroidFileHandler()
    {

    }
    public static AndroidJavaClass Current()
    {
        if(Application.platform==RuntimePlatform.Android)
        {
            if(_current==null)
            {
                _current = new AndroidJavaClass("com.dysgd.kkludotpdz.application.KKApplication");
            }
            return _current;
        }
        else
        {
            return null;
        }
    }
    private static byte[] _getNativeStreamFromAssets(string absFile)
    {
        AndroidJavaClass jclass = Current();
        if(jclass!=null)
        {
            return jclass.CallStatic<byte[]>("getNativeStreamFromAssets", absFile);
        }
        return null;
    }
    private static int _getFileSize(string absFile)
    {
        AndroidJavaClass jclass = Current();
        if(jclass!=null)
        {
            return jclass.CallStatic<int>("getFileSize", absFile);
        }
        return -1;
    }
    public override bool IsFileInSide(string absFile)
    {
        return _getFileSize(absFile) >= 0;
    }
    public override byte[] LoadInSideBytes(string absFile)
    {
        if(IsFileInSide(absFile))
        {
            int size = _getFileSize(absFile);
            if(size>0)
            {
                byte[] bytes = _getNativeStreamFromAssets(absFile);
                return bytes;
            }
            if(size==0)
            {
                return System.Text.Encoding.Default.GetBytes("");
            }
        }
        return null;
    }
    public override bool IsFileExists(string wholeFile, string absFile)
    {
        bool isExists = true;
        if(!File.Exists(wholeFile))
        {
            int fileSize = _getFileSize(absFile);
            isExists = fileSize > 0;
        }
        return isExists;
    }
    public override byte[] LoadFileBytes(string wholeFile, string absFile)
    {
        if(!File.Exists(wholeFile))
        {
            int size = _getFileSize(absFile);
            if(size>0)
            {
                byte[] bytes = _getNativeStreamFromAssets(absFile);
                return bytes;
            }
            if(size==0)
            {
                return System.Text.Encoding.Default.GetBytes("");
            }
            return null;
        }
        return base.LoadFileBytes(wholeFile, absFile);
    }
    public override string LoadFile(string wholeFile, string absFile)
    {
        byte[] bytes = LoadFileBytes(wholeFile, absFile);
        if(bytes==null)
        {
            return string.Empty;
        }
        return System.Text.Encoding.Default.GetString(bytes);
    }
}

public class FileProxy 
{
    private static IFileHandler handler = null;
    private static LDFSProxy _luaFs;
    private static int Head_Size = sizeof(Int32);
    public static void Initilize(string filePath)
    {
        _luaFs = LDFSProxy.GetReader(filePath);
    }
    static FileProxy()
    {
        switch(Application.platform)
        {
            case RuntimePlatform.Android:
                handler = new AndroidFileHandler();
                break;
            default:
                handler = new BaseFileHandler();
                break;
        }
    }
    public static void Release()
    {
        if(_luaFs!=null)
        {
            _luaFs.DisPose();

        }
        _luaFs = null;
    }
    public static bool IsFileInSide(string absFile)
    {
        if(handler==null)
        {
            return false;
        }
        return handler.IsFileInSide(absFile);
    }
    public static byte[] LoadInSideBytes(string absFile)
    {
        if(handler==null)
        {
            return null;
        }
        return handler.LoadInSideBytes(absFile);
    }
    public static bool IsFileExists(string wholeFIle,string absFile)
    {
        if(handler.IsFileExists(wholeFIle,absFile))
        {
            return true;
        }
        if(_luaFs==null)
        {
            return false;
        }
        return _luaFs.IsFileExists(absFile);
    }
    public static byte[] LoadFileBytes(string wholeFile,string absFile)
    {
        try
        {
            if(handler.IsFileExists(wholeFile,absFile))
            {
                return GetDecodeBytes(handler.LoadFileBytes(wholeFile, absFile));
            }
            if(_luaFs==null)
            {
                return null;
            }
            return _luaFs.LoadFileBytes(absFile);
        }
        catch(Exception e)
        {
            Debug.LogError("absFile " + absFile + " exception:" + e.StackTrace);
            return null;
        }
    }
    public static string LoadFile(string wholeFile,string absFile)
    {
        try
        {
            if(handler.IsFileExists(wholeFile,absFile))
            {
                string str = GetDecodeStr(handler.LoadFileBytes(wholeFile, absFile));
                return str;
            }
            if(_luaFs==null)
            {
                return string.Empty;
            }
            return _luaFs.LoadFile(absFile);
        }
        catch(Exception e)
        {
            Debug.LogError("absFile " + absFile + " exception:" + e.StackTrace);
            return null;
        }
    }

    private static string GetDecodeStr(byte[] bytes)
    {
        if(bytes==null)
        {
            return string.Empty;
        }
        string str = System.Text.Encoding.UTF8.GetString(GetDecodeBytes(bytes));
        return str;
    }

    private static byte[] GetDecodeBytes(byte[] bytes)
    {
        if(bytes==null)
        {
            return null;
        }
        if(bytes.Length<Head_Size)
        {
            return bytes;
        }
        byte[] byteFlags = new byte[Head_Size];
        Buffer.BlockCopy(bytes, 0, byteFlags, 0, Head_Size);
        int length = BitConverter.ToInt32(byteFlags, 0);
        int leftLen = bytes.Length - Head_Size;
        if(length!=leftLen)
        {
            return bytes;
        }
        byte[] srouceDst = new byte[leftLen];
        Buffer.BlockCopy(bytes, 4, srouceDst, 0, leftLen);
        return DeCompress(srouceDst);
    }

    private static byte[] DeCompress(byte[] srouceDst)
    {
        using (GZipInputStream gzi = new GZipInputStream(new MemoryStream(srouceDst)))
        {
            MemoryStream re = new MemoryStream();
            int count = 0;
            byte[] data = new byte[1024];
            while((count=gzi.Read(data,0,data.Length))!=0)
            {
                re.Write(data, 0, count);
                
            }
            return re.ToArray();
        }
    }
}
