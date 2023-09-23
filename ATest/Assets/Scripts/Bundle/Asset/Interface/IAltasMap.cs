using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IAltasMap
{
    void Initlize();
    Sprite GetSprite(int index);
    Sprite GetSprite(string id);
    string GetAltasName();
}
