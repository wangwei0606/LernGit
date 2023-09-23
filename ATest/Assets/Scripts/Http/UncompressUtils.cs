using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;

public class UncompressUtils
{
    public static float rate = 0;
    public static bool UnZip(string FileToUpZip,string ZipedFolder,Action<float> processHandle=null)
    {
        ZipConstants.DefaultCodePage = Encoding.UTF8.CodePage;
        bool res = true;
        if(!File.Exists(FileToUpZip))
        {
            return false;
        }
        if(!Directory.Exists(ZipedFolder))
        {
            Directory.CreateDirectory(ZipedFolder);
        }
        ZipInputStream zipInputs = null;
        ZipEntry theEntry = null;
        string fileName = "";
        FileStream file = null;
        int zipLeng = 0;
        int zipIndex = 0;
        float rate = 0;
        try
        {
            file = File.OpenRead(FileToUpZip);
            zipInputs = new ZipInputStream(file);
            while((theEntry=zipInputs.GetNextEntry())!=null)
            {
                if(theEntry.Name!=string.Empty)
                {
                    zipLeng++;
                }
            }
            file = File.OpenRead(FileToUpZip);
            zipInputs = new ZipInputStream(file);
            while((theEntry=zipInputs.GetNextEntry())!=null)
            {
                if(theEntry.Name!=string.Empty)
                {
                    zipIndex++;
                    rate = (float)zipIndex / (float)zipLeng;
                    if(processHandle!=null)
                    {
                        processHandle(rate);
                    }
                    fileName = Path.Combine(ZipedFolder, theEntry.Name);
                    if(fileName.EndsWith("/")||fileName.EndsWith("\\"))
                    {
                        HttpUtils.RecursionCreateFolder(fileName);
                        continue;
                    }
                    HttpUtils.CheckFileSavePath(fileName);
                    using (FileStream streamWriter = File.Create(fileName))
                    {
                        int size = 2048;
                        byte[] data = new byte[size];
                        while(true)
                        {
                            size = zipInputs.Read(data, 0, data.Length);
                            if(size>0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
        catch(Exception e)
        {
            UnityEngine.Debug.LogError(e.StackTrace);
            res = false;
        }
        finally
        {
            if(file!=null)
            {
                file.Close();
                file = null;
            }
            if(theEntry!=null)
            {
                theEntry = null;
            }
            if(zipInputs!=null)
            {
                zipInputs.Close();
                zipInputs = null;
            }
            GC.Collect();
        }
        return res;
    }

    public static bool UnZipFiles(string FileToUnZip,string ZipedFolder,Action<float> processHandler=null)
    {
        ZipConstants.DefaultCodePage = Encoding.UTF8.CodePage;
        bool res = true;
        if(!File.Exists(FileToUnZip))
        {
            return false;
        }
        if(!Directory.Exists(ZipedFolder))
        {
            Directory.CreateDirectory(ZipedFolder);
        }
        ZipInputStream zipInputS = null;
        ZipEntry theEntry = null;
        string fileName;
        FileStream file = null;
        long readSize = 0;
        rate = 0;
        try
        {
            file = File.OpenRead(FileToUnZip);
            zipInputS = new ZipInputStream(file);
            while((theEntry=zipInputS.GetNextEntry())!=null)
            {
                if(theEntry.Name!=string.Empty)
                {
                    fileName = Path.Combine(ZipedFolder, theEntry.Name);
                    if (fileName.EndsWith("/") || fileName.EndsWith("\\"))
                    {
                        FileUtils.RecursionCreateFolder(fileName);
                        continue;
                    }
                    FileUtils.CheckFileSavePath(fileName);
                    using (FileStream streamWriter = File.Create(fileName))
                    {
                        int size = 1024 * 10;
                        byte[] data = new byte[size];
                        while((size=zipInputS.Read(data,0,data.Length))>0)
                        {
                            readSize += size;
                            rate = (float)readSize / (float)zipInputS.Length;
                            streamWriter.Write(data, 0, size);
                            System.Threading.Thread.Sleep(1);
                        }
                    }
                }
            }
        }
        catch(Exception e)
        {
            res = false;
        }
        finally
        {
            if(file!=null)
            {
                file.Close();
                file = null;
            }
            if(theEntry!=null)
            {
                theEntry = null;
            }
            if(zipInputS!=null)
            {
                zipInputS.Close();
                zipInputS = null;
            }
            GC.Collect();
        }
        return res;
    }
}
