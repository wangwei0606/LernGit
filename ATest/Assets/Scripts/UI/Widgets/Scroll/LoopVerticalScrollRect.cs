using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Loop Vertical ScrollRect")]
[DisallowMultipleComponent]

public class LoopVerticalScrollRect : WLoopScrollRect
{
    public override bool IsFullFill()
    {
        if(content.sizeDelta.y>viewRect.sizeDelta.y)
        {
            return true;
        }
        return false;
    }

    protected override float GetDimension(Vector2 vector)
    {
        return vector.y;
    }

    protected override float GetSize(RectTransform item)
    {
        float size = 0;
        if(m_GridLayout!=null)
        {
            size = m_GridLayout.cellSize.y;
        }
        else
        {
            size = LayoutUtility.GetPreferredHeight(item);
        }
        if(size==0)
        {
            size = item.sizeDelta.y;
        }
        size += contentSpacing;
        return size;
    }

    protected override Vector2 GetVector(float value)
    {
        return new Vector2(0, value);
    }

    protected override void Awake()
    {
        base.Awake();
        directionSign = -1;
        createSimpleResourceManager();
        if(content!=null)
        {

        }
    }

    protected override void init()
    {
        base.init();
        directionSign = -1;
        createSimpleResourceManager();
    }

    public override void ScrollToBottom()
    {
        if(Application.isPlaying)
        {
            StopMovement();
            m_lock = true;
            int endIndex = totalCount - 1;
            itemTypeEnd = totalCount;
            float size = 0;
            while(size<viewRect.sizeDelta.y)
            {
                if(endIndex<0)
                {
                    break;
                }
                RectTransform newItem = InstantiateNextItem(endIndex);
                endIndex--;
                itemTypeStart = endIndex + 1;
                newItem.SetAsFirstSibling();
                size += GetSize(newItem);
            }
            m_lock = false;
            Vector2 pos = content.anchoredPosition;
            if(size<viewRect.sizeDelta.y)
            {
                pos.y = 0;
            }
            else
            {
                pos.y = size - viewRect.sizeDelta.y;
            }
            content.anchoredPosition = pos;
        }
    }

    protected override float getViewRectSize()
    {
        return viewRect.sizeDelta.y;
    }

    protected override bool UpdateItems(Bounds viewBounds, Bounds contentBounds)
    {
        bool changed = false;
        if(viewBounds.min.y<contentBounds.min.y+1)
        {
            //Debug.LogError("ver UpdateItems(Bounds viewBounds, Bounds contentBounds)");
            float size = NewItemAtEnd();
            if(size>0)
            {
                if(threshold<size)
                {
                    threshold = size * 1.1f;
                }
                changed = true;
            }
        }
        else if(viewBounds.min.y>contentBounds.min.y+threshold)
        {
            float size = DeleteItemAtEnd();
            if(size>0)
            {
                changed = true;
            }
        }
        if(viewBounds.max.y>contentBounds.max.y-1)
        {
            float size = NewItemAtStart();
            if(size>0)
            {
                if(threshold<size)
                {
                    threshold = size * 1.1f;
                }
                changed = true;
            }
        }
        else if(viewBounds.max.y<contentBounds.max.y-threshold)
        {
            float size = DeleteItemAtStart();
            if(size>0)
            {
                changed = true;
            }
        }
        return changed;
    }
}
