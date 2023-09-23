using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public interface ISpriteAnimationPlugin
{
    void Init(GameObject obj);
    void SetSpeed(int speed);
    void Stop();
    void PlayOnce();
    void Release();
}