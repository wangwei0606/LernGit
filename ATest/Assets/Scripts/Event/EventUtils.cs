using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class EventUtils
{
    public static int UniqueId
    {
        get
        {
            return BitConverter.ToInt32(Encoding.UTF8.GetBytes(System.Guid.NewGuid().ToString()), 0);
        }
    }
}
