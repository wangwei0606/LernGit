using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IIgnoreRaycast
{
    void Init(GameObject obj);
    void Release();
}