using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LDFSFileTools
{
    private static LDFSFileWriter GetWriter(string filePath)
    {
        return new LDFSFileWriter(new FileStream(filePath, FileMode.Create, FileAccess.Write));
    }

    public static void WriteFilesToPkg(string pkgFile,string rootPath,List<string> files)
    {
        if(FileUtils.IsFileExists(pkgFile))
        {
            FileUtils.DelFile(pkgFile);
        }
        FileUtils.CheckFilePath(pkgFile);
        LDFSFileWriter writer = GetWriter(pkgFile);
        writer.WriteFiles(rootPath, files);
        writer.Dispose();
    }
}