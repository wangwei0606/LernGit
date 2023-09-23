using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectGuidance : MonoBehaviour
{
    public static RectGuidance instance;
    public Image target;
    private Vector3[] corners = new Vector3[4];
    private Vector4 rectCenter;
    private float targetOffsetX = 0;
    private float targetOffsetY = 0;
    private Material material;
    private float currentOffsetX = 0f;
    private float currentOffsetY = 0f;
    private float shrinkTime = 0.2f;
    private GuidanceEventPenetrate eventPenetrate;
    private Transform guidleIcon;
    private float startTime = 0.0f;
    private void Awake()
    {
        instance = this;
        guidleIcon = transform.Find("guidleIcon");
        guidleIcon.gameObject.SetActive(false);
    }

    public void Init(Image target)
    {
        startTime = Time.time;
        this.target = target;
        eventPenetrate = GetComponent<GuidanceEventPenetrate>();
        if(eventPenetrate!=null)
        {
            eventPenetrate.SetTargetImage(target);
        }
        Canvas canvas = GameObject.Find("Canvas(Clone)").GetComponent<Canvas>();
        target.rectTransform.GetWorldCorners(corners);
        targetOffsetX = Vector2.Distance(WorldToCanvasPos(canvas, corners[0]), WorldToCanvasPos(canvas, corners[3])) / 2f;
        targetOffsetY = Vector2.Distance(WorldToCanvasPos(canvas, corners[0]), WorldToCanvasPos(canvas, corners[1])) / 2f;
        float x = corners[0].x + ((corners[3].x - corners[0].x) / 2);
        float y = corners[0].y + ((corners[1].y - corners[0].y) / 2);
        Vector3 centerWorld = new Vector3(x, y, 0);
        Vector2 center = WorldToCanvasPos(canvas, centerWorld);
        Vector4 centerMat = new Vector4(center.x, center.y, 0, 0);
        rectCenter = new Vector4(center.x, center.y, 0, 0);
        material = GetComponent<Image>().material;
        material.SetVector("_Center", centerMat);
        RectTransform canRectTransform = canvas.transform as RectTransform;
        if(canRectTransform!=null)
        {
            canRectTransform.GetWorldCorners(corners);
            for(int i=0;i<corners.Length;i++)
            {
                if(i%2==0)
                {
                    currentOffsetX = Mathf.Max(Vector3.Distance(WorldToCanvasPos(canvas, corners[i]), center), currentOffsetX);
                }
                else
                {
                    currentOffsetY = Mathf.Max(Vector3.Distance(WorldToCanvasPos(canvas, corners[i]), center), currentOffsetY);
                }
            }
        }
        material.SetFloat("_SliderX", currentOffsetX);
        material.SetFloat("_SliderY", currentOffsetY);


    }

    private float shrinkVelocityX = 0f;
    private float shrinkVelocityY = 0f;
    private void Update()
    {
        //if(startTime>0 && (Time.time-startTime)<=shrinkTime)
        {
            float valueX = Mathf.SmoothDamp(currentOffsetX, targetOffsetX, ref shrinkVelocityX, shrinkTime);
            float valueY = Mathf.SmoothDamp(currentOffsetY, targetOffsetY, ref shrinkVelocityY, shrinkTime);
            if (!Mathf.Approximately(valueX, currentOffsetX))
            {
                currentOffsetX = valueX;
                material.SetFloat("_SliderX", currentOffsetX);
                guidleIcon.gameObject.SetActive(false);
            }
            else
            {
                guidleIcon.gameObject.SetActive(true);
                guidleIcon.transform.localPosition = new Vector3(rectCenter.x, rectCenter.y, 0);
            }
            if (!Mathf.Approximately(valueY, currentOffsetY))
            {
                currentOffsetY = valueY;
                material.SetFloat("_SliderY", currentOffsetY);
            }
        }
        
    }

    private Vector2 WorldToCanvasPos(Canvas canvas,Vector3 world)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, world, canvas.GetComponent<Camera>(), out position);
        return position;
    }
}
