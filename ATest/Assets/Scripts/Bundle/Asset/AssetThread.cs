using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetThread : MonoBehaviour
{
    private const string Tag = "_assetThread";
    private static AssetThread _instance;
    public static AssetThread Instance
    {
        get
        {
            if(_instance==null)
            {
                Initilize();
            }
            return _instance;
        }
    }

    public static void Initilize()
    {
        if(_instance!=null)
        {
            return;
        }
        GameObject obj = GameObject.Find(Tag);
        if(obj==null)
        {
            obj = new GameObject();
            obj.name = Tag;
        }
        _instance = obj.GetComponent<AssetThread>();
        if(_instance==null)
        {
            _instance = obj.AddComponent<AssetThread>();
        }
        GameObject.DontDestroyOnLoad(obj);
    }

    internal static void LoadFileFromAnsyc(IEnumerator loadHandle)
    {
        Instance.StartCoroutine(loadHandle);
    }
    public static void Release()
    {
        if(_instance==null)
        {
            return;
        }
        GameObject.Destroy(_instance.gameObject);
        _instance = null;
    }
    public static void DoTaskAnsyc(IEnumerator taskHandle)
    {
        Instance.StartCoroutine(taskHandle);
    }
}
