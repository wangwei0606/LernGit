using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class UICSToLua :ILink,ILocalization,IUIResLoader
{
    class SpriteHandler
    {
        public string spriteName;
        public Action<string, Sprite> onLoad;
    }

    class SpriteLoader
    {
        public string altasSrouce;
        public List<SpriteHandler> loadList = new List<SpriteHandler>();
    }

    class SpriteAltas
    {
        public string spriteName;
        public string altasSrouce;
        public int count = 0;
    }

    enum WebSpriteStatus
    {
        Load,
        OK,
        Fail
    }

    class WebSpriteLoader
    {
        public WebSpriteStatus state;
        public string url;
        public string customID;
        public Action<string, Sprite> onLoad;
    }

    class WebSprite
    {
        private string filePath;
        public string spriteName;
        private Sprite sp;
        private int refCount = 0;
        public WebSprite(string name,string path)
        {
            spriteName = name;
            filePath = path;
            init();
        }
        private void init()
        {
            if(FileUtils.IsFileExists(filePath))
            {
                var bytes = FileUtils.LoadByteFile(filePath);
                var texture = new Texture2D(2, 2);
                texture.LoadImage(bytes);
                sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                sp.name = spriteName;
            }
        }

        public bool IsInit()
        {
            return sp != null;
        }

        public Sprite getSprite()
        {
            refCount++;
            return sp;
        }

        public void Release()
        {
            refCount--;
        }

        public bool Destory()
        {
            bool isDestory = refCount <= 0;
            if(isDestory)
            {
                UnityEngine.GameObject.Destroy(sp);
                sp = null;
            }
            return isDestory;
        }
    }


    private static UICSToLua _instance;
    public static void Initilize()
    {
        if(_instance==null)
        {
            _instance = new UICSToLua();
        }
    }
    private UICSToLua()
    {
        init();
    }
    private int _checkTimer = -1;
    private int _checkTime = 1;
    private void init()
    {
        HttpLoader.Initialize(AppPath.RootPath, AppConfig.Instance.update_url);
        UISupport.Initilize(this, this, AppSetting.AltasConfFile, this);
        _checkTimer = TimerMgr.SetEveryMinute(checkWebSprite, _checkTime);
    }

    private List<string> _mRemoveList = new List<string>();
    private Dictionary<string, WebSprite> _webSpritesPool = new Dictionary<string, WebSprite>();
    private Dictionary<string, WebSpriteLoader> _webLoaderPool = new Dictionary<string, WebSpriteLoader>();
    private Dictionary<string, SpriteLoader> _loadPool = new Dictionary<string, SpriteLoader>();
    private Dictionary<string, SpriteAltas> _altasPool = new Dictionary<string, SpriteAltas>();


    private void checkWebSprite(int id,int time)
    {
        if(_webSpritesPool.Count==0)
        {
            return;
        }
        _mRemoveList.Clear();
        var target = _webSpritesPool.GetEnumerator();
        while(target.MoveNext())
        {
            if(target.Current.Value.Destory())
            {
                _mRemoveList.Add(target.Current.Key);
            }
        }
        target.Dispose();
        for(int i=0;i<_mRemoveList.Count;i++)
        {
            _webSpritesPool.Remove(_mRemoveList[i]);
        }
        if(_mRemoveList.Count>0)
        {
            Resources.UnloadUnusedAssets();
            _mRemoveList.Clear();
        }
    }

    public void onLink(string linkArgs)
    {

    }

    public string Get(string key)
    {
        return key;
    }

    public void TextLocalize(string key,Transform transform)
    {

    }

    public void LoadSprite(string spriteName,string altasSrouce,Action<string,Sprite> onComplete)
    {
        altasSrouce = altasSrouce.ToLower();
        var asset = AssetLoader.GetAsset(altasSrouce);
        if(asset!=null)
        {
            onComplete(spriteName, asset.LoadSprite(spriteName));
            if(!_altasPool.ContainsKey(spriteName))
            {
                _altasPool.Add(spriteName, new SpriteAltas { spriteName = spriteName, altasSrouce = altasSrouce, count = 1 });
            }
            else
            {
                _altasPool[spriteName].count++;
            }
            return;
        }
        if(_loadPool.ContainsKey(altasSrouce))
        {
            _loadPool[altasSrouce].loadList.Add(new SpriteHandler() { spriteName = spriteName, onLoad = onComplete });
            return;
        }
        else
        {
            var loader = new SpriteLoader();
            loader.altasSrouce = altasSrouce;
            loader.loadList.Add(new SpriteHandler() { spriteName = spriteName, onLoad = onComplete });
            _loadPool.Add(altasSrouce, loader);
        }
        AssetLoader.WWW(altasSrouce, onLoadSpriteComplete, onLoadSpriteFail, null, true, false, true);
    }

    public void RemoveCacheSprite(string spriteName,string altasSrouce)
    {
        altasSrouce = altasSrouce.ToLower();
        var asset = AssetLoader.GetAsset(altasSrouce);
        if(asset!=null)
        {
            asset.RemoveCacheSprite(spriteName);
        }
    }

    private void onLoadSpriteComplete(string altasSrouce,Asset asset)
    {
        if(_loadPool.ContainsKey(altasSrouce))
        {
            var loader = _loadPool[altasSrouce];
            for(int i=0;i<loader.loadList.Count;i++)
            {
                if(!_altasPool.ContainsKey(loader.loadList[i].spriteName))
                {
                    _altasPool.Add(loader.loadList[i].spriteName, new SpriteAltas() { spriteName = loader.loadList[i].spriteName, altasSrouce = loader.altasSrouce, count = 1 });

                }
                else
                {
                    _altasPool[loader.loadList[i].spriteName].count++;
                }
                Sprite sp = null;
                if(asset!=null)
                {
                    sp = asset.LoadSprite(loader.loadList[i].spriteName);
                }
                loader.loadList[i].onLoad.Invoke(loader.loadList[i].spriteName, sp);
            }
            _loadPool.Remove(altasSrouce);
        }
    }

    private void onLoadSpriteFail(string altasSrouce)
    {
        if(_loadPool.ContainsKey(altasSrouce))
        {
            var loader = _loadPool[altasSrouce];
            for(int i=0;i<loader.loadList.Count;i++)
            {
                loader.loadList[i].onLoad.Invoke(loader.loadList[i].spriteName, null);
            }
            _loadPool.Remove(altasSrouce);
        }
    }

    private void OnDownLoadSprite(IAsyncTask task,HttpLoadCode code)
    {
        var customId = task.CustomID;
        if(!_webLoaderPool.ContainsKey(customId))
        {
            return;
        }
        var loader = _webLoaderPool[customId];
        if(code==HttpLoadCode.eOK)
        {
            loader.state = WebSpriteStatus.OK;
            string filePath = Path.Combine(AppPath.RootPath, customId);
            string spName = FileUtils.RemoveExName(customId);
            var webSp = new WebSprite(spName, filePath);
            if(webSp.IsInit())
            {
                _webSpritesPool.Add(spName, webSp);
                loader.onLoad.Invoke(loader.url, webSp.getSprite());
            }
            else
            {
                Debug.LogError("====loadspritebywww==== not save: " + filePath);
            }
        }
        else
        {
            loader.state = WebSpriteStatus.Fail;
        }
    }

    public void LoadSpriteByWWW(string url,Action<string,Sprite> onComplete)
    {
        string fileName = FileUtils.GetImageName(url).ToLower();
        string spName = FileUtils.RemoveExName(fileName);
        if(_webSpritesPool.ContainsKey(spName))
        {
            onComplete.Invoke(url, _webSpritesPool[spName].getSprite());
            return;
        }
        string resFile = Path.Combine(AppConfig.Instance.absIconPath, fileName);
        resFile = resFile.ToLower();
        string filePath = Path.Combine(AppPath.RootPath, resFile);
        var webSp = new WebSprite(spName, filePath);
        if(webSp.IsInit() )
        {
            _webSpritesPool.Add(spName, webSp);
            onComplete.Invoke(url, webSp.getSprite());
            return;
        }
        if(_webLoaderPool.ContainsKey(resFile))
        {
            var loader = _webLoaderPool[resFile];
            if(loader.state==WebSpriteStatus.Fail)
            {
                onComplete(url, null);
                return;
            }
            if(loader.state==WebSpriteStatus.Load)
            {
                loader.onLoad += onComplete;
                return;
            }
            if(loader.state==WebSpriteStatus.OK)
            {
                return;
            }
        }
        _webLoaderPool.Add(resFile, new WebSpriteLoader { url = url, state = WebSpriteStatus.Load, customID = resFile, onLoad = onComplete });
        HttpLoader.DownLoadSmall(url, resFile, resFile, OnDownLoadSprite);
    }

    public void ReleaseSprite(Sprite sp)
    {
        if(sp==null)
        {
            return;
        }
        string spriteName = sp.name;
        ReleaseSprite(spriteName);
    }
    public void ReleaseSprite(string spName)
    {
        if(_altasPool.ContainsKey(spName))
        {
            AssetLoader.ReleaseAsset(_altasPool[spName].altasSrouce);
            _altasPool[spName].count--;
            if(_altasPool[spName].count<=0)
            {
                _altasPool.Remove(spName);
            }
        }
        if(_webSpritesPool.ContainsKey(spName))
        {
            _webSpritesPool[spName].Release();
        }
    }

    public void LoadModel(string modelPath,Action<string,bool,GameObject> onComplete,int utype)
    {
        PoolUseType type = (PoolUseType)utype;
        PoolMgr.Create(modelPath, onComplete, false, PoolType.UseTime, type);
    }

    public void UnLoadModel(string modelPath,Action<string,bool,GameObject> onComplete)
    {
        PoolMgr.UnCreate(modelPath, onComplete);
    }

    public void ReleaseModel(GameObject go)
    {
        PoolMgr.Destory(go);
    }

    public static void LoadUI(string resurl,Action<string,bool,GameObject> onLoadObject,bool isBuildIn,int ptype,int putype,int userInterval,int initCount)
    {
        PoolType pt = (PoolType)ptype;
        PoolUseType put = (PoolUseType)putype;
        PoolMgr.Create(resurl, onLoadObject, isBuildIn, pt, put, userInterval, initCount);
    }

    public static void ReleaseUI(GameObject obj)
    {
        PoolMgr.Destory(obj);
    }

    public static void Release()
    {
        if(_instance==null)
        {
            return;
        }
        if(_instance._checkTimer!=-1)
        {
            TimerMgr.Remove(_instance._checkTimer);
            _instance._checkTimer = -1;
        }
    }

    public static float GetTransAlpha(Transform transform)
    {
        if(transform!=null)
        {
            Graphic[] graphics = transform.GetComponentsInChildren<Graphic>(true);
            if(graphics==null || graphics.Length==0)
            {
                return 0;
            }
            return graphics[0].color.a;
        }
        return 0;
    }

    public static void setTramsAlpha(Transform transform,float alpha)
    {
        if(transform==null)
        {
            return;
        }
        Graphic[] graphics = transform.GetComponentsInChildren<Graphic>(true);
        if(graphics==null)
        {
            return;
        }
        int graphicCount = graphics.Length;
        for(int i=0;i<graphicCount;i++)
        {
            Graphic gc = graphics[i];
            Color color = gc.color;
            gc.color = new Color(color.r, color.g, color.b, alpha);
        }
    }

    public static GameObject AddChild(GameObject parent,GameObject prefab)
    {
        GameObject go = GameObject.Instantiate(prefab) as GameObject;
        if(!go.activeSelf)
        {
            go.SetActive(true);
        }
        if(go!=null && parent!=null)
        {
            Transform t = go.transform;
            t.SetParent(parent.transform, false);
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = parent.layer;
        }
        return go;
    }

    public static GameObject SetChild(GameObject parent,GameObject child)
    {
        if(child!=null && parent !=null)
        {
            Transform t = child.transform;
            t.SetParent(parent.transform, false);
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            child.layer = parent.layer;
        }
        return child;
    }

    public static void SetMainCameraEnable(bool b)
    {

    }

    public static void SetGridHeight(GridLayoutGroup grid,int addH=0)
    {
        if(grid!=null)
        {
            int num = grid.constraintCount;
            float childCount = grid.transform.childCount;
            float height = ((childCount + num - 1) / num) * grid.cellSize.y;
            height += (((childCount + num - 1) / num) - 1) * grid.spacing.y;
            grid.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height - addH);
        }
    }

    private static Camera uiCamera;
    public static Camera getUiCamera()
    {
        if(uiCamera==null)
        {
            uiCamera = GameObject.FindGameObjectWithTag("MainUI").GetComponent<Camera>();
        }
        return uiCamera;
    }

    public static Vector3 getScenePrefix()
    {
        RectTransform mainUI = (RectTransform)getUiCamera().transform;
        Vector2 wh = mainUI.sizeDelta;
        Vector3 scenePrefix = new Vector3(wh.x / Screen.width, wh.y / Screen.height, 1);
        return scenePrefix;
    }

    public static Vector3 getFixPosition(Vector3 pos,Vector3 fix)
    {
        return V3Multiplication(pos, fix);
    }

    public static Vector3 getFixPositionAuto(Vector3 pos)
    {
        return V3Multiplication(pos, GameUtil.getScreenPrefix());
    }

    public static Vector3 V3Multiplication(Vector3 v1,Vector3 v2)
    {
        return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    }

    public static void CleanChildGo(GameObject parent)
    {
        if(parent!=null)
        {
            int index = 0;
            Transform gridTrans = parent.transform;
            int childCount = gridTrans.childCount;
            GameObject go = null;
            while(index<childCount)
            {
                go = gridTrans.GetChild(index).gameObject;
                go.SetActive(false);
                UnityEngine.Object.Destroy(go);
                index++;
            }
        }
    }

    public string LoadFile(string file)
    {
        switch(Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                file = file.ToLower();
                break;
        }
        string fullPath = Path.Combine(AppPath.ScriptPath, file);
        return FileProxy.LoadFile(fullPath, file);
    }

    public static Camera getUICamera()
    {
        GameObject uicam = GameObject.FindGameObjectWithTag(GameTagLayers.MainUITag);
        if(uicam!=null)
        {
            return uicam.GetComponent<Camera>();
        }
        return null;
    }
}