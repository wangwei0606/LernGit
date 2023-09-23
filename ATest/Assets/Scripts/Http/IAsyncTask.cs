using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IAsyncTask
{
    string CustomID { get; set; }
    string LastError { get; set; }
    int StateCode { get; set; }
}
