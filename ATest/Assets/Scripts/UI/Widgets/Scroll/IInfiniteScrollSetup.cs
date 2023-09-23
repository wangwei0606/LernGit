using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IInfiniteScrollSetup
{
    void OnPostSetupItems();
    void OnInitItem(GameObject obj);
    void OnUpdateItem(int wrapIndex, int realIndex, GameObject obj);
}