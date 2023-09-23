using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class LDFSFileReader
{
    private string _FilePath = "";
    private FileStream _FileHandle = null;
    private LDFSHeader _Header = null;
    private Dictionary<string, LDFSFileInfo> _Map;
    private LDFSFileCode code = null;
    public LDFSFileReader(FileStream fileHandle)
    {
        code = new LDFSFileCode();
        _Header = new LDFSHeader(0, 0);
        _Map = new Dictionary<string, LDFSFileInfo>();
        _FileHandle = fileHandle;
    }
    protected virtual bool _Init()
    {
        bool isInit = _InitHead();
        if(isInit)
        {
            _InitFileInfos();
        }
        return isInit;
    }
    protected virtual bool _InitHead()
    {
        byte[] lbs = _Read(_FileHandle.Position, sizeof(long), false);
        _Header.FileSize = BitConverter.ToInt64(lbs, 0);
        if(_Header.FileSize != _FileHandle.Length)
        {
            return false;
        }
        lbs = _Read(_FileHandle.Position, sizeof(long), false);
        _Header.HeadPosition = BitConverter.ToInt64(lbs, 0);
        return true;
    }
    protected virtual void _InitFileInfos()
    {
        byte[] bytes = _Read(_Header.HeadPosition, _FileHandle.Length - _Header.HeadPosition, false);
        ByteArray array = new ByteArray(bytes);
        int count = array.ReadInt();
        for(int i=0;i<count;i++)
        {
            LDFSFileInfo info = new LDFSFileInfo();
            info.FilePath = array.ReadUTFString();
            info.FilePosition = array.ReadLong();
            info.FileSize = array.ReadLong();
            if(!_Map.ContainsKey(info.FilePath))
            {
                _Map.Add(info.FilePath, info);
            }
        }
    }
    protected virtual byte[] _Read(long Position,long size,bool needDecode)
    {
        byte[] bytes = new byte[size];
        _FileHandle.Seek(Position, SeekOrigin.Begin);
        _FileHandle.Read(bytes, 0, bytes.Length);
        if(!needDecode)
        {
            return bytes;
        }
        byte[] buff = code.Decrypto(bytes);
        return buff;
    }
    public static void WriteFile(string fileName,string context)
    {
        string log = string.Format("{0}{1}{2}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "[Info]", context);
        using (StreamWriter sw = new StreamWriter(File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)))
        {
            sw.WriteLine(log);
            sw.Flush();
            sw.Dispose();
        }
    }

    protected virtual byte[] _ReadBytes(LDFSFileInfo info,bool needDecode=true)
    {
        return _Read(info.FilePosition, info.FileSize, needDecode);
    }
    protected virtual string _Read(LDFSFileInfo info)
    {
        byte[] bytes = _ReadBytes(info);
        if(bytes==null)
        {
            return string.Empty;
        }
        return System.Text.Encoding.Default.GetString(bytes);
    }

    public virtual bool IsFileExists(string absFile)
    {
        absFile = absFile.ToLower();
        return _Map.ContainsKey(absFile);
    }

    public virtual byte[] LoadFileBytes(string absFile)
    {
        absFile = absFile.ToLower();
        if(!_Map.ContainsKey(absFile))
        {
            return null;
        }
        return _ReadBytes(_Map[absFile]);
    }

    public virtual string LoadFile(string absFile)
    {
        absFile = absFile.ToLower();
        if (!_Map.ContainsKey(absFile))
        {
            return string.Empty;
        }
        return _Read(_Map[absFile]);
    }
    public virtual void DisPose()
    {
        _FileHandle.Dispose();
        _FileHandle = null;
    }
}