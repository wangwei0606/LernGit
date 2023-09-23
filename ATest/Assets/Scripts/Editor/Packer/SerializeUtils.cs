using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class SerializeUtils
{
    public static void SerializeList(string path,bool isOverride)
    {
        if(string.IsNullOrEmpty(path))
        {
            UnityEngine.Debug.LogError("文件不存在");
            return;
        }
        if(!File.Exists(path))
        {
            UnityEngine.Debug.LogError("文件不存在");
            return;
        }
        string fileData = File.ReadAllText(path);
        UnityEngine.Debug.LogError(path);
        UnityEngine.Debug.LogError(fileData);
        JsonData jsondata = Json.ToObject(fileData);
        var lst = jsondata.GetKeys();
        ByteArray bytes = new ByteArray();
        bytes.WriteInt(lst.Count);
        for(int i=0;i<lst.Count;i++)
        {
            JsonData child = jsondata.Get(lst[i]);
            string strData = Json.ToJson(child);
            bytes.WriteUTF(strData);
        }
        if(isOverride)
        {
            File.Delete(path);
        }
        else
        {
            path = path + "_bin";
            if(File.Exists(path))
            {
                File.Delete(path);
            }
        }
        File.WriteAllBytes(path, bytes.Buffer);
    }
}