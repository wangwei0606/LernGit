using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CmdTools
{
    public static string Excute(string cmd,string arguments)
    {
        var process = new Process {
            StartInfo=
            {
                FileName=cmd,
                Arguments=arguments,
                UseShellExecute=false,
                RedirectStandardOutput=true,
                CreateNoWindow=true
            }
        };
        process.Start();
        var output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return output;
    }
}
