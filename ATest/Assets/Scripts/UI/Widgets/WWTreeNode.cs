using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WWTreeNode : IEnumerable<WWTreeNode>
{
    public RectTransform Rect{get; private set;}
    public Toggle NodeToggle{get;private set;}
    private WWTree m_tree;
    public WWTreeNode Parent{get;private set;}
    protected List<WWTreeNode> m_childs;
    private bool m_IsOn;
    private string text;
    private bool m_Interactable;
    public WWTreeNode()
    {
        m_IsOn=true;
        m_Interactable=true;
        m_childs=new List<WWTreeNode>();
    }
    public WWTreeNode(string text):this()
    {
        this.text=text;
    }
    public string Text
    {
        get
        {
            return text;
        }
        set
        {
            text=value;
            if(Rect)
            {
                Rect.Find("Body").Find("Text").GetComponent<Text>().text=text;
            }
        }
    }
    public WWTreeNode Root
    {
        get
        {
            WWTreeNode item=this;
            while (item.Parent!=null)
            {
                item=item.Parent;
            }
            return item;
        }
    }
    public bool Interactable
    {
        get
        {
            return m_Interactable;
        }
        set
        {
            if(m_Interactable!=value)
            {
                m_Interactable=value;
                Rect.Find("Body").GetComponent<Toggle>().interactable=value;
            }
        }
    }
    public int ChildCount
    {
        get
        {
            return m_childs.Count;
        }
    }
    private void RefreshView(bool isOn)
    {
        isOn&=m_IsOn;
        if(isOn)
        {
            foreach (var item in m_childs)
            {
                item.RefreshView(isOn);
                item.Rect.localScale=new Vector3(1,1,1);
            }
        }
        else
        {
            foreach (var item in m_childs)
            {
                item.RefreshView(isOn);
                item.Rect.localScale=new Vector3(1,0,1);
            }
        }
        m_tree.onRefresh.Invoke();
    }
    private void RefreshPos()
    {
        int index=0;
        if(Parent!=null)
        {
            foreach (var item in Parent.m_childs)
            {
                if(item==this)
                {
                    break;
                }
                index+=item.GetItemCount();
            }
        }
        Rect.anchoredPosition=new Vector2(0,-index*(Rect.sizeDelta.y+m_tree.interval));
        foreach (var item in m_childs)
        {
            item.RefreshPos();
        }
        m_tree.onRefresh.Invoke();
    }
    private void SetToggle(bool isActive)
    {
        Rect.Find("Toggle").gameObject.SetActive(isActive);
    }
    private void InternalCreateTree(WWTree tree)
    {
        InitEntity(tree);
        if(m_childs.Count==0)
        {
            SetToggle(false);
        }
        else
        {
            foreach (var item in m_childs)
            {
                item.InternalCreateTree(m_tree);
            }
        }
    }
    private void InitEntity(WWTree tree)
    {
        m_tree=tree;
        if(Parent==null)
        {
            Rect=Object.Instantiate(m_tree.NodeTemplate,m_tree.NodeTemplate.transform.parent.Find("Root")).GetComponent<RectTransform>();
        }
        else
        {
            Rect=Object.Instantiate(m_tree.NodeTemplate,Parent.Rect.Find("Child")).GetComponent<RectTransform>();
        }
        Toggle toggle=Rect.Find("Toggle").GetComponent<Toggle>();
        Image toggleImage=toggle.GetComponent<Image>();
        toggle.onValueChanged.AddListener((value)=>
        {
            m_IsOn=value;
            if(value)
            {
                toggleImage.sprite=m_tree.ToggleSpriteOn;
            }
            else
            {
                toggleImage.sprite=m_tree.ToggleSpriteOff;
            }
            RefreshView(value);
            Root.RefreshPos();
            tree.onOn_Off.Invoke(value,this);
        });
        toggleImage.sprite=m_tree.ToggleSpriteOn;
        NodeToggle=Rect.Find("Body").GetComponent<Toggle>();
        NodeToggle.onValueChanged.AddListener((value)=>
        {
            if(value)
            {
                m_tree.SelectTreeNode(this);
            }
            else
            {
                m_tree.UnCheckTreeNode(this);
            }
        });
        Rect.Find("Body").Find("Text").GetComponent<Text>().text=this.Text;
    }
    public int GetItemCount()
    {
        if(m_childs.Count==0 || !m_IsOn)
        {
            return 1;
        }
        else
        {
            int count=0;
            foreach (var item in m_childs)
            {
                count+=item.GetItemCount();
            }
            return count+1;
        }
    }
    public int GetSiblingIndex()
    {
        if(Parent!=null)
        {
            int index=0;
            foreach (var item in Parent.m_childs)
            {
                index++;
                if(item==this)
                {
                    return index;
                }
            }
        }
        return 0;
    }
    public void SetParent(WWTreeNode parent)
    {
        if(parent!=this)
        {
            Parent.m_childs.Remove(this);
            parent.AddChild(this);
        }
        else
        {
            throw new System.Exception("error parent==self");
        }
    }
    public WWTreeNode Find(string path)
    {
        var temp=path.Split(new char[]{'/'},2);
        if(temp.Length==1)
        {
            foreach (var item in m_childs)
            {
                if(item.Text==temp[0])
                {
                    return item;
                }
            }
            return null;
        }
        WWTreeNode node=null;
        foreach (var item in m_childs)
        {
            if(item.Text==temp[0])
            {
                node=item;
                break;
            }
        }
        if(node==null)
        {
            return node;
        }
        else
        {
            return node.Find(temp[1]);
        }
    }
    public WWTreeNode GetChild(int index)
    {
        return m_childs[index];
    }
    public WWTreeNode AddChild(WWTreeNode node)
    {
        m_childs.Add(node);
        node.Parent=this;
        return this;
    }
    public WWTreeNode AddChild(string text)
    {
        return AddChild(new WWTreeNode(text));
    }
    public void CreateTree(WWTree tree)
    {
        InternalCreateTree(tree);
        RefreshPos();
    }
    public WWTreeNode CreateChild(WWTreeNode item)
    {
        if(ChildCount==0)
        {
            SetToggle(true);
        }
        AddChild(item);
        item.InternalCreateTree(m_tree);
        RefreshView(m_IsOn);
        Root.RefreshPos();
        return this;
    }
    public WWTreeNode CreateChild(string text)
    {
        WWTreeNode item=new WWTreeNode(text);
        return CreateChild(item);
    }
    public void Delete()
    {
        if(Rect==null)
        {
            return;
        }
        Object.Destroy(Rect.gameObject);
        if(Parent!=null)
        {
            Parent.m_childs.Remove(this);
            if(Parent.ChildCount==0)
            {
                Parent.SetToggle(false);
            }
        }
        Root.RefreshPos();
    }
    public void SetHide(bool isHide=true)
    {
        Rect.GetChild(0).gameObject.SetActive(!isHide);
        Rect.GetChild(1).gameObject.SetActive(!isHide);
    }
    public IEnumerator<WWTreeNode> GetEnumerator()
    {
        return m_childs.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return m_childs.GetEnumerator();
    }
    public T When<T>(System.Func<T,bool> func) where T:WWTreeNode
    {
        return InternalWhen(func);
    }
    private T InternalWhen<T>(System.Func<T,bool> func) where T:WWTreeNode
    {
        foreach (var item in m_childs)
        {
            if(func.Invoke(item as T))
            {
                return item as T;
            }
            else
            {
                WWTreeNode node=item.InternalWhen(func);
                if(node!=null)
                {
                    return node as T;
                }
            }
        }
        return null;
    }
    public void Foreach(System.Action<WWTreeNode> action,bool includeSelf=true)
    {
        if(includeSelf)
        {
            action.Invoke(this);
        }
        Foreach(action);
    }
    private void Foreach(System.Action<WWTreeNode> action)
    {
        if(m_childs.Count>0)
        {
            foreach (var item in m_childs)
            {
                action.Invoke(item);
                item.Foreach(action);
            }
        }
    }
    public void Foreach<T>(System.Action<T> action,bool includeSelf=true) where T:WWTreeNode
    {
        if(includeSelf)
        {
            action.Invoke(this as T);
        }
        Foreach<T>(action);
    }
    private void Foreach<T>(System.Action<T> action) where T:WWTreeNode
    {
        if(m_childs.Count>0)
        {
            foreach (var item in m_childs)
            {
                action.Invoke(item as T);
                item.Foreach(action);
            }
        }
    }

}

public sealed class WWTreeNode<T>:WWTreeNode
{
    public T data;
    public WWTreeNode(string name,T data):base(name)
    {
        this.data=data;
    }
}