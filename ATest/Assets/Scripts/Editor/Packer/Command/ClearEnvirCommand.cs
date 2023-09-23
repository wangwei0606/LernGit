using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearEnvirCommand : ICommand
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
            return (int)CommandEnum.ClearEnvir;
        }
    }
    public bool Excute(CommandArguments args)
    {
        return true;
    }
}
