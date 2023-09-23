using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Dropdownscript:MonoBehaviour
{
    public string[] contentText;
    Dropdown dropDown;
    DropDownValueChangeHandle handle;
    private void Start()
    {
        
    }

    public void Initilize(string[] content,DropDownValueChangeHandle hand)
    {
        contentText = content;
        handle = hand;
        dropDown = this.GetComponent<Dropdown>();
        UpdateDorpDown(content);
        dropDown.onValueChanged.AddListener(onValueChange);
    }

    private void onValueChange(int value)
    {
        handle(dropDown.value);
    }

    public void changeContent(string[] content)
    {
        contentText = content;
        dropDown.value = 0;
        UpdateDorpDown(content);
    }
    public void changeDropValue(int val)
    {
        dropDown.value = val;
        dropDown.captionText.text = contentText[val];
    }

    public int getValue()
    {
        return dropDown.value;
    }

    void UpdateDorpDown(string[] content)
    {
        dropDown.options.Clear();
        Dropdown.OptionData optionData;
        for(int i=0;i<content.Length;i++)
        {
            optionData = new Dropdown.OptionData();
            optionData.text = content[i];
            dropDown.options.Add(optionData);
        }
        dropDown.captionText.text = content[0];
    }
    private void Update()
    {
        
    }
    protected virtual void init()
    {
        dropDown = this.gameObject.GetComponent<Dropdown>();
    }

    public static Dropdownscript Create(GameObject go)
    {
        Dropdownscript view = go.GetComponent<Dropdownscript>();
        if(view==null)
        {
            view = go.AddComponent<Dropdownscript>();
        }
        view.init();
        return view;
    }

    public void Dispose()
    {
        dropDown.onValueChanged.RemoveAllListeners();
        dropDown = null;
    }
}