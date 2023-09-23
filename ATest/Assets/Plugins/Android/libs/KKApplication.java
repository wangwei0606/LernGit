package com.dysgd.kkludotpdz.application;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.Application;
import android.content.BroadcastReceiver;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.pm.ApplicationInfo;
import android.content.pm.PackageManager;
import android.content.res.AssetFileDescriptor;
import android.content.res.AssetManager;
import android.net.Uri;
import android.os.Build;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;
import androidx.core.content.FileProvider;

import android.os.Process;
import android.util.Log;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.lang.reflect.Field;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.Map;


@SuppressLint({"NewApi"})
public class KKApplication
        extends Application
{
    protected static float curBatteryPercent = 1.0F;
    private BroadcastReceiver batteryReceiver;

    protected void attachBaseContext(Context base)
    {
        super.attachBaseContext(base);
    }

    public void onCreate()
    {
        registerBatteryReceiver();
    }

    protected void registerBatteryReceiver()
    {
        this.batteryReceiver = new BatteryChangedReceiver();
        registerReceiver(this.batteryReceiver, new IntentFilter("android.intent.action.BATTERY_CHANGED"));
    }

    protected void unRegisterBatteryReceiver()
    {
        if (this.batteryReceiver != null) {
            unregisterReceiver(this.batteryReceiver);
        }
        this.batteryReceiver = null;
    }

    public void onLowMemory()
    {
        super.onLowMemory();
    }

    public void onTrimMemory(int level)
    {
        super.onTrimMemory(level);
    }

    public void onTerminate()
    {
        unRegisterBatteryReceiver();
    }

    class BatteryChangedReceiver
            extends BroadcastReceiver
    {
        BatteryChangedReceiver() {}

        public void onReceive(Context context, Intent intent)
        {
            if (intent.getAction().equalsIgnoreCase("android.intent.action.BATTERY_CHANGED"))
            {
                int level = intent.getIntExtra("level", 1);
                int scale = intent.getIntExtra("scale", 1);
                System.out.println("  电量改变 level=" + level + "   scale=" + scale);
                KKApplication.curBatteryPercent = level * 100 / scale;
                System.out.println("  电量改变  =" + KKApplication.curBatteryPercent);
            }
        }
    }

    public static float getCurBatteryPercent()
    {
        System.out.println("  电量获得 =" + curBatteryPercent);
        return curBatteryPercent;
    }

    public String getAppId(Context paramContext)
    {
        try
        {
            ApplicationInfo localApplicationInfo = paramContext.getPackageManager()
                    .getApplicationInfo(paramContext.getPackageName(), PackageManager.GET_META_DATA);
            Object localObject = localApplicationInfo.metaData.get("YvImSdkAppId");
            return localObject.toString();
        }
        catch (Exception localException)
        {
            localException.printStackTrace();
        }
        return null;
    }

    public static void installApk(String file)
    {
        Context context = getApp().getApplicationContext();
        try
        {
            File apkfile = new File(file);
            if (!apkfile.exists()) {
                return;
            }
            if (Build.VERSION.SDK_INT >= 26)
            {
                boolean b = getApp().getPackageManager().canRequestPackageInstalls();
                if (b) {
                    _installApk(context, apkfile);
                } else {
                    ActivityCompat.requestPermissions(getCurrentActivity(), new String[] { "android.permission.REQUEST_INSTALL_PACKAGES" }, 10010);
                }
            }
            else
            {
                _installApk(context, apkfile);
            }
            android.os.Process.killProcess(android.os.Process.myPid());
        }
        catch (Exception e)
        {
            Log.e("BCGame", e.getMessage() + " StackTrace:" + e.getStackTrace().toString());
        }
    }

    private static void _installApk(Context context, File apkFile)
    {
        if ((context == null) || (apkFile == null)) {
            return;
        }
        Intent intent = new Intent("android.intent.action.VIEW");
        if (Build.VERSION.SDK_INT >= 24)
        {
            Uri apkUri = FileProvider.getUriForFile(context, "com.dysgd.kkludotpdz.fileProvider", apkFile);

            intent.addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION);
            intent.addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION);
            intent.setDataAndType(apkUri, "application/vnd.android.package-archive");
        }
        else
        {
            intent.addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION);
            intent.setDataAndType(Uri.fromFile(apkFile), "application/vnd.android.package-archive");
        }
        context.startActivity(intent);
    }

    public static String getCpuInfo()
    {
        String result = "";
        try
        {
            String[] args = { "/system/bin/cat", "/proc/cpuinfo" };
            ProcessBuilder cmd = new ProcessBuilder(args);

            java.lang.Process process = cmd.start();
            StringBuffer sb = new StringBuffer();
            String readLine = "";
            BufferedReader responseReader = new BufferedReader(
                    new InputStreamReader(process.getInputStream(), "utf-8"));
            while ((readLine = responseReader.readLine()) != null) {
                sb.append(readLine);
            }
            responseReader.close();
            result = sb.toString().toLowerCase();
        }
        catch (IOException localIOException) {}
        return result;
    }

    public static Application getApp()
    {
        Application application = null;
        try
        {
            Class<?> activityThreadClass = Class.forName("android.app.ActivityThread");
            Method method2 = activityThreadClass.getMethod("currentActivityThread", new Class[0]);

            Object localObject = method2.invoke(null, null);
            Method method = activityThreadClass.getMethod("getApplication", new Class[0]);
            application = (Application)method.invoke(localObject, null);
        }
        catch (ClassNotFoundException e)
        {
            e.printStackTrace();
        }
        catch (NoSuchMethodException e)
        {
            e.printStackTrace();
        }
        catch (IllegalAccessException e)
        {
            e.printStackTrace();
        }
        catch (IllegalArgumentException e)
        {
            e.printStackTrace();
        }
        catch (InvocationTargetException e)
        {
            e.printStackTrace();
        }
        return application;
    }

    public static int checkLocationStatus()
    {
        if (ContextCompat.checkSelfPermission(getCurrentActivity(), "android.permission.ACCESS_COARSE_LOCATION") != 0)
        {
            ActivityCompat.requestPermissions(getCurrentActivity(), new String[] { "android.permission.ACCESS_COARSE_LOCATION" }, 1);
            return 0;
        }
        return 1;
    }

    public static int checkVoiceStatus()
    {
        if (ContextCompat.checkSelfPermission(getCurrentActivity(), "android.permission.RECORD_AUDIO") != 0)
        {
            ActivityCompat.requestPermissions(getCurrentActivity(), new String[] { "android.permission.RECORD_AUDIO" }, 1);
            return 0;
        }
        return 1;
    }

    public static void lineShare(String url, String content)
    {
        try
        {
            ComponentName cn = new ComponentName("jp.naver.line.android", "jp.naver.line.android.activity.selectchat.SelectChatActivity");
            Intent shareIntent = new Intent();
            shareIntent.setAction("android.intent.action.SEND");
            shareIntent.setType("text/plain");
            shareIntent.putExtra("android.intent.extra.TEXT", content + " " + url);
            shareIntent.setComponent(cn);
            getCurrentActivity().startActivity(Intent.createChooser(shareIntent, "share"));
        }
        catch (Exception ex)
        {
            Log.e("tag", "lineShare:" + ex);
        }
    }

    public static void emailShare(String url, String content)
    {
        try
        {
            Intent shareIntent = new Intent();
            shareIntent.setAction("android.intent.action.SEND");
            shareIntent.setType("text/plain");
            shareIntent.putExtra("android.intent.extra.TEXT", content + " " + url);
            getCurrentActivity().startActivity(Intent.createChooser(shareIntent, "share"));
        }
        catch (Exception ex)
        {
            Log.e("tag", "lineShare:" + ex);
        }
    }

    public static void whatsAppShare(String url, String content)
    {
        try
        {
            Intent vIt = new Intent("android.intent.action.SEND");
            //vIt.setPackage("com.whatsapp");
            vIt.setType("text/plain");
            vIt.putExtra("android.intent.extra.TEXT", content + " " + url);
            getCurrentActivity().startActivity(Intent.createChooser(vIt, "share"));
        }
        catch (Exception ex)
        {
            Log.e("tag", "whatsAppShare:" + ex);
        }
    }
    
    public static void whatsAppShareImage(String filePath)
    {
        try
        {
            Intent vIt = new Intent();
            vIt.setAction(Intent.ACTION_SEND);
            //vIt.setPackage("com.whatsapp");
            vIt.setType("image/*");
            vIt.putExtra(Intent.EXTRA_STREAM, Uri.parse(filePath));
            getCurrentActivity().startActivity(Intent.createChooser(vIt, "shareImage"));
        }
        catch (Exception ex)
        {
            Log.e("tag", "whatsAppShareImage:" + ex);
        }
    }

    public static void emailShareImage(String filePath)
    {
        try
        {
            Intent vIt = new Intent();
            vIt.setAction(Intent.ACTION_SEND);
            vIt.setType("image/*");
            vIt.putExtra(Intent.EXTRA_STREAM, Uri.parse(filePath));
            getCurrentActivity().startActivity(Intent.createChooser(vIt, "shareImage"));
        }
        catch (Exception ex)
        {
            Log.e("tag", "emailShareImage:" + ex);
        }
    }

    public static void messengerShare(String url, String content)
    {
        try
        {
            Intent vIt = new Intent("android.intent.action.SEND");
            vIt.setPackage("com.facebook.orca");
            vIt.setType("text/plain");
            vIt.putExtra("android.intent.extra.TEXT", content + " " + url);
            getCurrentActivity().startActivity(Intent.createChooser(vIt, "share"));
        }
        catch (Exception ex)
        {
            Log.e("tag", "messengerShare:" + ex);
        }
    }

    public static void telegramShare(String url, String content)
    {
        try
        {
            Intent vIt = new Intent("android.intent.action.SEND");
            vIt.setPackage("org.telegram.messenger");
            vIt.setType("text/plain");
            vIt.putExtra("android.intent.extra.TEXT", content + " " + url);
            getCurrentActivity().startActivity(Intent.createChooser(vIt, "share"));
        }
        catch (Exception ex)
        {
            Log.e("tag", "telegramShare:" + ex);
        }
    }

    public static void wechatShare(String url, String content)
    {
        try
        {
            Intent vIt = new Intent("android.intent.action.SEND");
            vIt.setPackage("com.tencent.mm");
            vIt.setType("text/plain");
            vIt.putExtra("android.intent.extra.TEXT", content + " " + url);
            getCurrentActivity().startActivity(Intent.createChooser(vIt, "share"));
        }
        catch (Exception ex)
        {
            Log.e("tag", "wechatShare:" + ex);
        }
    }

    public static void instagramShare(String url, String content)
    {
        try
        {
            Intent vIt = new Intent("android.intent.action.SEND");
            vIt.setPackage("com.instagram.android");
            vIt.setType("text/plain");
            vIt.putExtra("android.intent.extra.TEXT", content + " " + url);
            getCurrentActivity().startActivity(Intent.createChooser(vIt, "share"));
        }
        catch (Exception ex)
        {
            Log.e("tag", "instagramShare:" + ex);
        }
    }

    public static Activity getCurrentActivity()
    {
        try
        {
            Class activityThreadClass = Class.forName("android.app.ActivityThread");
            Object activityThread = activityThreadClass.getMethod("currentActivityThread", new Class[0]).invoke(
                    null, new Object[0]);
            Field activitiesField = activityThreadClass.getDeclaredField("mActivities");
            activitiesField.setAccessible(true);
            Map activities = (Map)activitiesField.get(activityThread);
            for (Object activityRecord : activities.values())
            {
                Class activityRecordClass = activityRecord.getClass();
                Field pausedField = activityRecordClass.getDeclaredField("paused");
                pausedField.setAccessible(true);
                if (!pausedField.getBoolean(activityRecord))
                {
                    Field activityField = activityRecordClass.getDeclaredField("activity");
                    activityField.setAccessible(true);
                    return (Activity)activityField.get(activityRecord);
                }
            }
        }
        catch (ClassNotFoundException e)
        {
            e.printStackTrace();
        }
        catch (InvocationTargetException e)
        {
            e.printStackTrace();
        }
        catch (NoSuchMethodException e)
        {
            e.printStackTrace();
        }
        catch (NoSuchFieldException e)
        {
            e.printStackTrace();
        }
        catch (IllegalAccessException e)
        {
            e.printStackTrace();
        }
        return null;
    }

    public static byte[] getNativeStreamFromAssets(String fileName)
    {
        AssetManager asset = getApp().getAssets();
        byte[] buffer = null;
        AssetFileDescriptor fd = null;
        InputStream input = null;
        try
        {
            fd = asset.openFd(fileName);
            buffer = new byte[(int)fd.getLength()];
            input = asset.open(fileName);
            input.read(buffer, 0, buffer.length);
            input.close();
        }
        catch (FileNotFoundException e)
        {
            e.printStackTrace();
            try
            {
                if (fd != null) {
                    fd.close();
                }
                if (input != null) {
                    input.close();
                }
            }
            catch (IOException ie)
            {
                ie.printStackTrace();
            }
        }
        catch (IOException e)
        {
            e.printStackTrace();
            try
            {
                if (fd != null) {
                    fd.close();
                }
                if (input != null) {
                    input.close();
                }
            }
            catch (IOException ie)
            {
                ie.printStackTrace();
            }
        }
        finally
        {
            try
            {
                if (fd != null) {
                    fd.close();
                }
                if (input != null) {
                    input.close();
                }
            }
            catch (IOException e)
            {
                e.printStackTrace();
            }
        }
        return buffer;
    }

    public static int getFileSize(String fileName)
    {
        AssetManager asset = getApp().getAssets();
        AssetFileDescriptor fd = null;
        long len = 0L;
        try
        {
            fd = asset.openFd(fileName);
            len = fd.getLength();
        }
        catch (IOException localIOException1)
        {
            try
            {
                if (fd != null) {
                    fd.close();
                }
            }
            catch (IOException e)
            {
                e.printStackTrace();
            }
        }
        finally
        {
            try
            {
                if (fd != null) {
                    fd.close();
                }
            }
            catch (IOException e)
            {
                e.printStackTrace();
            }
        }
        return (int)len;
    }

    public static void ExitGame()
    {
        //MainActivity.this.finish();
        android.os.Process.killProcess(android.os.Process.myPid());
        System.exit(0);
    }
}
