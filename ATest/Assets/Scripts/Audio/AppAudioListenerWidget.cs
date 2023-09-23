using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AppAudioListenerWidget : MonoBehaviour
{
    private static readonly string ConstAudioListenerName = "AudioListener";
    private static AppAudioListenerWidget _instance;
    private AudioListener _listener = null;
    private Transform _target = null;
    private void Awake()
    {
        _target = this.transform;
        initAudioListener();
    }
    private void initAudioListener()
    {
        _listener = _target.gameObject.GetComponent<AudioListener>();
        if(_listener==null)
        {
            _listener = _target.gameObject.AddComponent<AudioListener>();
        }
    }
    private void attach(GameObject obj)
    {
        if(obj!=null)
        {
            _target.SetParent(obj.transform, false);
        }
    }
    private void unAttach()
    {
        _target.SetParent(null, false);
    }
    private void _StopListener()
    {
        if(_listener!=null)
        {
            _listener.enabled = false;
        }
        if(_target!=null)
        {
            _target.gameObject.SetActive(false);
        }
    }
    private void _StartListener()
    {
        if(_listener!=null)
        {
            _listener.enabled = true;
        }
        if(_target!=null)
        {
            _target.gameObject.SetActive(true);
        }
    }
    private void Dispose()
    {
        if(this.gameObject!=null)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
    private void OnDestroy()
    {
        _listener = null;
        _target = null;
        _instance = null;
    }
    public static AppAudioListenerWidget Instance
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
            var obj = new GameObject(ConstAudioListenerName);
            GameObject.DontDestroyOnLoad(obj);
            _instance = obj.AddComponent<AppAudioListenerWidget>();
        }
    }
    public static void Release()
    {
        if(_instance!=null)
        {
            _instance.Dispose();
        }
        _instance = null;
    }
    public static void Attach(GameObject obj)
    {
        Instance.attach(obj);
    }
    public static void UnAttach()
    {
        Instance.unAttach();
    }
    public static void StopListener()
    {
        Instance._StopListener();
        AppAudioWidget.StopAudioWidget();
    }
    public static void StartListener()
    {
        Instance._StartListener();
        AppAudioWidget.StartAudioWidget();
    }
}
