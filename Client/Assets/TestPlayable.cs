using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class TestPlayable : MonoBehaviour
{
    private Animator m_Animator;

    [SerializeField] private AnimationClip m_AnimationClip;

    private PlayableGraph m_PlayableGraph;

    /// <summary>
    /// 剪辑片段数组
    /// </summary>
    [SerializeField] private AnimationClip[] m_Clips;

    private AnimationPlayableOutput m_AnimationPlayableOutput;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        //第一步, 创建画布
        m_PlayableGraph = PlayableGraph.Create("testGraph");
        //第二步, 创建输出节点
        m_AnimationPlayableOutput = AnimationPlayableOutput.Create(m_PlayableGraph, "output", m_Animator);

        //Test1();
        // Test2();
    }

    private void Test1()
    {
        AnimationClipPlayable animationClipPlayable = AnimationClipPlayable.Create(m_PlayableGraph, m_AnimationClip);

        //连线
        m_AnimationPlayableOutput.SetSourcePlayable(animationClipPlayable, 0);
        m_PlayableGraph.Play();
    }

    /// <summary>
    /// 动画剪辑Playable列表
    /// </summary>
    private List<AnimationClipPlayable> lst = new List<AnimationClipPlayable>(100);

    /// <summary>
    /// 动画混合 Playable
    /// </summary>
    private AnimationMixerPlayable m_AnimationMixerPlayable;

    private void Test2()
    {
        int clipCount = m_Clips.Length;

        //创建动画混合Playable
        m_AnimationMixerPlayable = AnimationMixerPlayable.Create(m_PlayableGraph, clipCount);

        // 设置Output的源
        m_AnimationPlayableOutput.SetSourcePlayable(m_AnimationMixerPlayable, 0);

        for (int i = 0; i < clipCount; i++)
        {
            //创建AnimationClipPlayable
            AnimationClipPlayable animationClipPlayable = AnimationClipPlayable.Create(m_PlayableGraph, m_Clips[i]);

            //连接到动画混合Playable
            m_PlayableGraph.Connect(animationClipPlayable, 0, m_AnimationMixerPlayable, i);

            //把AnimationClipPlayable加入列表
            lst.Add(animationClipPlayable);
        }
    }

    private int index = 0;

    private void Play()
    {
        if (lst.Count == 0)
        {
            return;
        }
        
        int len = lst.Count;
        if (index > len)
        {
            index = 0;
        }
        
        m_PlayableGraph.Play();
        //根据索引拿到Playable
        Playable playable = m_AnimationMixerPlayable.GetInput(index);
        playable.SetTime(0);
        playable.Play();

        
        for (int i = 0; i < len; i++)
        {
            if (i == index)
            {
                //需要播放的权重设置为1
                m_AnimationMixerPlayable.SetInputWeight(i, 1);
            }
            else
            {
                //需要播放的权重设置为0
                m_AnimationMixerPlayable.SetInputWeight(i, 0);
            }
        }
        index++;
    }

    private void CreateMixerPlayable()
    {
        //创建动画混合Playable
        m_AnimationMixerPlayable = AnimationMixerPlayable.Create(m_PlayableGraph,100);
        
        //设置Output的源
        m_AnimationPlayableOutput .SetSourcePlayable(m_AnimationMixerPlayable,0);
        
        //这时候是没有任何动画的
    }

    private void Start()
    {
        CreateMixerPlayable();
    }


    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            AnimationClip animationClip = m_Clips[lst.Count];
            
            //创建AnimationClipPlayable
            AnimationClipPlayable animationClipPlayable = AnimationClipPlayable.Create(m_PlayableGraph,animationClip);
            
            //连接到MixerPlayable
            m_PlayableGraph.Connect(animationClipPlayable, 0, m_AnimationMixerPlayable, lst.Count);
            
            //一定要把权重设置为0
            m_AnimationMixerPlayable.SetInputWeight(lst.Count,0);
            
            //把AnimationClipPlayable加入列表
            lst.Add(animationClipPlayable);
        }else if (Input.GetKeyUp(KeyCode.B))
        {
            if (lst.Count > 0)
            {
                AnimationClipPlayable animationClipPlayable = lst[lst.Count - 1];
                lst.Remove(animationClipPlayable);
                animationClipPlayable.Destroy();
            }
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            Play();
        }
    }
}