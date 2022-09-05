using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace YouYou
{
    public class PlayAnimPlayableBehaviour : BasePlayableBehaviour<PlayAnimEventArgs>
    {
        protected override void OnYouYouBehaviourPlay(Playable playable, FrameData info)
        {
            CurrTimeLineCtrl.PlayAnim(CurrArgs);
        }

        protected override void OnYouYouBehaviourStop(Playable playable, FrameData info)
        {
            
        }
    }
}