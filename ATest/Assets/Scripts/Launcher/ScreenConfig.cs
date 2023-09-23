using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ScreenConfig
{
    private static int quality = 0;
    private static int SCREEN_WIDTH = 1280;
    private static int SCREEN_HEIGHT = 720;
    private static int REAL_SCREEN_WIDTH = 1280;
    private static int REAL_SCREEN_HEIGHT = 720;
    public static void Initilize()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        SCREEN_WIDTH = Screen.width;
        SCREEN_HEIGHT = Screen.height;
        REAL_SCREEN_WIDTH = Screen.width;
        REAL_SCREEN_HEIGHT = Screen.height;
    }
    public static int GetRealWidth()
    {
        return REAL_SCREEN_WIDTH;
    }
    public static int GetRealHeight()
    {
        return REAL_SCREEN_HEIGHT;
    }
    public static void modifyScreen(int qua)
    {
        if(quality==qua)
        {
            return;
        }
        quality = qua;
        if(Application.platform==RuntimePlatform.Android)
        {
            if(qua==1)
            {
                if(SCREEN_WIDTH>1800&&SCREEN_WIDTH<2000&&SCREEN_HEIGHT>1000&&SCREEN_HEIGHT<1200)
                {
                    REAL_SCREEN_WIDTH = 1280;
                    REAL_SCREEN_HEIGHT = 720;
                }
                else if(SCREEN_WIDTH>1000&&SCREEN_WIDTH<1400&SCREEN_HEIGHT>600&&SCREEN_HEIGHT<900)
                {
                    REAL_SCREEN_WIDTH = 960;
                    REAL_SCREEN_HEIGHT = 540;
                }
                GameUtil.RefreshScreenPrefix();
                Screen.SetResolution(REAL_SCREEN_WIDTH, REAL_SCREEN_HEIGHT, true);
            }
            else
            {
                REAL_SCREEN_WIDTH = SCREEN_WIDTH;
                REAL_SCREEN_HEIGHT = SCREEN_HEIGHT;
                GameUtil.RefreshScreenPrefix();
                Screen.SetResolution(REAL_SCREEN_WIDTH, REAL_SCREEN_HEIGHT, true);
            }
        }
    }
}
