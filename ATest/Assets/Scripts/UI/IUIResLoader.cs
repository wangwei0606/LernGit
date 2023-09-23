using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IUIResLoader
{
    string LoadFile(string file);
    void LoadSprite(string spriteName, string altasSrouce, Action<string, Sprite> onComplete);
    void LoadSpriteByWWW(string url, Action<string, Sprite> onComplete);
    void ReleaseSprite(Sprite sp);
    void ReleaseSprite(string spriteName);
    void RemoveCacheSprite(string spriteName, string altasSrouce);
    void LoadModel(string modelPath, Action<string, bool, GameObject> onComplete, int utype);
    void UnLoadModel(string modelPath, Action<string, bool, GameObject> onComplete);
    void ReleaseModel(GameObject go);
}
