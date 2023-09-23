using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LoadingSimulation
{
    public delegate void Complete();
    public delegate void Progress(float prog);
    private int _count;
    private float _step;
    private MonoBehaviour _behaviour;
    private float _progressValue;
    private Complete _complete;
    private Progress _progress;
    public void Start(MonoBehaviour behaviour,float timer,Complete complete,Progress progress)
    {
        _behaviour = behaviour;
        _progress = progress;
        _complete = complete;
        _count = Mathf.CeilToInt(timer / Time.deltaTime) + 1;
        _step = 1.0f / _count;
        _behaviour.StartCoroutine(doProgress());
    }
    IEnumerator doProgress()
    {
        yield return 0;
        while(_count>0)
        {
            _count--;
            _progressValue += _step;
            if(_progressValue>1)
            {
                _progressValue = 1;
            }
            if(_progress!=null)
            {
                _progress(_progressValue);
            }
            yield return new WaitForEndOfFrame();
        }
        if(_complete!=null)
        {
            _complete();
        }
    }
    public void DisPose()
    {
        _behaviour.StopCoroutine("doProgress");
        _complete = null;
        _progress = null;
        _behaviour = null;
    }
}
