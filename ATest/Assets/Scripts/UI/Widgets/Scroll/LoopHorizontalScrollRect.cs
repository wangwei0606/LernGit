using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Loop Horizontal ScrollRect",50)]
[DisallowMultipleComponent]

public class LoopHorizontalScrollRect : WLoopScrollRect
{
    public override bool IsFullFill()
    {
        if(content.sizeDelta.x>viewRect.sizeDelta.x)
        {
            return true;
        }
        return false;
    }

    protected override float GetDimension(Vector2 vector)
    {
        return vector.x;
    }

    protected override float GetSize(RectTransform item)
    {
        float size = 0;
        if(m_GridLayout!=null)
        {
            size = m_GridLayout.cellSize.x;
        }
        else
        {
            size = LayoutUtility.GetPreferredWidth(item);
        }
        if(size==0)
        {
            size = item.sizeDelta.x;
        }
        size += contentSpacing;
        return size;
    }

    protected override Vector2 GetVector(float value)
    {
        return new Vector2(-value, 0);
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void init()
    {
        base.init();
        directionSign = 1;
        createSimpleResourceManager();
        GridLayoutGroup layout = content.GetComponent<GridLayoutGroup>();

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
            while(size<viewRect.sizeDelta.x)
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
            if(size<viewRect.sizeDelta.x)
            {
                pos.x = 0;
            }
            else
            {
                pos.x = size - viewRect.sizeDelta.x;
            }
            content.anchoredPosition = pos;
        }
    }

    protected override float getViewRectSize()
    {
        return viewRect.sizeDelta.x;
    }

    protected override bool UpdateItems(Bounds viewBounds, Bounds contentBounds)
    {
        bool changed = false;
        if(viewBounds.max.x>contentBounds.max.x)
        {
            //Debug.LogError("hor UpdateItems(Bounds viewBounds, Bounds contentBounds)");
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
        else if(viewBounds.max.x<contentBounds.max.x-threshold)
        {
            float size = DeleteItemAtEnd();
            if(size>0)
            {
                changed = true;
            }
        }

        if(viewBounds.min.x<contentBounds.min.x)
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
        else if(viewBounds.min.x>contentBounds.min.x+threshold)
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