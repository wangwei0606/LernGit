using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand 
{
    int Priority { get; }
    string Error { get; }
    bool Excute(CommandArguments args);
}
