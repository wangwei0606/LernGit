using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using XCharts;

public class WWLineChart:MonoBehaviour
{
    LineChart lineChart;
    private void Start()
    {
        
    }

    public void initLineChart(int[] content,string[] xTitles)
    {
        for (int i = 0; i < xTitles.Length; i++)
        {
            lineChart.AddXAxisData(xTitles[i]);
        }
        for (int i = 0; i < content.Length; i++)
        {
            lineChart.AddData(0,content[i]);
        }
    }

    public void changeContent(int[] content)
    {
        var d = lineChart.series.GetSerie("serie1").data;
        int oldCount=d.Count;
        for (int i = 0; i < content.Length; i++)
        {
            if(i<=oldCount-1)
            {
                lineChart.UpdateData(0, i, content[i]);
            }
            else
            {
                lineChart.AddData(0,content[i]);
            }
            
        }
    }

    public void addCountent(double data,string xTitle)
    {
        lineChart.AddXAxisData(xTitle);
        lineChart.AddData(0,data);
        lineChart.RefreshChart();
    }

    private void Update()
    {
        
    }

    protected virtual void init()
    {
        lineChart = this.gameObject.GetComponent<LineChart>();
    }
    public static WWLineChart Create(GameObject go)
    {
        WWLineChart view = go.GetComponent<WWLineChart>();
        if(view==null)
        {
            view = go.AddComponent<WWLineChart>();
        }
        view.init();
        return view;
    }

    public void Dispose()
    {
        lineChart=null;
    }
}