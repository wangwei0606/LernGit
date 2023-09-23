using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class RenderTextureCamera : MonoBehaviour
{
    public int GUID = -1;
    private Camera rcamera;
    RenderTexture renderTexture;
    Transform root;
    private float orthographicSize = -1;
    private bool isHighQuality = false;
    private void Awake()
    {
        
    }
    public void init()
    {
        isHighQuality = false;
        renderTexture = new RenderTexture(512, 512, 16, RenderTextureFormat.ARGB32);
        rcamera = this.gameObject.GetComponent<Camera>();
        rcamera.farClipPlane = 100;
        rcamera.targetTexture = renderTexture;
        rcamera.useOcclusionCulling = false;
        root = transform.Find("Root");
    }
    public void reInit()
    {
        setCamerEnable(true);
    }
    public void setCamerEnable(bool enable)
    {
        if(rcamera!=null)
        {
            rcamera.enabled = enable;
        }
    }
    public void setQuality(bool isHigh)
    {
        if(isHighQuality==isHigh)
        {
            return;
        }
        isHighQuality = isHigh;
        renderTexture.Release();
        if(isHighQuality)
        {
            renderTexture.width = 1024;
            renderTexture.height = 1024;
        }
        else
        {
            renderTexture.width = 512;
            renderTexture.height = 512;
        }
    }
    public void setOrthographic(float size)
    {
        if(size>0)
        {
            orthographicSize = size;
            rcamera.orthographic = true;
            rcamera.orthographicSize = orthographicSize;
        }
    }
    public Texture RTexture
    {
        get
        {
            return renderTexture;
        }
    }
    public Transform Root
    {
        get
        {
            return root;
        }
    }
    public void reset()
    {
        if(orthographicSize>0)
        {
            rcamera.orthographic = false;
        }
        orthographicSize = -1;
        setCamerEnable(false);
        setQuality(false);
    }
    private void OnDestroy()
    {
        if(renderTexture!=null)
        {
            renderTexture.Release();
        }
        rcamera.targetTexture = renderTexture = null;
        UIRenderTextureCameraPool.DestoryCamera(this);
    }
}