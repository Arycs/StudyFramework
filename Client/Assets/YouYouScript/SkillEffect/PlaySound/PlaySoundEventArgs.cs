using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyCommonEnum;

namespace YouYou
{
    public class PlaySoundEventArgs : IPlayableBehaviourArgs
    {
        [Header("目标点")] public DynamicTarget Target;

        [Header("声音")] public string SoundName;

        public void Reset()
        {
            SoundName = null;
        }
    }
}