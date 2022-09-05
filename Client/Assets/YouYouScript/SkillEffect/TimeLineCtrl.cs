using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace YouYou
{
    /// <summary>
    /// 技能控制器
    /// </summary>
    public class TimeLineCtrl : MonoBehaviour
    {
        private PlayableDirector m_CurrPlayableDirector;

        /// <summary>
        /// 当前释放技能的角色
        /// </summary>
        public RoleCtrl CurrRole;

        /// <summary>
        /// 停止播放委托
        /// </summary>
        public BaseAction OnStopped;

        private LinkedList<Transform> m_GameObjectList;

        /// <summary>
        /// 攻击结束时间
        /// </summary>
        public float AttackEndTime { get; private set; }

        private void Awake()
        {
            m_GameObjectList = new LinkedList<Transform>();
            m_CurrPlayableDirector = GetComponent<PlayableDirector>();
            m_CurrPlayableDirector.stopped += OnPlayableDirectorStopped;
        }

        private void OnPlayableDirectorStopped(PlayableDirector playableDirector)
        {
            ClearGameObject();
            OnStopped ?.Invoke();
        }

        private void OnDestroy()
        {
            m_CurrPlayableDirector.stopped -= OnPlayableDirectorStopped;
            m_CurrPlayableDirector = null;
        }

        private void OnEnable()
        {
            var lst = m_CurrPlayableDirector.playableAsset.outputs.GetEnumerator();
            while (lst.MoveNext())
            {
                var data = lst.Current;
                if (data.sourceObject != null)
                {
                    var trackAsset = data.sourceObject as TrackAsset;
                    var trackList = trackAsset.GetClips().GetEnumerator();
                    if (trackAsset is PlaySoundTrack)
                    {
                        while (trackList.MoveNext())
                        {
                            TimelineClip clip = trackList.Current;
                            PlaySoundPlayable playableAsset = clip.asset as PlaySoundPlayable;

                            if (playableAsset != null && playableAsset.CurrPlayableBehaviour != null)
                            {
                                playableAsset.CurrPlayableBehaviour.CurrTimeLineCtrl = this;
                                playableAsset.CurrPlayableBehaviour.Start = clip.start;
                                playableAsset.CurrPlayableBehaviour.End = clip.end;
                                playableAsset.CurrPlayableBehaviour.Reset();
                            }
                        }
                    }
                    else if (trackAsset is PlayAnimTrack)
                    {
                        while (trackList.MoveNext())
                        {
                            TimelineClip clip = trackList.Current;
                            PlayAnimPlayable playableAsset = clip.asset as PlayAnimPlayable;

                            if (playableAsset != null && playableAsset.CurrPlayableBehaviour != null)
                            {
                                playableAsset.CurrPlayableBehaviour.CurrTimeLineCtrl = this;
                                playableAsset.CurrPlayableBehaviour.Start = clip.start;
                                playableAsset.CurrPlayableBehaviour.End = clip.end;
                                playableAsset.CurrPlayableBehaviour.Reset();

                                AttackEndTime = (float) clip.end;
                            }
                        }
                    }
                    else if (trackAsset is PlayResourceTrack)
                    {
                        while (trackList.MoveNext())
                        {
                            TimelineClip clip = trackList.Current;
                            PlayResourcePlayable playableAsset = clip.asset as PlayResourcePlayable;

                            if (playableAsset != null && playableAsset.CurrPlayableBehaviour != null)
                            {
                                playableAsset.CurrPlayableBehaviour.CurrTimeLineCtrl = this;
                                playableAsset.CurrPlayableBehaviour.Start = clip.start;
                                playableAsset.CurrPlayableBehaviour.End = clip.end;
                                playableAsset.CurrPlayableBehaviour.Reset();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="args"></param>
        public void PlayaSound(PlaySoundEventArgs args)
        {
            if (args.Target == MyCommonEnum.DynamicTarget.OurOne)
            {
                GameEntry.Audio.PlayAudio(args.SoundName, volume: 1, parameterName: null
                    , is3D: true, pos3D: CurrRole.transform.position);
            }
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="args"></param>
         public void PlayAnim(PlayAnimEventArgs args)
        {
            if (args.Target == MyCommonEnum.DynamicTarget.OurOne)
            {
                CurrRole.PlayAnimByAnimCategory(args.Category, args.Param);
            }
        }

        public void PlayResource(PlayResourceEventArgs args)
        {
            if (args.Target == MyCommonEnum.DynamicTarget.OurOne)
            {
                GameEntry.Pool.GameObjectSpawn(args.PrefabPath, onComplete: (Transform trans, bool isNewInstance) =>
                {
                    //设置到角色身下
                    trans.SetParent(CurrRole.transform);
                    trans.localPosition = args.Offset;
                    trans.localEulerAngles = args.Rotation;
                    trans.localScale = args.Scale;

                    m_GameObjectList.AddLast(trans);
                });
            }
        }
        
        private void ClearGameObject()
        {
            LinkedListNode<Transform> node = m_GameObjectList.First;
            while (node != null)
            {
                LinkedListNode<Transform> next = node.Next;

                GameEntry.Pool.GameObjectDeSpawn(node.Value);
                m_GameObjectList.Remove(node);

                node = next;
            }
        }
    }
}