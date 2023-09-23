using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideManagers : MonoBehaviour
{
    private void Start()
    {
        //Next();
    }
    public List<GuideUIList> guideList = new List<GuideUIList>();
    private int currentIndex = 0;
    private bool isFinish = false;
    private GameObject maskPrefabs;
    public void Next()
    {
        if(isFinish || currentIndex>guideList.Count)
        {
            return;
        }
        if(currentIndex!=0 && guideList[currentIndex-1].go.GetComponent<EventListener>()!=null)
        {
            //EventListener.Get(guideList[currentIndex - 1].go).onClick -= null;
        }
        if(maskPrefabs==null)
        {
            maskPrefabs = Instantiate(Resources.Load<GameObject>("RectGuidance_Panel"), this.transform);
        }
        maskPrefabs.GetComponent<RectGuidance>().Init(guideList[currentIndex].go.GetComponent<Image>());
        currentIndex++;
        if(currentIndex<guideList.Count)
        {
            EventListener.Get(guideList[currentIndex - 1].go).onClick += (go) =>
                {
                    Next();
                };
        }
        else if(currentIndex==guideList.Count)
        {
            EventListener.Get(guideList[currentIndex-1].go).onClick+=(go)=>
            {
                maskPrefabs.gameObject.SetActive(false);
            };
            isFinish = true;
        }
    }

    public static GuideManagers Create(GameObject go)
    {
        GuideManagers view = go.GetComponent<GuideManagers>();
        if (view == null)
        {
            view = go.AddComponent<GuideManagers>();
        }            
        return view;
    }
}

[Serializable]
public class GuideUIList
{
    public GameObject go;
    public GuideUIList(GameObject go)
    {
        this.go = go;
    }
}
