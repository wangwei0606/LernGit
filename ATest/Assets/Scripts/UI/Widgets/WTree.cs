using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WTree : MonoBehaviour
{
    WWTree tree;
    WWTreeNode treeNode;
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void createTree(string jsonData)
    {
 
    }

    private void PraseJsonData(string jsonData)
    {
        JsonData jData=Json.ToObject(jsonData);
        List<string> keyList=jData.GetKeys();
        int len=keyList.Count;
        string keyItem;
        JsonData valueItem;
        for (int i = 0; i < len; i++)
        {
            keyItem = keyList[i];
            valueItem = jData.Get(keyItem);
            string jsonType= valueItem.Type;
            Debug.LogError(jsonType);
        }
    }

    protected virtual void init()
    {
        tree = this.gameObject.GetComponent<WWTree>();
    }
    public static WTree Create(GameObject go)
    {
        WTree view = go.GetComponent<WTree>();
        if(view==null)
        {
            view = go.AddComponent<WTree>();
        }
        view.init();
        return view;
    }

    public void Dispose()
    {
        tree=null;
    }
}
