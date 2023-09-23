using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ModelConfig : ConfBase
{
    public string Id;
    public string ResPath;
    public override string UniqueId
    {
        get
        {
            return Id.ToString();
        }
    }
}