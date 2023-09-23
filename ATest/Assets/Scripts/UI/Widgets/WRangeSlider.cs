using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class WRangeSlider : MonoBehaviour
{
    RangeSlider slider;
    RangeSliderValueChangeHandle handle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addValueChanged(RangeSliderValueChangeHandle hand)
    {
        handle = hand;
        slider.OnValueChanged.AddListener(onValueChange);
    }

    protected virtual void init()
    {
        slider = this.gameObject.GetComponent<RangeSlider>();
    }

    private void onValueChange(float min,float max)
    {
        handle(min, max);
    }

    public static WRangeSlider Create(GameObject go)
    {
        WRangeSlider view = go.GetComponent<WRangeSlider>();
        if (view == null)
        {
            view = go.AddComponent<WRangeSlider>();
        }
        view.init();
        return view;
    }

    public void Dispose()
    {
        slider.OnValueChanged.RemoveAllListeners();
        slider = null;
    }
}
