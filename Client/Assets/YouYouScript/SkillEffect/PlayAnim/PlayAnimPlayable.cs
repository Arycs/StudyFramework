using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using static MyCommonEnum;

namespace YouYou
{
    public class PlayAnimPlayable : BasePlayableAsset<PlayAnimPlayableBehaviour, PlayAnimEventArgs>
    {
        [Header("目标点")] public DynamicTarget Target;

        [Header("动画类型")] public RoleAnimCategory Category;

        [Header("动画参数")] public int Param = 0;

        protected override void OnYouYouCreatePlayable(ScriptPlayable<PlayAnimPlayableBehaviour> playable)
        {
            base.OnYouYouCreatePlayable(playable);
            CurrPlayableBehaviour.CurrArgs.Target = Target;
            CurrPlayableBehaviour.CurrArgs.Category = Category;
            CurrPlayableBehaviour.CurrArgs.Param = Param;
        }
    }
}