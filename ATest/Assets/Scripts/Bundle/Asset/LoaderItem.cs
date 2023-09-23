using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

struct LoaderItem
{
    public string Path;
    public LoadType Type;
    public bool IsAsync;
    public bool IsBuildIn;
    public bool IsPriority;
    public Action<string, Asset> complete;
    public Action<string, int, int> Progress;
    public Action<string> Fail;
    public int ResourceCurrentCount;
    public int ResourceTotal;
    public List<LoaderResources> resources;
}
struct LoaderResources
{
    public string RelativePath
    {
        get;
        set;
    }
}