using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuidanceEventPenetrate : MonoBehaviour,ICanvasRaycastFilter
{
    private Image targetImage;
    public void SetTargetImage(Image target)
    {
        targetImage = target;
    }
    public bool IsRaycastLocationValid(Vector2 sp,Camera eventCamera)
    {
        if(targetImage==null)
        {
            return true;
        }
        return !RectTransformUtility.RectangleContainsScreenPoint(targetImage.rectTransform, sp, eventCamera);
    }
}
