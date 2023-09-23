using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FileOption
{
    public FileOption()
    {

    }

    public static void UpdateCmdFile(string filePath,string content)
    {
        if(FileUtils.IsFileExists(filePath))
        {
            string oldCode = FileUtils.LoadFile(filePath);
            content = PassLine(content, 3);
            FileUtils.SaveFile(filePath, oldCode + "\r\n" + content);
        }
        else
        {
            FileUtils.SaveFile(filePath, content);
        }
    }

    public static void UpdateConstantControl(string constantPath,string modulusControlName)
    {
        string oldCode = null;
        if(FileUtils.IsFileExists(constantPath))
        {
            oldCode = FileUtils.LoadFile(constantPath);
        }
        else
        {
            oldCode = "AppModulusConst={}\r\n";
        }
        string content = string.Format("AppModulusConst.{0}=\"{1}\"  ", modulusControlName, modulusControlName);
        if(oldCode.IndexOf(content)==-1)
        {
            FileUtils.SaveFile(constantPath, oldCode + "\r\n" + content);
        }
        else
        {

        }
    }

    public static string PassLine(string content,int count)
    {
        string[] lines = content.Split('\r');
        string res = "";
        for(int i=count;i<lines.Length;i++)
        {
            res += (lines[i] + "\r");
        }
        return res;
    }
}