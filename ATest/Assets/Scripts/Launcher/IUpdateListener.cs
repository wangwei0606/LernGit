using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IUpdateListener 
{
    void onPatchedProgress(float progress, string tips);
    void onPatchedTips(string tips);
    void OnPatchedWarning(string tips, Action handle);
    void onPatched(string clientVersion, string resVersion);
    void onFinish(string clientVersion, string resVersion);
    void installApk(string appFile);
    void onClearAppCache();
    void getWWWData(string url, Action<bool, int, string> hand);
}
