using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WWTree : MonoBehaviour
{
    public GameObject NodeTemplate{get;private set;}
    [HideInInspector]
    public WWTreeNode m_RootTreeNode;
    public string rootText="Root";
    public Sprite ToggleSpriteOn;
    public Sprite ToggleSpriteOff;
    public float interval;
    private bool m_AllowmulChoice=false;
    private WWTreeNode m_SelectedNode;
    private List<WWTreeNode> m_SelectedNodes;
    public bool AllowMulChoice
    {
        get
        {
            return m_AllowmulChoice;
        }
        set
        {
            if(m_AllowmulChoice!=value)
            {
                if(value)
                {
                    if(m_SelectedNodes==null)
                    {
                        m_SelectedNodes=new List<WWTreeNode>();
                    }
                    if(m_SelectedNode!=null)
                    {
                        m_SelectedNodes.Add(m_SelectedNode);
                        m_SelectedNode=null;
                    }
                }
                else
                {
                    if(m_SelectedNodes!=null)
                    {
                        for (int i =  m_SelectedNodes.Count-1;i>=0; i--)
                        {
                            m_SelectedNodes[i].NodeToggle.isOn=false;
                        }
                        m_SelectedNodes.Clear();
                        m_SelectedNodes=null;
                    }
                }
                m_AllowmulChoice=value;
            }
        }
    }
    public WWTreeNode SelectedNode
    {
        get
        {
            return m_SelectedNode;
        }
    }
    public List<WWTreeNode> SelectedNodes
    {
        get
        {
            return m_SelectedNodes;
        }
    }
    public TreeEvent onSelectNode=new TreeEvent();
    public SwitchEvent onOn_Off=new SwitchEvent();
    public UnityEvent onRefresh=new UnityEvent();
    private WWTree()
    {
        if(m_AllowmulChoice)
        {
            m_SelectedNodes=new List<WWTreeNode>();
        }
    }
    private void Awake()
    {
        NodeTemplate=transform.Find("NodeTemplate").gameObject;
        NodeTemplate.GetComponent<RectTransform>().anchoredPosition=new Vector2(10000,10000);
    }
    public float Height
    {
        get
        {
            float y1=m_RootTreeNode.Rect.position.y;
            WWTreeNode node=m_RootTreeNode;
            while (node.ChildCount!=0)
            {
                node=node.GetChild(node.ChildCount-1);
            }
            float y2=node.Rect.position.y;
            return y1-y2+node.Rect.sizeDelta.y;
        }
    }
    public void GenerateTree(WWTreeNode rootNode)
    {
        if(m_RootTreeNode!=null)
        {
            m_RootTreeNode.Delete();
        }
        m_RootTreeNode=rootNode;
        m_RootTreeNode.CreateTree(this);
    }
    public bool Delete(string path)
    {
        WWTreeNode node=m_RootTreeNode.Find(path);
        if(node!=null)
        {
            node.Delete();
            return true;
        }
        return false;
    }
    public bool Delete()
    {
        if(m_RootTreeNode==null)
        {
            return false;
        }
        else
        {
            if(m_AllowmulChoice)
            {
                foreach (var item in m_SelectedNodes)
                {
                    item.NodeToggle.isOn=false;
                }
                m_SelectedNodes.Clear();
            }
            else
            {
                if(m_SelectedNodes!=null)
                {
                    m_SelectedNode.NodeToggle.isOn=false;
                    m_SelectedNode=null;
                }
            }
            m_RootTreeNode.Delete();
            return true;
        }
    }
    public float GetTreeHeight()
    {
        return m_RootTreeNode!=null ? m_RootTreeNode.GetItemCount()*NodeTemplate.GetComponent<RectTransform>().sizeDelta.y : 0;
    }
    public void SetAllSelectOff()
    {
        if(m_AllowmulChoice)
        {
            foreach (var item in m_SelectedNodes)
            {
                item.NodeToggle.isOn=false;
            }
            m_SelectedNodes.Clear();
        }
        else if(m_SelectedNode!=null)
        {
            m_SelectedNode=null;
        }
    }
    public void SetRootNodeActive(bool isActive)
    {
        if(m_RootTreeNode!=null)
        {
            m_RootTreeNode.Rect.GetChild(0).gameObject.SetActive(isActive);
            m_RootTreeNode.Rect.GetChild(1).gameObject.SetActive(isActive);
        }
    }
    internal void SelectTreeNode(WWTreeNode node)
    {
        if(m_AllowmulChoice)
        {
            m_SelectedNodes.Add(node);
            onSelectNode.Invoke(node);
            for (int i = 0; i < node.ChildCount; i++)
            {
                node.GetChild(i).NodeToggle.isOn=true;
            }
        }
        else
        {
            if(m_SelectedNode!=null)
            {
                m_SelectedNode.NodeToggle.isOn=false;
            }
            m_SelectedNode=node;
            onSelectNode.Invoke(node);
        }
    }
    internal void UnCheckTreeNode(WWTreeNode node)
    {
        if(m_AllowmulChoice)
        {
            if(!m_SelectedNodes.Remove(node))
            {
                throw new System.Exception("多选状态时存在隐患");
            }
            for (int i = 0; i < node.ChildCount; i++)
            {
                node.GetChild(i).NodeToggle.isOn=false;
            }
        }
        else if(node!=m_SelectedNode)
        {
            throw new System.Exception("单选状态时存在隐患");
        }
        else
        {
            m_SelectedNode=null;
        }
    }
    public void SetSelectNode(WWTreeNode node)
    {
        if(node!=null)
        {
            node.NodeToggle.isOn=true;
        }
    }
}

public class TreeEvent:UnityEvent<WWTreeNode>
{

}
public class SwitchEvent:UnityEvent<bool,WWTreeNode>
{
    
}