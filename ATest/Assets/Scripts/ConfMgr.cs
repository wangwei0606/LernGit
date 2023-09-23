using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Reflection;

public class ConfMgr
{
    private static ConfMgr _instance;

    private Dictionary<string, Dictionary<string, object>> _configData = new Dictionary<string, Dictionary<string, object>>();
    private Dictionary<string, string> _configDataJson = new Dictionary<string, string>();
    private readonly int OneFrameParse = 300;
    private bool _isAllComplete = false;
    private int decodeCount = 0;
    private int curDecodeIndex=0;
    private int curDataCount = 0;
    private int curDataIndex = 0;
    private ByteArray _jsondata;
    private Dictionary<string, object> _objdic;
    private ConfBase _curConfigObj;
    private Action _complete;
    private List<Type> _decodes;
    public static ConfMgr Instance
    {
        get
        {
            if(_instance==null)
            {
                _instance = new ConfMgr();
            }
            return _instance;
        }
    }

    private void Dispose()
    {
        _configData.Clear();
    }

    public static void Release()
    {
        if(_instance!=null)
        {
            _instance.Dispose();
        }
        _instance = null;
    }

    public void Create(Action complete)
    {
        _complete = complete;
        _decodes = ReflectionUtil.GetDecodeByType(typeof(ConfBase));
        decodeCount = _decodes.Count;
        resetConfigObj();
        AssetThread.LoadFileFromAnsyc(parserConfig());
    }

    IEnumerator parserConfig()
    {
        while(true)
        {
            yield return new WaitForEndOfFrame();
            //Debug.LogError(_jsondata != null);
            if (_jsondata!=null)
            {
                for(int i=0;i<OneFrameParse;i++)
                {
                    string ID = _jsondata.ReadUTFString();
                    _configDataJson.Add(_curConfigObj.GetType().FullName + "_" + ID, _jsondata.ReadUTFString());
                    curDataIndex++;
                    AppCoreExtend.Dispatch(LoadingCmd.Loading_Progress, 0.6 + ((curDecodeIndex - 1) * 1.0f / decodeCount) + (curDataIndex * 1.0f / curDataCount) * (1.0f / decodeCount), "loading resources");
                    //Debug.LogError(curDataIndex + "      " + curDataCount);
                    if (curDataIndex==curDataCount)
                    {
                        resetConfigObj();
                        break;
                    }
                }
            }
            else
            {
                resetConfigObj();
            }
            if(_isAllComplete)
            {
                _complete();
                break;
            }
        }
    }

    private void resetConfigObj()
    {
        if(curDecodeIndex>=decodeCount)
        {
            _isAllComplete = true;
            return;
        }
        _curConfigObj = (ConfBase)_decodes[curDecodeIndex].GetConstructor(new Type[] { }).Invoke(new Type[] { });
        string data = string.Empty;
        string name = _curConfigObj.GetType().Name;
        name = name + ".txt";
        string absFile = Path.Combine(AppSetting.AbsConfigPath, name);
        switch(Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                absFile = absFile.ToLower();
                name = name.ToLower();
                break;
        }
        string fullPath = Path.Combine(AppPath.ConfigPath, name);
        //Debug.LogError(fullPath);
        //Debug.LogError(absFile);
        if (!FileProxy.IsFileExists(fullPath,absFile))
        {
            //Debug.LogError("666");
            _jsondata = null;
            return;
        }
        byte[] bytes = FileProxy.LoadFileBytes(fullPath, absFile);
        _jsondata = new ByteArray(bytes);
        curDataCount = _jsondata.ReadInt();
        curDataIndex = 0;
        _objdic = new Dictionary<string, object>();
        curDecodeIndex++;
    }

    private T Create<T>(IDictionary data) where T:ConfBase,new()
    {
        T obj = Activator.CreateInstance<T>();
        Type type = typeof(T);
        FieldInfo[] fields = type.GetFields();
        foreach(FieldInfo f in fields)
        {
            if(data.Contains(f.Name))
            {
                if(data[f.Name] is Array && ((Array)data[f.Name]).Length==0)
                {
                    continue;
                }
                f.SetValue(obj, data[f.Name]);
            }
        }
        PropertyInfo[] pros = type.GetProperties();
        foreach(PropertyInfo p in pros)
        {
            if(data.Contains(p.Name))
            {
                p.SetValue(obj, data[p.Name], null);
            }
        }
        return obj;
    }

