using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeBuildCommand : ICommand
{
    public string Error
    {
        get;
        set;
    }
    public int Priority
    {
        get
        {
            return (int)CommandEnum.BuildBefore;
        }
    }
    public bool Excute(CommandArguments args)
    {
        return true;
    }
}
