using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using YouYou;

namespace YouYou
{
    public class PlaySoundPlayableBehaviour : BasePlayableBehaviour<PlaySoundEventArgs>
    {
        protected override void OnYouYouBehaviourPlay(Playable playable, FrameData info)
        {
            Debug.LogError("PlaySoundPlayableBehaviour Play");
            CurrTimeLineCtrl.PlayaSound(CurrArgs);
        }

        protected override void OnYouYouBehaviourStop(Playable playable, FrameData info)
        {
            Debug.LogError("PlaySoundPlayableBehaviour Stop");
        }
    }
}