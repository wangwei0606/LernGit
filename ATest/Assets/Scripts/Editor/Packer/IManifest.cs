using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManifest
{
    string MainAsset { get; }
    int Priority { get; }
    string ABName { get; }
    void setManifestInfo(string abName, string mainAsset);
    void addAsset(string asset);
    void addDep(string relyABName);
    List<string> getAssets(bool isIncludeMainAsset = true);
    List<string> getDependencie();
}
