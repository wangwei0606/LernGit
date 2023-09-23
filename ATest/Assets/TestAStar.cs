using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyAI;

public class TestAStar : MonoBehaviour
{
    public int[,] map;
    public RawImage[] images;
    public int startRow = 0;
    public int startCol = 0;
    public int endRow = 0;
    public int endCol = 0;

    int GetIndex(int row,int col,int[,] tempMap)
    {
        int cols = tempMap.GetLength(1);
        return row * cols + col;
    }

    void ResetMapColor(AMap map)
    {
        for(int i=0;i<map.aNodes.Length;i++)
        {
            images[i].color = map.aNodes[i].adjacent.Count == 0 ? Color.gray : Color.white;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        map = new int[,]
            {
                {1,1,1,1,1,1,1,1,1,1 },
                {1,0,0,0,1,0,0,0,0,1 },
                {1,0,0,0,1,0,0,0,0,1 },
                {1,0,0,0,1,1,1,1,0,1 },
                {1,0,0,0,1,0,0,0,0,1 },
                {1,0,1,1,0,0,0,0,0,1 },
                {1,0,0,0,0,0,0,0,0,1 },
                {1,1,1,1,1,1,1,1,1,1 },
            };

        if(map[startRow,startCol]==1)
        {
            Debug.LogError("起点是墙壁");
            return;
        }
        if(map[endRow,endCol]==1)
        {
            Debug.LogError("终点是墙壁");
            return;
        }
        if(startRow<0 || startRow>=map.GetLength(0) || startCol<0 || startCol>=map.GetLength(1) ||
            endRow<0 || endRow>=map.GetLength(0) || endCol<0 || endCol>=map.GetLength(1))
        {
            Debug.LogError("位置非法");
            return;
        }
        AMap myMap = new AMap(map);
        AStarAlgorithm gbpa = new AStarAlgorithm(myMap);
        Debug.LogError("start-----");
        gbpa.Start(myMap.aNodes[GetIndex(startRow, startCol, map)], myMap.aNodes[GetIndex(endRow, endCol, map)]);
        Stack<ANode> path = gbpa.Find();
        ResetMapColor(myMap);
        if(path!=null)
        {
            Debug.LogError("找到路径");
            while(path.Count!=0)
            {
                ANode node = path.Pop();
                images[GetIndex(node.Row, node.Col, map)].color = Color.blue;
            }
        }
        else
        {
            Debug.LogError("未找到路径");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
