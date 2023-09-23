using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum PoolType
{
    None,
    UseTime,
    Level,
    Global
}

public enum PoolUseType
{
    None,
    Model,
    UI,
    Effect,
    Atlas,
    Audio
}