using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PackageInfo
{
    public string srcver;
    public string dstver;
    public int size;
    public string package;
    public string filecrc;
}

public class UpdateInfo
{
    public Int64 srcVer;
    public Int64 dstVer;
    public int size;
    public string package;
    public string fileCrc;
    public UpdateInfo(PackageInfo info)
    {
        srcVer = VersionHelper.strToVersion(info.srcver);
        dstVer = VersionHelper.strToVersion(info.dstver);
        size = info.size;
        package = info.package;
        fileCrc = "";
    }
}

public class UpdateInfoLst
{
    public List<PackageInfo> lst;
}

public class UpdateLst
{
    public Int64 forcesize;
    public bool forceupdate;
    public string forceurl;
    public bool ignoreupdate;
    public Dictionary<Int64, UpdateInfo> packageLst;
    public UpdateLst(UpdateInfoLst infos)
    {
        packageLst = new Dictionary<long, UpdateInfo>();
        for(int i=0;i<infos.lst.Count;i++)
        {
            UpdateInfo info = new UpdateInfo(infos.lst[i]);
            if(packageLst.ContainsKey(info.srcVer))
            {
                continue;
            }
            packageLst.Add(info.srcVer, info);
        }
    }
}

public class VersionCfg
{
    public string clientVersion = "";
    public string clientUrl = "";
    public string resourceVersion = "";
    public string resourceUrl = "";
    public string is_enforce_update = "";
    public string download_url = "";
    public string updateListUrl = "";
}
