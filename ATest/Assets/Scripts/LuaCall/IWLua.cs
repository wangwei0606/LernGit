using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IWLua
{
    void call(object arg1);
    void call(object arg1, object arg2);
    void call(object arg1, object arg2, object arg3);
    void call(object arg1, object arg2, object arg3, object arg4);
    void call(object arg1, object arg2, object arg3, object arg4, object arg5);
    void call(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6);
    void Dispatch(string eventName, short protocol, ByteArray bytes);
}