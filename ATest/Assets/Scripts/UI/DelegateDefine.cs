using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public delegate void onLoadSprite(string spriteName, string altasSrouce, Action<Sprite> onComplete);
public delegate void SpecialLoad(int imgId, Action<Sprite> onComplete);

public delegate void SocketConnected(string ip, int port, string code);
public delegate void GetImageHandle(string imageType, string imageStr, Sprite sprite);
public delegate void DropDownValueChangeHandle(int value);
public delegate void RangeSliderValueChangeHandle(float min,float max);

public delegate void OnTouchDebugHandle(string str);
public delegate void OnTouchHandle(float x, float y);
public delegate void OnMulTouchHandle(int num, List<MulTouchData> datas);
public delegate void OnMulTouchEndHandle();
public delegate void OnScaleHandle(float delta);
public delegate void OnKeyHandle(int key);
public delegate void OnKeyHoldHandle(int keys);


public delegate void OnItemPositionChange(int wrapIndex, int realIndex, GameObject obj);
public delegate void OnItemInit(GameObject obj);
public delegate void OnLoopRectInstanceItem(GameObject itemobj, int hash);
public delegate void OnLoopRectRender(int hash, int index);