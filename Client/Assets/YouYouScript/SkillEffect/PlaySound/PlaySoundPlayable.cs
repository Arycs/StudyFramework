using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using static MyCommonEnum;

namespace YouYou
{
    public class PlaySoundPlayable : BasePlayableAsset<PlaySoundPlayableBehaviour, PlaySoundEventArgs>
    {
        [Header("目标点")]
        public DynamicTarget Target;
        
        [Header("声音")]
        public string SoundName;
        protected override void OnYouYouCreatePlayable(ScriptPlayable<PlaySoundPlayableBehaviour> playable)
        {
            base.OnYouYouCreatePlayable(playable);
            this.CurrPlayableBehaviour.CurrArgs.Target = Target;
            this.CurrPlayableBehaviour.CurrArgs.SoundName = SoundName;
        }
    }
}