using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UIRenderTextureCameraPool
{
    private static int power = 100;
    private static int dis = 2000;
    private static int id = 1;
    private static List<RenderTextureCamera> _pools = new List<RenderTextureCamera>();
    private static List<RenderTextureCamera> removeLst = new List<RenderTextureCamera>();
    private static Vector3 getPos()
    {
        Vector3 pos = new Vector3(dis, dis, dis);
        dis += power;
        return new Vector3(dis, dis, dis);
    }
    public static void checkUnUse()
    {
        if(_pools.Count<=1)
        {
            return;
        }
        for(int i=0;i<_pools.Count;i++)
        {
            if(_pools[i].GUID==-1)
            {
                removeLst.Add(_pools[i]);
            }
        }
        for(int i=0;i<removeLst.Count;i++)
        {
            removeCamera(removeLst[i]);
        }
        removeLst.Clear();
    }
    private static void removeCamera(RenderTextureCamera item)
    {
        if(_pools.Count==1)
        {
            return;
        }
        _pools.Remove(item);
        GameObject.Destroy(item.gameObject);
    }
    private static RenderTextureCamera getNewCamera()
    {
        var obj = Resources.Load("prefab/uiModelCamera") as GameObject;
        if(obj==null)
        {
            obj = createRenderObj();
        }
        else
        {
            obj = GameObject.Instantiate(obj) as GameObject;
            obj.name = "UIModelCamera";
        }
        obj.transform.position = getPos();
        GameObject.DontDestroyOnLoad(obj);
        RenderTextureCamera rCamera = obj.AddComponent<RenderTextureCamera>();
        rCamera.init();
        return rCamera;
    }
    private static GameObject createRenderObj()
    {
        var obj = new GameObject("UIModelCamera");
        Camera cam = obj.AddComponent<Camera>();
        cam.depth = -10;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0, 0, 0, 0);
        cam.farClipPlane = 100;
        var root = new GameObject("Root");
        root.transform.SetParent(obj.transform, false);
        root.transform.localPosition = new Vector3(0, 0, 10);
        return obj;
    }
    public static void release(RenderTextureCamera camera)
    {
        if(_pools.Contains(camera))
        {
            camera.reset();
            camera.GUID = -1;
        }
    }

    public static void DestoryCamera(RenderTextureCamera camera)
    {
        _pools.Remove(camera);
    }
    public static RenderTextureCamera getUnUse()
    {
        var target = _pools.GetEnumerator();
        RenderTextureCamera camera = null;
        while(target.MoveNext())
        {
            if(target.Current.GUID==-1)
            {
                camera = target.Current;
                break;
            }
        }
        target.Dispose();
        return camera;
    } 
    public static RenderTextureCamera getModelCamera()
    {
        var camera = getUnUse();
        if(camera==null)
        {
            camera = getNewCamera();
            _pools.Add(camera);
        }
        else
        {
            camera.reInit();
        }
        camera.GUID = id++;
        return camera;
    }
}