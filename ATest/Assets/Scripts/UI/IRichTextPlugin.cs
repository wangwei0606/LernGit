using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public interface IRichTextPlugin
{
    void Init(GameObject obj);
    void RegisterLinkHandle(Action<string> linkHandle);
    string Text { get; set; }
    float GetPreferredheight();
    float GetPreferredWidth();
    float GetWidth();
    float GetHeight();
    void SetSize(float width, float height);
    void SetAlignment(TextAnchor anchor);
    void SetHorizontalWrapMode(HorizontalWrapMode overflow);
    void SetVerticalWrapMode(VerticalWrapMode overflow);
    void SetIsFaceMat(bool b);
    void SetIsStaticeMat(bool b);
    void Release();
}