    private void getDataConfig<T>(Action<string> callback)
    {
        string data = string.Empty;
        string name = typeof(T).Name;
        name = name + ".txt";
        string absFile = Path.Combine(AppSetting.AbsConfigPath, name);
        switch(Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                absFile = absFile.ToLower();
                name = name.ToLower();
                break;
        }
        string fullPath = Path.Combine(AppPath.ConfigPath, name);
        callback(FileProxy.LoadFile(fullPath, absFile));
    }

    private void getDataConfig(ConfBase config,Action<string> callback)
    {
        string data = string.Empty;
        string name = config.GetType().Name;
        name = name + ".txt";
        string absFile = Path.Combine(AppSetting.AbsConfigPath, name);
        switch(Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                absFile = absFile.ToLower();
                name = name.ToLower();
                break;
        }
        string fullPath = Path.Combine(AppPath.ConfigPath, name);
        if(!FileProxy.IsFileExists(fullPath,absFile))
        {
            callback(null);
            return;
        }
        callback(FileProxy.LoadFile(fullPath, absFile));
    }

    public bool ContainsKey<T>(int id)where T:ConfBase,new()
    {
        Type type = typeof(T);
        if(_configData.ContainsKey(type.FullName))
        {
            Dictionary<string, object> t = _configData[type.FullName];
            string key = string.Format("{0}", id);
            return t.ContainsKey(key);
        }
        return false;
    }

    public bool ContainsKey<T>(string key)where T:ConfBase,new ()
    {
        Type type = typeof(T);
        if(_configData.ContainsKey(type.FullName))
        {
            Dictionary<string, object> t = _configData[type.FullName];
            return t.ContainsKey(key);
        }
        return false;
    }

    public T Select<T>(string key) where T:ConfBase,new()
    {
        Type type = typeof(T);
        T obj = default(T);
        string jsonKey = type.FullName + "_" + key;
        if(!_configData.ContainsKey(type.FullName))
        {
            _configData.Add(type.FullName, new Dictionary<string, object>());
        }
        Dictionary<string, object> t = _configData[type.FullName];
        if(!t.ContainsKey(key))
        {
            if(!_configDataJson.ContainsKey(jsonKey))
            {
                return null;
            }
            string jsonStr = _configDataJson[jsonKey];
            ConfBase temp = null;
            try
            {
                temp = (ConfBase)Json.GetT(type, Json.ToObject(jsonStr));
                t.Add(key, temp);
                _configData[type.FullName] = t;
                _configDataJson.Remove(jsonKey);
            }
            catch(Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        obj = (T)t[key];
        return obj;
    }

    public T Select<T>(int id)  where T:ConfBase,new()
    {
        Type type = typeof(T);
        T obj = default(T);
        if(_configData.ContainsKey(type.FullName))
        {
            Dictionary<string, object> t = _configData[type.FullName];
            string key = string.Format("{0}", id);
            if (t.ContainsKey(key))
            {
                obj = (T)t[key];
            }
            else
            {

            }
        }
        else
        {

        }
        return obj;
    }

    public List<T> Select<T>()
    {
        Type type = typeof(T);
        List<T> list = new List<T>();
        if(_configData.ContainsKey(type.FullName))
        {
            foreach(KeyValuePair<string,object> k in _configData[type.FullName])
            {
                list.Add((T)k.Value);
            }
        }
        return list;
    }

    public List<T> Select<T>(Func<T,bool> func)
    {
        Type type = typeof(T);
        List<T> list = new List<T>();
        if(_configData.ContainsKey(type.FullName))
        {
            foreach(KeyValuePair<string,object> k in _configData[type.FullName])
            {
                if(func((T)k.Value))
                {
                    list.Add((T)k.Value);
                }
            }
        }
        return list;
    }

    public static T Get<T>(string key) where T:ConfBase,new()
    {
        return Instance.Select<T>(key);
    }

    public static T Get<T>(int id) where T:ConfBase,new()
    {
        return Instance.Select<T>(id.ToString());
    }
}
