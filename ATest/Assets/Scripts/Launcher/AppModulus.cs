using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AppModulus
{
    public static AppModulus _instance = null;
    public static void Initilize()
    {
        if(_instance==null)
        {
            _instance = new AppModulus();
        }
    }

    private AppModulus()
    {
        initModulus();
    }

    private void initModulus()
    {
        //UICSToLua.Initilize();
        AppAudioWidget.Initilize();
    }

    private void onResease()
    {
        
    }
}