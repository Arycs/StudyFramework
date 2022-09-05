using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace YouYou
{
    /// <summary>
    /// Playable视图基类
    /// </summary>
    public class BasePlayableAsset<T,TP> :PlayableAsset ,ITimelineClipAsset where T : BasePlayableBehaviour<TP>, new() where TP: class,IPlayableBehaviourArgs,new()
    {
        /// <summary>
        /// 被允许的功能将可以再Inspector被编辑, 这些功能都是与Clip相关的操作
        /// </summary>
        public ClipCaps clipCaps => ClipCaps.Blending;
            
        /// <summary>
        /// 当前视图对应的控制器
        /// </summary>
        public T CurrPlayableBehaviour;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<T>.Create(graph);
            if (Application.isPlaying)
            {
                CurrPlayableBehaviour = playable.GetBehaviour();
                if (CurrPlayableBehaviour != null)
                {
                    //这里进行参数数据取池
                    CurrPlayableBehaviour.CurrArgs = GameEntry.Pool.DequeueClassObject<TP>();
                    OnYouYouCreatePlayable(playable);
                }
            }

            return playable;
        }

        protected virtual void OnYouYouCreatePlayable(ScriptPlayable<T> playable)
        {
            
        }

    }
}