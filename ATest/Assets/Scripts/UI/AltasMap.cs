using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;    

public class AltasMap : MonoBehaviour,IAltasMap
{
    public string AltasName;
    public Sprite[] Sprites;
    private Dictionary<string, int> _index;
    public void Initlize()
    {
        if(_index==null)
        {
            _index = new Dictionary<string, int>();
            for(int i=0;i<Sprites.Length;i++)
            {
                Sprite sp = Sprites[i];
                if(sp!=null)
                {
                    try
                    {
                        _index.Add(sp.name.ToLower(), i);
                    }
                    catch(Exception e)
                    {
                        Debug.LogError(sp.name + "   " + e.Message);
                    }
                }
            }
        }
    }
    public Sprite GetSprite(int index)
    {
        if(index>=0&&index<Sprites.Length)
        {
            return Sprites[index];
        }
        return null;
    }
    public Sprite GetSprite(string id)
    {
        Initlize();
        if(_index.ContainsKey(id))
        {
            return Sprites[_index[id]];
        }
        return null;
    }
    public string GetAltasName()
    {
        return AltasName;
    }
    public static IAltasMap GetAltalMap(GameObject obj)
    {
        if(obj!=null)
        {
            AltasMap component = obj.GetComponent<AltasMap>();
            if(component!=null)
            {
                return component as IAltasMap;
            }
              
        }
        return null;
    }
}
