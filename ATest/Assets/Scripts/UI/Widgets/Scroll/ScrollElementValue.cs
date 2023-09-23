using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ScrollElementValue : MonoBehaviour
{
    public enum ElementType
    {
        Fixed,
        Random
    }

    [SerializeField]
    public ElementType Type;
    public Rect ElementRect;
}