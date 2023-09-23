using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyAI
{
    public class ANode : System.IComparable
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public ANode parent = null;
        public List<ANode> adjacent = new List<ANode>();
        public int h = 0;
        public int g = 0;
        public int f = 0;
        public void F(ANode startNode, ANode endNode)
        {
            h = Mathf.Abs(endNode.Row - Row) + Mathf.Abs(endNode.Col - Col);
            g = Mathf.Abs(startNode.Row - Row) + Mathf.Abs(startNode.Col - Col);
            f = g + h;
        }
        public void Clear()
        {
            parent = null;
            h = 0;
            g = 0;
            f = 0;
        }
        public int CompareTo(object obj)
        {
            ANode node = obj as ANode;
            if (f - node.f < 0)
            {
                return -1;
            }
            else if (f - node.f == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }

    public class AMap
    {
        public int cols = 0;
        public int rows = 0;
        public ANode[] aNodes;
        public AMap(int[,] mapArray)
        {
            rows = mapArray.GetLength(0);
            cols = mapArray.GetLength(1);
            aNodes = new ANode[mapArray.Length]; 
            for (int i = 0; i < aNodes.Length; i++)
            {
                ANode node = new ANode();
                node.Row = i / cols;
                node.Col = i - node.Row * cols;
                aNodes[i] = node;
            }
            int row, col;
            for (int j = 0; j < aNodes.Length; j++)
            {
                row = aNodes[j].Row;
                col = aNodes[j].Col;
                if (mapArray[row, col] != 1)
                {
                    if (row > 0 && mapArray[row - 1, col] != 1)
                    {
                        aNodes[j].adjacent.Add(aNodes[(row - 1) * cols + col]);
                    }
                    if (col + 1 < cols && mapArray[row, col + 1] != 1)
                    {
                        aNodes[j].adjacent.Add(aNodes[row * cols + col + 1]);
                    }
                    if (row + 1 < rows && mapArray[row + 1, col] != 1)
                    {
                        aNodes[j].adjacent.Add(aNodes[(row + 1) * cols + col]);
                    }
                    if (col > 0 && mapArray[row, col - 1] != 1)
                    {
                        aNodes[j].adjacent.Add(aNodes[row * cols + col - 1]);
                    }
                }
            }
        }    
    }

    public class AStarAlgorithm
    {
        private ANode startNode;
        private ANode destNode;
        private List<ANode> openSet = new List<ANode>();
        private List<ANode> closeSet = new List<ANode>();
        private AMap map;
        public AStarAlgorithm(AMap map)
        {
            this.map = map;
        }
        private ANode FindLowest()
        {
            openSet.Sort();
            return openSet[0];
        }
        private void AddAdjacent(ANode node)
        {
            for (int i = 0; i < node.adjacent.Count; i++)
            {
                if (closeSet.Contains(node.adjacent[i]))
                {
                    continue;
                }
                else if (openSet.Contains(node.adjacent[i]))
                {
                    int newG = node.adjacent[i].g + (Mathf.Abs(node.Row - node.adjacent[i].Row) + Mathf.Abs(node.Col - node.adjacent[i].Col));
                    if (newG < node.adjacent[i].g)
                    {
                        node.adjacent[i].parent = node;
                        node.adjacent[i].g = newG;
                        node.adjacent[i].f = newG + node.adjacent[i].h;
                    }
                }
                else
                {
                    node.adjacent[i].parent = node;
                    node.adjacent[i].F(startNode, destNode);
                    openSet.Add(node.adjacent[i]);
                }
            }
        }

        public void UpdateMap(AMap map)
        {
            this.map = map;
        }
        public void Start(ANode startNode, ANode endNode)
        {
            openSet.Clear();
            closeSet.Clear();
            closeSet.Add(startNode);
            destNode = endNode;
            this.startNode = startNode;
            Debug.LogError(map.aNodes.Length);
            for (int i = 0; i < map.aNodes.Length; i++)
            {
                Debug.LogError("clear node");
                map.aNodes[i].Clear();
            }
        }

        public Stack<ANode> Find()
        {
            Stack<ANode> path = new Stack<ANode>();
            ANode curNode = closeSet[0];
            while (curNode != destNode)
            {
                AddAdjacent(curNode);
                if (openSet.Count == 0)
                {
                    break;
                }
                curNode = FindLowest();
                openSet.Remove(curNode);
                closeSet.Add(curNode);
            }
            if (curNode == destNode)
            {
                ANode node = destNode;
                while (node != null)
                {
                    path.Push(node);
                    node = node.parent;
                }
            }
            else
            {
                return null;
            }
            return path;
        }
    }
}
