using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LDFSProxy : LDFSFileReader
{
    public LDFSProxy(FileStream fileHandle) : base(fileHandle)
    {
    }
    public static LDFSProxy GetReader(string path)
    {
        if(!File.Exists(path))
        {
            return null;
        }
        LDFSProxy reader = new LDFSProxy(new FileStream(path, FileMode.Open, FileAccess.Read));
        if(!reader._Init())
        {
            reader.DisPose();
            reader = null;
        }
        return reader;
    }
}
