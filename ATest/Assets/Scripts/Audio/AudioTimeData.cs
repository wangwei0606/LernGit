using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class AudioTimeData
{
    public int timerId;
    public int uid;
    public Action onPlayComplete;
    public AudioTimeData(int _uid,int _timerId,Action _onPlayComplete)
    {
        uid = _uid;
        timerId = _timerId;
        onPlayComplete = _onPlayComplete;
    }
}