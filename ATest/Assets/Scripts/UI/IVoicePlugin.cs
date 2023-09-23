using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IVoicePlugin
{
    void SetAutoStart(bool isAuto);
    void SetLoop(bool isLoop);
    void SetDebugGui(bool isDebugGui);
    void SetVideoLocation(int fileLocation);
    void SetVideoPath(string path);
    float GetDurationMs();
    void AddVideoEvent(Action<int, int> onVideoEvent);
    void Play();
    void CloseVideo();
}
