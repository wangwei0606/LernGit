using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface ILocalization
{
    string Get(string key);
    void TextLocalize(string key, Transform transform);
}