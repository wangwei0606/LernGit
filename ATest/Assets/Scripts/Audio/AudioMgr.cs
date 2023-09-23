using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AudioMgr
{
    class AudioLoader
    {
        public string audioName;
        public string audioSrouce;
        public Action<AudioClip> onLoad;
    }
    class AudioAltas
    {
        public string audioName;
        public string audioSrouce;
        private Asset asset;
        private int refCount = 0;
        public AudioAltas(string name,string url,Asset asset)
        {
            audioName = name;
            audioSrouce = url;
            this.asset = asset;
        }
        public AudioClip getClip()
        {
            var clip = (AudioClip)asset.Content;
            refCount++;
            return clip;
        }
        public void releaseAudio()
        {
            refCount--;
            AssetLoader.ReleaseAsset(audioSrouce);
        }
        public void clear()
        {
            refCount = 0;
            asset = null;
        }
        public bool Destory()
        {
            bool isDestory = refCount <= 0;
            if(isDestory)
            {
                asset = null;
            }
            return isDestory;
        }
    }
    private Dictionary<string, AudioLoader> _loadPool = new Dictionary<string, AudioLoader>();
    private Dictionary<string, AudioAltas> _audioSroucePool = new Dictionary<string, AudioAltas>();
    private List<string> _mRemoveLst = new List<string>();
    private static AudioMgr _instance = null;
    public static AudioMgr Instance
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
        if(_instance==null)
        {
            _instance = new AudioMgr();
        }
    }
    private int _checkTimer = -1;
    private int _checkTime = 1;
    private AudioMgr()
    {
        _checkTimer = TimerMgr.SetEveryMinute(checkAudioSrouce, _checkTimer);
    }
    private void checkAudioSrouce(int id,int time)
    {
        if(_audioSroucePool.Count==0)
        {
            return;
        }
        _mRemoveLst.Clear();
        var target = _audioSroucePool.GetEnumerator();
        while(target.MoveNext())
        {
            if(target.Current.Value.Destory())
            {
                _mRemoveLst.Add(target.Current.Key);
            }
        }
        target.Dispose();
        for(int i=0;i<_mRemoveLst.Count;i++)
        {
            _audioSroucePool.Remove(_mRemoveLst[i]);
        }
        if(_mRemoveLst.Count>0)
        {
            Resources.UnloadUnusedAssets();
            _mRemoveLst.Clear();
        }
    }
    private string GetAudioUrl(string audioName)
    {
        audioName = audioName.ToLower();
        string url = string.Empty;
        var conf = ConfMgr.Get<ModelConfig>(audioName);
        if(conf==null)
        {

        }
        else
        {
            url = conf.ResPath;
        }
        return url;
    }
    private void LoadAudio(string audioName,Action<AudioClip> onComplete)
    {
        audioName = audioName.ToLower();
        if(_audioSroucePool.ContainsKey(audioName))
        {
            if(_audioSroucePool[audioName].getClip()==null)
            {
                _audioSroucePool[audioName].clear();
                _audioSroucePool.Remove(audioName);
            }
            else
            {
                onComplete.Invoke(_audioSroucePool[audioName].getClip());
                return;
            }
        }
        string audioSrouce = GetAudioUrl(audioName);
        if(string.IsNullOrEmpty(audioSrouce))
        {
            onComplete(null);
            return;
        }
        if(_loadPool.ContainsKey(audioSrouce))
        {
            _loadPool[audioSrouce].onLoad += onComplete;
            return;
        }
        else
        {
            _loadPool.Add(audioSrouce, new AudioLoader { audioName = audioName, audioSrouce = audioSrouce, onLoad = onComplete });
        }
        AssetLoader.WWW(audioSrouce, onLoadAudioComplete, onLoadAudioFail, null);
    }
    private void onLoadAudioComplete(string audioSrouce,Asset asset)
    {
        if(asset==null)
        {
            onLoadAudioFail(audioSrouce);
            return;
        }
        if(_loadPool.ContainsKey(audioSrouce))
        {
            var loader = _loadPool[audioSrouce];
            var audioAltas = new AudioAltas(loader.audioName, loader.audioSrouce, asset);
            _audioSroucePool.Add(loader.audioName, audioAltas);
            _loadPool.Remove(audioSrouce);
            try
            {
                loader.onLoad.Invoke(audioAltas.getClip());
            }
            catch(Exception e)
            {
                Debug.LogError(e.StackTrace);
            }
        }
    }
    private void onLoadAudioFail(string audioSrouce)
    {
        if(_loadPool.ContainsKey(audioSrouce))
        {
            var loader = _loadPool[audioSrouce];
            try
            {
                loader.onLoad.Invoke(null);
            }
            catch(Exception e)
            {
                Debug.LogError(e.StackTrace);
            }
            _loadPool.Remove(audioSrouce);
        }

    }
    private void releaseAudio(AudioClip clip)
    {
        if(clip==null)
        {
            return;
        }
        string name = clip.name.ToLower();
        if(_audioSroucePool.ContainsKey(name))
        {
            _audioSroucePool[name].releaseAudio();
        }
        clip = null;
    }
    public static void Load(string audioName)
    {
        Instance.LoadAudio(audioName, (audioClip)=> { });
    }
    public static void Load(string audioName,Action<AudioClip> onComplete)
    {
        Instance.LoadAudio(audioName, onComplete);
    }
    public static void ReleaseAudio(AudioClip clip)
    {
        Instance.releaseAudio(clip);
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
}
