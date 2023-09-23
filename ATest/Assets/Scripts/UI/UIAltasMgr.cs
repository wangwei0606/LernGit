using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class UIAltasMgr
{
    class AltasConf
    {
        public string id = "";
        public string spriteName = "";
        public string abName = "";
    }
    enum WorkStatus
    {
        Loading,
        Idle
    }
    class SpriteTask
    {
        public string id = "";
        public string spriteName = "";
        public string abName = "";
        public List<Action<string, Sprite>> loadList = new List<Action<string, Sprite>>();
        public void execute(string id,Sprite sp)
        {
            for(int i=0;i<loadList.Count;i++)
            {
                if(loadList[i].Target==null || loadList[i].Target.Equals(null))
                {
                    continue;
                }
                try
                {
                    loadList[i].Invoke(id, sp);
                }
                catch(Exception e)
                {
                    Debug.LogError("spriteTask execute error: " + e.StackTrace);
                }
            }
        }
        public void add(Action<string,Sprite> load)
        {
            loadList.Add(load);
        }
        public void del(Action<string,Sprite> load)
        {
            if(loadList.Contains(load))
            {
                loadList.Remove(load);
            }
        }
        public void reset()
        {
            id = "";
            spriteName = "";
            abName = "";
            loadList.Clear();
        }
    }


    private Queue<SpriteTask> _loaderCache = new Queue<SpriteTask>();
    private Dictionary<string, SpriteTask> _loaderPool = new Dictionary<string, SpriteTask>();
    private Dictionary<string, AltasConf> _config = new Dictionary<string, AltasConf>();
    private Sprite _defaultSprite = null;
    private static UIAltasMgr _instance = null;
    public static void Initilize(string altasConfFile="")
    {
        if(_instance!=null)
        {
            return;
        }
        _instance = new UIAltasMgr(altasConfFile);
    }
    private UIAltasMgr(string altasConfFile="")
    {
        initConf(altasConfFile);
        initDefaultRes();
    }
    private void initDefaultRes()
    {
        _defaultSprite = Resources.Load<Sprite>("sprite/default");
    }
    private void initConf(string altasConfFile)
    {
        if(string.IsNullOrEmpty(altasConfFile))
        {
            return;
        }
        string str = UISupport.LoadFile(altasConfFile);
        AltasConf[] confs = Json.ToObject<AltasConf[]>(str);
        if(confs!=null)
        {
            for(int i=0;i<confs.Length;i++)
            {
                if(!_config.ContainsKey(confs[i].id))
                {
                    _config.Add(confs[i].id, confs[i]);
                }
            }
        }
    }

    private AltasConf getAltasConf(string id)
    {
        AltasConf conf = null;
        if(id!=null && _config.ContainsKey(id))
        {
            conf = _config[id];
        }
        return conf;
    }

    private SpriteTask getLoadTask(string spriteName)
    {
        if(_loaderPool.ContainsKey(spriteName))
        {
            return _loaderPool[spriteName];
        }
        return null;
    }

    private void addTask(SpriteTask task)
    {
        if(_loaderPool.ContainsKey(task.spriteName))
        {
            return;
        }
        _loaderPool.Add(task.spriteName, task);
    }

    private void recycleTask(string spriteName)
    {
        if(_loaderPool.ContainsKey(spriteName))
        {
            var item = _loaderPool[spriteName];
            _loaderPool.Remove(spriteName);
            item.reset();
            _loaderCache.Enqueue(item);
        }
    }

    private SpriteTask getCacheTask()
    {
        if(_loaderCache.Count>0)
        {
            return _loaderCache.Dequeue();
        }
        return new SpriteTask();
    }

    private void removeTaskHandler(string spriteName,Action<string,Sprite> load)
    {
        var item = getLoadTask(spriteName);
        if(item!=null)
        {
            item.del(load);
            if(item.loadList.Count==0)
            {
                recycleTask(spriteName);
            }
        }
    }

    private void removeSpriteTaskHandler(string id,Action<string,Sprite> load)
    {
        if(!string.IsNullOrEmpty(id))
        {
            id = id.ToLower();
            AltasConf conf = getAltasConf(id);
            if(conf!=null)
            {
                removeTaskHandler(conf.spriteName, load);
            }
        }
    }

    private void addSpriteTask(string id,Action<string,Sprite> load)
    {
        if(!string.IsNullOrEmpty(id))
        {
            id = id.ToLower();
        }
        AltasConf conf = getAltasConf(id);
        if(conf!=null)
        {
            var item = getLoadTask(conf.spriteName);
            bool isNeedLoad = item == null;
            if(item==null)
            {
                item = getCacheTask();
                item.id = id;
                item.abName = conf.abName;
                item.spriteName = conf.spriteName;
                addTask(item);
            }
            item.add(load);
            if(isNeedLoad)
            {
                UISupport.LoadSprite(item.spriteName, item.abName, onLoadSpriteComplete);
            }
        }
        else
        {
            load.Invoke(id, _defaultSprite);
        }
    }

    private void removeCacheSprite(string id)
    {
        if(!string.IsNullOrEmpty(id))
        {
            id = id.ToLower();
        }
        AltasConf conf = getAltasConf(id);
        if(conf!=null)
        {
            UISupport.RemoveCacheSprite(conf.spriteName, conf.abName);
        }
    }

    protected void onLoadSpriteComplete(string spriteName,Sprite sp)
    {
        var item = getLoadTask(spriteName);
        if(item==null)
        {
            UISupport.ReleaseSprite(sp);
            return;
        }
        item.execute(spriteName, sp);
        recycleTask(spriteName);
    }

    public void addSpriteTaskWWW(string url,Action<string,Sprite> load,bool isLoadWWW=false)
    {
        var item = getLoadTask(url);
        bool isNeedLoad = item == null;
        if(item==null)
        {
            item = getCacheTask();
            item.id = url;
            item.abName = url;
            item.spriteName = url;
            addTask(item);
        }
        item.add(load);
        if(isNeedLoad)
        {
            UISupport.LoadSpriteByWWW(item.spriteName, onLoadSpriteComplete);
        }
    }

    private void onDispose()
    {
        _config.Clear();
        _loaderCache.Clear();
        _loaderPool.Clear();
        _defaultSprite = null;
    }

    public static void ReleaseSprite(Sprite sp)
    {
        if(sp==null)
        {
            return;
        }
        UISupport.ReleaseSprite(sp.name);
    }

    public static void ReleaseSprite(string spName)
    {
        UISupport.ReleaseSprite(spName);
    }

    public static void RemoveCachedSprite(string id)
    {
        if(_instance!=null)
        {
            _instance.removeCacheSprite(id);
        }
    }

    public static void LoadSpriteWWW(string id,Action<string,Sprite> load,bool isLoadWWW=false)
    {
        if(_instance!=null)
        {
            _instance.addSpriteTaskWWW(id, load, isLoadWWW);
        }
        else
        {
            load(null, null);
        }
    }

    public static void LoadSprite(string url,Action<string,Sprite> load)
    {
        Debug.LogError(_instance==null);
        if(_instance!=null)
        {
            _instance.addSpriteTask(url, load);
        }
        else
        {
            load(null, null);
        }
    }

    public static void UnLoadSprite(string url,Action<string,Sprite> load)
    {
        if(_instance!=null)
        {
            _instance.removeSpriteTaskHandler(url, load);
        }
    }

    public static void UnLoadSpriteWWW(string url,Action<string,Sprite> load)
    {
        if(_instance!=null)
        {
            _instance.removeTaskHandler(url, load);
        }
    }

    public static Sprite GetDefault()
    {
        return _instance == null ? null : _instance._defaultSprite;
    }

    public static void Dispose()
    {
        if(_instance!=null)
        {
            _instance.onDispose();
        }
    }
}