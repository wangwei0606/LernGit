using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public interface ISpineAnimationPlugin
{
    void Init(GameObject obj);
    void AddStartHand(Action<string> hand);
    void AddEndHand(Action<string> hand);
    void AddDisposeHand(Action<string> hand);
    void AddCompleteHand(Action<string> hand);
    void SetAnimation(int trackIndex, string animationName, bool loop);
    void AddAnimation(int trackIndex, string animationName, bool loop, float delay);
    void Pause();
    void Proceed();
    void Dispose();
    void Release();
}