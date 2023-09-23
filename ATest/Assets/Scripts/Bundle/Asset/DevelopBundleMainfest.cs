#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DevelopBundleMainfest
{
    private static DevelopBundleMainfest _instance = null;
    public static string ManifestSuffix = "_develop.mainfest";
    private Dictionary<string, string> _manifest = new Dictionary<string, string>();
    public static DevelopBundleMainfest Instance
    {
        get
        {
            if(_instance==null)
            {
                _instance = new DevelopBundleMainfest();
            }
            return _instance;
        }
    }
    private DevelopBundleMainfest()
    {

    }
    private bool init(string file)
    {
        file = file + ManifestSuffix;
        string str = FileUtils.LoadFile(file);
        if(string.IsNullOrEmpty(str))
        {
            return false;
        }
        JsonData data = Json.ToObject(str);
        if(data==null)
        {
            return false;
        }
        var lst = data.GetKeys();
        for(int i=0;i<lst.Count;i++)
        {
            parser(data.Get(lst[i]));
        }
        return true;
    }
    private void parser(JsonData data)
    {
        if(data==null)
        {
            return;
        }
        string abName = data.Get("name").ToString();
        if(!_manifest.ContainsKey(abName))
        {
            string asset = data.Get("asset").ToString();
            _manifest.Add(abName, asset);
        }
    }
    public string getAsset(string abName)
    {
        string dps = null;
        if(_manifest.ContainsKey(abName))
        {
            dps = _manifest[abName];
        }
        return dps;
    }
    public void clear()
    {
        _manifest.Clear();
    }
    public static string GetAssetPath(string abName)
    {
        return Instance.getAsset(abName);
    }
    public static DevelopBundleMainfest Initlize(string file)
    {
        bool init = Instance.init(file);
        return init ? Instance : null;
    }
    public static void Dispose()
    {
        if(_instance!=null)
        {
            _instance.clear();
        }
        _instance = null;
    }
}
#endif