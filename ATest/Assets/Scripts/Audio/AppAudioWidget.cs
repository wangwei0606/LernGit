using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AppAudioWidget :MonoBehaviour
{
    private static readonly string ConstAudioName = "AppAudio";
    private static readonly string ConstAudioSourceName = "AudioSource";
    private AudioSource _audioSource = null;
    private AudioSource _audioLoopSource = null;
    private Transform _target = null;
    private int _audioUid = 0;
    private int _loopAudioUid = 0;
    private Dictionary<int, string> _uidAudioMap = new Dictionary<int, string>();
    private Dictionary<int, AudioClip> _pools = new Dictionary<int, AudioClip>();
    private Dictionary<int, AudioTimeData> _timePools = new Dictionary<int, AudioTimeData>();
    private static AppAudioWidget _instance;
    public static void Initilize()
    {
        if(_instance==null)
        {
            var obj = new GameObject(ConstAudioName);
            GameObject.DontDestroyOnLoad(obj);
            _instance = obj.AddComponent<AppAudioWidget>();
        }

    }
    private void _StopAudioWidget()
    {
        if(_target!=null)
        {
            _target.gameObject.SetActive(false);
        }
    }
    private void _StartAudioWidget()
    {
        if(_target!=null)
        {
            _target.gameObject.SetActive(true);
        }
    }
    private void Awake()
    {
        _target = this.transform;
        initAudioListener();
        initAudioSrouce();
    }
    private int createUid()
    {
        _audioUid++;
        return _audioUid;
    }
    private void initAudioListener()
    {
        AppAudioListenerWidget.Attach(_target.gameObject);
    }
    private void initAudioSrouce()
    {
        var obj = new GameObject(ConstAudioSourceName);
        _audioSource = obj.AddComponent<AudioSource>();
        _audioLoopSource = obj.AddComponent<AudioSource>();
        obj.transform.SetParent(_target, false);
    }

    public int playAudio(string audioName,float volume,bool isLoop,Action onPlayComplete)
    {
        int uid = createUid();
        Action<AudioClip> onComplete = (clip) => 
        {
            if(clip==null)
            {
                if(onPlayComplete!=null)
                {
                    onPlayComplete();
                }
                return;
            }
            onPlayAudio(clip, volume, isLoop, uid, onPlayComplete);
        };
        if(!_uidAudioMap.ContainsKey(uid))
        {
            _uidAudioMap.Add(uid, audioName);
        }
        AudioMgr.Load(audioName, onComplete);
        return uid;
    }
    private void onPlayAudio(AudioClip clip,float volume,bool isLoop,int uid,Action onComplete)
    {
        if(!_uidAudioMap.ContainsKey(uid))
        {
            AudioMgr.ReleaseAudio(clip);
            return;
        }
        if(!isLoop)
        {
            _audioSource.PlayOneShot(clip, volume);
            _audioSource.loop = false;
            float time = clip.length;
            int timerId = TimerMgr.SetCountDown(time, onPlayComplete);
            AudioTimeData data = new AudioTimeData(uid, timerId, onComplete);
            _timePools.Add(uid, data);
            _pools.Add(uid, clip);
            return;
        }
        if(uid<_loopAudioUid)
        {
            AudioMgr.ReleaseAudio(clip);
            return;
        }
        if(_loopAudioUid!=0)
        {
            stopAudioById(_loopAudioUid);
        }
        _audioLoopSource.clip = clip;
        _audioLoopSource.loop = isLoop;
        _audioLoopSource.Play();
        _loopAudioUid = uid;
        _pools.Add(uid, clip);
    }
    private void setBGMVolume(float volume)
    {
        _audioLoopSource.volume = volume;
    }
    private void onPlayComplete(int timerId)
    {
        int removeUid = 0;
        foreach(KeyValuePair<int,AudioTimeData> kv in _timePools)
        {
            if(kv.Value.timerId==timerId)
            {
                removeUid = kv.Value.uid;
                break;
            }
        }
        stopAudioById(removeUid);
    }
    private void stopAudioById(int id,bool isForceStop=false)
    {
        bool isLoopAudio = false;
        if(_uidAudioMap.ContainsKey(id))
        {
            _uidAudioMap.Remove(id);
        }
        if(_pools.ContainsKey(id))
        {
            AudioMgr.ReleaseAudio(_pools[id]);
            _pools.Remove(id);
            if(_loopAudioUid==id)
            {
                isLoopAudio = true;
                _loopAudioUid = 0;
                _audioLoopSource.Stop();
            }
            if(_timePools.ContainsKey(id))
            {
                AudioTimeData data = _timePools[id];
                if(data!=null && data.onPlayComplete!=null)
                {
                    data.onPlayComplete();
                    data.onPlayComplete = null;
                }
                _timePools.Remove(id);
            }
        }
        if(isForceStop && !isLoopAudio)
        {
            _audioSource.Stop();
        }
    }
    private void stopAll()
    {
        if(_pools.Count==0)
        {
            return;
        }
        var target = _pools.GetEnumerator();
        while(target.MoveNext())
        {
            AudioMgr.ReleaseAudio(target.Current.Value);
        }
        _pools.Clear();
        _loopAudioUid = 0;
    }

    private void Dispose()
    {
        stopAll();
        _audioSource = null;
        _audioLoopSource = null;
        GameObject.Destroy(_target.gameObject);
        _target = null;
    }
    public static void Release()
    {
        if(_instance!=null)
        {
            _instance.Dispose();
        }
        _instance = null;
    }
    public static int Play(string audioName,float volume,bool isLoop,Action onComplete)
    {
        if(_instance!=null)
        {
            return _instance.playAudio(audioName, volume, isLoop, onComplete);
        }
        return 0;
    }
    public static void SetBGMVolume(float volume)
    {
        if(_instance!=null)
        {
            _instance.setBGMVolume(volume);
        }
    }
    public static void StopById(int uid)
    {
        if(_instance!=null)
        {
            _instance.stopAudioById(uid, true);
        }
    }
    public static void StopAll()
    {
        if(_instance!=null)
        {
            _instance.stopAll();
        }
    }
    public static void StopAudioWidget()
    {
        if(_instance!=null)
        {
            _instance._StopAudioWidget();
        }
    }
    public static void StartAudioWidget()
    {
        if(_instance!=null)
        {
            _instance._StartAudioWidget();
        }
    }
}
