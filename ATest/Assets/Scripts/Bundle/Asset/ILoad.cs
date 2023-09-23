using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public delegate void SyncLoadCallBack(AssetBundle ab, string reason);
internal interface ILoad
{
    AssetBundle getAssetBundle(string absFileName, string url, SyncLoadCallBack callback = null);
}
