using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleGuidance : MonoBehaviour
{
    public static CircleGuidance instance;
    public Image target;
    private Vector3[] corners = new Vector3[4];
    private Vector4 center;
    private float radius;
    private Material material;
    private float currentRadius;
    private float shrinkTime = 0.2f;
    private GuidanceEventPenetrate eventPenetrate;
    private Transform guidleIcon;
    private void Awake()
    {
        instance = this;
    }
    public void Init(Image target)
    {
        this.target = target;
        eventPenetrate = GetComponent<GuidanceEventPenetrate>();
        if(eventPenetrate!=null)
        {
            eventPenetrate.SetTargetImage(target);
        }
        Canvas canvas = GameObject.Find("Canvas(Clone)").GetComponent<Canvas>();
        target.rectTransform.GetWorldCorners(corners);
        radius = Vector2.Distance(WorldToCanvasPos(canvas, corners[0]), WorldToCanvasPos(canvas, corners[2])) / 2;
        float x = corners[0].x + ((corners[3].x - corners[0].x) / 2);
        float y = corners[0].y + ((corners[1].y - corners[0].y) / 2);
        Vector3 centerWorld = new Vector3(x, y, 0);
        Vector2 center = WorldToCanvasPos(canvas, centerWorld);
        Vector4 centerMat = new Vector4(center.x, center.y, 0, 0);
        material = GetComponent<Image>().material;
        material.SetVector("_Center", centerMat);
        RectTransform canRectTransform = canvas.transform as RectTransform;
        if(canRectTransform!=null)
        {
            canRectTransform.GetWorldCorners(corners);
            foreach(var corner in corners)
            {
                currentRadius = Mathf.Max(Vector3.Distance(WorldToCanvasPos(canvas, corner), corner), currentRadius);
            }
        }
        material.SetFloat("_Slider", currentRadius);

        guidleIcon = transform.Find("guidleIcon");
        guidleIcon.transform.localPosition = new Vector3(center.x, center.y, 0);
    }

    private float shrinkVelocity = 0f;
    private void Update()
    {
        float value = Mathf.SmoothDamp(currentRadius, radius, ref shrinkVelocity, shrinkTime);
        if(!Mathf.Approximately(value,currentRadius))
        {
            currentRadius = value;
            material.SetFloat("_Slider", currentRadius);
        }
    }

    private Vector2 WorldToCanvasPos(Canvas canvas,Vector3 world)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, world, canvas.GetComponent<Camera>(), out position);
        return position;
    }
}
