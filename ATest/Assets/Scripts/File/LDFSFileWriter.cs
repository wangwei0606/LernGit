using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class LDFSFileWriter
{
    private FileStream _fileHandler = null;
    private LDFSFileCode code = null;
    public LDFSFileWriter(FileStream handler)
    {
        code = new LDFSFileCode();
        _fileHandler = handler;
    }
    protected virtual void _WriteBuff(byte[] bytes)
    {
        _fileHandler.Write(bytes, 0, bytes.Length);
    }

    protected virtual void WriteHead(LDFSHeader header)
    {
        _fileHandler.Seek(0, SeekOrigin.Begin);
        _WriteBuff(BitConverter.GetBytes(header.FileSize));
        _WriteBuff(BitConverter.GetBytes(header.HeadPosition));
    }

    protected virtual void WriteFileInfos(List<LDFSFileInfo> infos)
    {
        ByteArray array = new ByteArray();
        array.WriteInt(infos.Count);
        foreach(var info in infos)
        {
            array.WriteUTF(info.FilePath);
            array.WriteLong(info.FilePosition);
            array.WriteLong(info.FileSize);
        }
        _WriteBuff(array.Buffer);
    }

    protected virtual void WriteContext(byte[] bytes)
    {
        _WriteBuff(bytes);
    }

    protected virtual byte[] GetFileBytes(string path)
    {
        if(!File.Exists(path))
        {
            return null;
        }
        byte[] buff = File.ReadAllBytes(path);
        byte[] tobuff = code.Encrypto(buff);
        return tobuff;
    }

    public virtual long GetFilePosition()
    {
        return _fileHandler.Position;
    }

    public virtual long GetFileLength()
    {
        return _fileHandler.Length;
    }

    public virtual void WriteFiles(string rootPath,List<string> files)
    {
        rootPath = rootPath.Replace("\\", "/");
        LDFSHeader header = new LDFSHeader(0, 0);
        WriteHead(header);
        List<LDFSFileInfo> infos = new List<LDFSFileInfo>();
        foreach(string file in files)
        {
            byte[] bytes = GetFileBytes(file.Replace("\\", "/"));
            if(bytes!=null)
            {
                LDFSFileInfo info = new LDFSFileInfo();
                info.FilePath = file.Replace("\\", "/").Replace(rootPath, "").ToLower();
                if (info.FilePath.StartsWith("/"))
                {
                    info.FilePath = info.FilePath.Substring(1);
                }
                info.FilePosition = GetFilePosition();
                info.FileSize = bytes.LongLength;
                infos.Add(info);
                WriteContext(bytes);

            }
        }
        header.HeadPosition = GetFilePosition();
        WriteFileInfos(infos);
        header.FileSize = GetFileLength();
        WriteHead(header);
    }

    public virtual void Dispose()
    {
        if(_fileHandler!=null)
        {
            _fileHandler.Flush();
            _fileHandler.Close();
            _fileHandler.Dispose();
        }
        _fileHandler = null;
    }

    public static LDFSFileWriter GetWriter(string path)
    {
        if(File.Exists(path))
        {
            File.Delete(path);
        }
        return new LDFSFileWriter(new FileStream(path, FileMode.Create, FileAccess.Write));
    }
}
