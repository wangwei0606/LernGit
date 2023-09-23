using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TimerUtils
{
    private static DateTime now;
    private static DateTime startTime;
    private static int timeId;
    private static System.Object lockObj;

    static TimerUtils()
    {
        lockObj = new object();
        startTime = new DateTime(1970, 1, 1, 8, 0, 0);
    }
    public static double GetTime(DateTime t)
    {
        double ts = (t - startTime).TotalMilliseconds;
        return ts;
    }
    internal static int GetTimeId()
    {
        lock(lockObj)
        {
            return ++timeId;
        }
    }

    internal static double GetNowTime()
    {
        now = DateTime.Now;
        return GetTime(now);
    }
}
