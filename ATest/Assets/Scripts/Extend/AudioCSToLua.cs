using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AudioCSToLua
{
    public static void PlayEntityAudio(long id,string audioName,float volume)
    {
        
    }
    public static void StopEntityAudio(long id)
    {

    }
    public static int PlayAudio(string audioName,float volume,bool isLoop,Action onComplete)
    {
        return AppAudioWidget.Play(audioName, volume, isLoop, onComplete);
    }
    public static void StopAllAudio()
    {
        AppAudioWidget.StopAll();
    }
    public static void StopAudioById(int uid)
    {
        AppAudioWidget.StopById(uid);
    }
    public static void SteBGMVolume(float volume)
    {
        AppAudioWidget.SetBGMVolume(volume);
    }
    public static void PreLoadAudio(string audioName)
    {
        AudioMgr.Load(audioName);
    }
}
