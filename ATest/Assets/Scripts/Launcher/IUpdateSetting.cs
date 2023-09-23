using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdateSetting 
{
    string getAppTag();
    string getChannelId();
    string getDeviceId();
    string getStoragePath();
    string getCurVersionUrl();
    string getCurClientVersion();
    string getCurResVersion();
    string getInsideVersionUrl();
    string getInsideClientVersion();
    string getInsideResVersion();
    string getInstallFile();
    bool IsNeedUpdate();
}
