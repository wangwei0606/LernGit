using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System;

public class ZipClass 
{
    List<string> files = new List<string>();
    List<string> paths = new List<string>();
    public static byte[] Compress(byte[] binary)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using (GZipOutputStream gzip = new GZipOutputStream(ms))
            {
                gzip.Write(binary, 0, binary.Length);
                gzip.Close();
                byte[] press = ms.ToArray();
                return press;
            }
        }
    }

    public static byte[] DeCompress(byte[] press)
    {
        using (GZipInputStream gzip = new GZipInputStream(new MemoryStream(press)))
        {
            MemoryStream re = new MemoryStream();
            int count = 0;
            byte[] data = new byte[1024];
            while((count=gzip.Read(data,0,data.Length))!=0)
            {
                re.Write(data, 0, count);
            }
            return re.ToArray();
        }
    }

    public void ZipFile(string fileToZip,string zipedFile,int compressionLevel,int blockSize)
    {
        if(!File.Exists(fileToZip))
        {
            throw new FileNotFoundException("file not found " + fileToZip);
        }
        FileStream streamToZip = new FileStream(fileToZip, FileMode.Open, FileAccess.Read);
        FileStream zipFile = File.Create(zipedFile);
        ZipOutputStream zipStream = new ZipOutputStream(zipFile);
        ZipEntry zipEntry = new ZipEntry(fileToZip);
        zipStream.PutNextEntry(zipEntry);
        zipStream.SetLevel(compressionLevel);
        byte[] buffer = new byte[blockSize];
        int size = streamToZip.Read(buffer, 0, buffer.Length);
        zipStream.Write(buffer, 0, size);
        try
        {
            while(size<streamToZip.Length)
            {
                int sizeRead = streamToZip.Read(buffer, 0, buffer.Length);
                zipStream.Write(buffer, 0, sizeRead);
                size += sizeRead;
            }
        }
        catch(Exception ex)
        {
            GC.Collect();
            throw ex;
        }
        zipStream.Finish();
        zipStream.Close();
        streamToZip.Close();
        GC.Collect();
    }

    public void ZipFileFromDirectory(List<ZipData> datas,string destinationPath,int compressLevel)
    {
        ZipOutputStream outPutStream = new ZipOutputStream(File.Create(destinationPath));
        outPutStream.SetLevel(compressLevel);
        for(int i=0;i<datas.Count;i++)
        {
            foreach(string file in datas[i].files)
            {
                FileStream fileStream = File.OpenRead(file);
                byte[] buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, buffer.Length);
                ZipEntry entry = new ZipEntry(file.Replace(datas[i].pathMark, string.Empty));
                entry.DateTime = DateTime.Now;
                entry.Size = fileStream.Length;
                fileStream.Close();
                outPutStream.PutNextEntry(entry);
                outPutStream.Write(buffer, 0, buffer.Length);
            }
            files.Clear();
            foreach(string emptyPath in datas[i].paths)
            {
                ZipEntry entry = new ZipEntry(emptyPath.Replace(datas[i].pathMark, string.Empty) + "/");
                outPutStream.PutNextEntry(entry);
            }
        }
        this.paths.Clear();
        outPutStream.Finish();
        outPutStream.Close();
        GC.Collect();
    }

    private void GetAllDirectories(string rootPath)
    {
        string[] subPaths = Directory.GetDirectories(rootPath);
        foreach(string path in subPaths)
        {
            GetAllDirectories(path);
        }
        string[] files = Directory.GetFiles(rootPath);
        foreach(string file in files)
        {
            this.files.Add(file);
        }
        if(subPaths.Length==files.Length && files.Length==0)
        {
            this.paths.Add(rootPath);
        }
    }

    public List<string> UnZip(string zipfilepath,string unzippath)
    {
        List<string> unzipFiles = new List<string>();
        if(unzippath.EndsWith("\\")==false || unzippath.EndsWith(":\\")==false)
        {
            unzippath += "\\";
        }
        ZipInputStream s = new ZipInputStream(File.OpenRead(zipfilepath));
        ZipEntry theEntry;
        while((theEntry=s.GetNextEntry())!=null)
        {
            string directoryName = Path.GetDirectoryName(unzippath);
            string fileName = Path.GetFileName(theEntry.Name);
            if(!string.IsNullOrEmpty(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            if(fileName!=string.Empty)
            {
                if(theEntry.CompressedSize==0)
                {
                    break;
                }
                directoryName = Path.GetDirectoryName(unzippath + theEntry.Name);
                Directory.CreateDirectory(directoryName);
                unzipFiles.Add(unzippath + theEntry.Name);
                FileStream streamWriter = File.Create(unzippath + theEntry.Name);
                int size = 2048;
                byte[] data = new byte[2048];
                while(true)
                {
                    size = s.Read(data, 0, data.Length);
                    if(size>0)
                    {
                        streamWriter.Write(data, 0, size);
                    }
                    else
                    {
                        break;
                    }
                }
                streamWriter.Close();
            }
        }
        s.Close();
        GC.Collect();
        return unzipFiles;
    }
}
