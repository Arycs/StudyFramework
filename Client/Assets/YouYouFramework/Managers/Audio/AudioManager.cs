using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace YouYou
{
    /// <summary>
    /// ����������
    /// </summary>
    public class AudioManager : ManagerBase, IDisposable
    {
        
        public AudioManager()
        {
            m_NextReleaseTime = Time.time;
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        public override void Init()
        {

        }

        /// <summary>
        /// ����Bank
        /// </summary>
        /// <param name="onComplete"></param>
        public void LoadBanks(BaseAction onComplete) {
#if DISABLE_ASSETBUNDLE && UNITY_EDITOR
           string[] arr = Directory.GetFiles(Application.dataPath + "./Download/Audio/", "*.bytes");
            int len = arr.Length;
            for (int i = 0; i < len; i++)
            {
                FileInfo file = new FileInfo(arr[i]);
                TextAsset asset =
                    UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Download/Audio/" + file.Name);
                RuntimeManager.LoadBank(asset);
            }

            if (onComplete != null)
            {
                onComplete();
            }
#else
                    
#endif
        }

        #region BGM

        /// <summary>
        /// ��ǰBGM������
        /// </summary>
        private string m_CurrBGMAudio;

        /// <summary>
        /// ��ǰBGM����
        /// </summary>
        private float m_CurrBGMVolume;

        /// <summary>
        /// ��ǰBGM�������
        /// </summary>
        private float m_CurrBGMMaxVolume;

        /// <summary>
        /// ��ǰBGM��Instance
        /// </summary>
        private EventInstance BGMEvent;
        
        /// <summary>
        /// ��ǰBGM�Ķ�ʱ�� ������������
        /// </summary>
        private TimeAction m_CurrBGMTimeAction;

        /// <summary>
        /// ����BGM
        /// </summary>
        /// <param name="audioId"></param>
        public void PlayBGM(int audioId)
        {
            Sys_AudioEntity entity = GameEntry.DataTable.Sys_AudioDBModel.Get(audioId);
            if (entity != null)
            {
                PlayBGM(entity.AssetPath, entity.volume);
            }
            else
            {
                GameEntry.LogError("BGM������ID={0}", audioId);
            }
        }

        /// <summary>
        /// BGM�л�����
        /// </summary>
        /// <param name="bgmPath"></param>
        /// <param name="volume"></param>
        public void PlayBGM(string bgmPath, float volume = 1)
        {
            m_CurrBGMAudio = bgmPath;
            m_CurrBGMMaxVolume = volume;
            CheckBGMEventInstance();
        }

        /// <summary>
        /// BGM�л�����
        /// </summary>
        /// <param name="switchName"></param>
        /// <param name="value"></param>
        public void BGMSwitch(string switchName, float value)
        {
            BGMEvent.setParameterByName(switchName, value);

        }
        
        /// <summary>
        /// ����BGM����
        /// </summary>
        /// <param name="value"></param>
        public void SetBGMVolume(float value)
        {
            BGMEvent.setVolume(value);
        }

        /// <summary>
        /// BGM��ͣ
        /// </summary>
        /// <param name="pause"></param>
        public void PauseBGM(bool pause)
        {
            if (!BGMEvent.isValid())
            {
                CheckBGMEventInstance();
            }

            if (BGMEvent.isValid())
            {
                BGMEvent.setPaused(pause);
            }
        }

        
        /// <summary>
        /// ���BGMʵ����֮ǰ���ͷŵ�
        /// </summary>
        public void CheckBGMEventInstance()
        {
            if (!string.IsNullOrEmpty(m_CurrBGMAudio))
            {
                BGMEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                BGMEvent.release();
            }

            BGMEvent = RuntimeManager.CreateInstance(m_CurrBGMAudio);

            m_CurrBGMVolume = 0;
            SetBGMVolume(m_CurrBGMVolume);

            BGMEvent.start();
            
            //�������𽥱��Max
            m_CurrBGMTimeAction = GameEntry.Time.CreateTimeAction();
            m_CurrBGMTimeAction.Init(null,0, 0.05f, 100, null, (int loop) =>
            {
                m_CurrBGMVolume += 0.1f;
                m_CurrBGMVolume = Mathf.Min(m_CurrBGMVolume, m_CurrBGMMaxVolume);
                SetBGMVolume(m_CurrBGMVolume);
                if (m_CurrBGMVolume == m_CurrBGMMaxVolume)
                {
                    m_CurrBGMTimeAction.Stop();
                }
            },null).Run();
        }

        /// <summary>
        /// ֹͣ����BGM
        /// </summary>
        public void StopBGM()
        {
            if (BGMEvent.isValid())
            {
                m_CurrBGMTimeAction = GameEntry.Time.CreateTimeAction();
                m_CurrBGMTimeAction.Init(null,0,0.05f,100,null, (int loop) =>
                    {
                        m_CurrBGMVolume -= 0.1f;
                        m_CurrBGMVolume = Mathf.Max(m_CurrBGMVolume, 0);
                        SetBGMVolume(m_CurrBGMVolume);
                        if (m_CurrBGMVolume == 0)
                        {
                            m_CurrBGMTimeAction.Stop();
                            BGMEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                        }
                    },
                    () => { BGMEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);}).Run();
            }
        }

        /// <summary>
        /// ��ʼ����
        /// </summary>
        public void StartBGM()
        {
            BGMEvent.start();
        }

        #endregion

        /// <summary>
        /// OnUpdate
        /// </summary>
        public void OnUpdate()
        {
            if (Time.time > m_NextReleaseTime + m_ReleaseInterval)
            {
                m_NextReleaseTime = Time.time;
                Release();
            }
        }

        #region ��Ч

        /// <summary>
        /// �ͷż��
        /// </summary>
        private int m_ReleaseInterval = 120;

        /// <summary>
        /// �´��ͷ�ʱ��
        /// </summary>
        private float m_NextReleaseTime = 0f;

        /// <summary>
        /// ���
        /// </summary>
        private int m_Serial = 0;

        /// <summary>
        /// ��Ч�ֵ�
        /// </summary>
        private Dictionary<int,EventInstance> m_CurrAudioEventsDic = new Dictionary<int, EventInstance>();
        
        /// <summary>
        /// ��Ҫ�ͷŵ���Ч���
        /// </summary>
        private LinkedList<int> m_NeedRemoveList = new LinkedList<int>();


        /// <summary>
        /// ������Ч
        /// </summary>
        /// <param name="audioId">��ЧID</param>
        /// <param name="parameterName">��������</param>
        /// <param name="value">����ֵ</param>
        /// <param name="pos3D">3D��Чλ��</param>
        /// <returns>��Чʵ�����</returns>
        public int PlayAudio(int audioId, string parameterName = null, float value = 0,
            Vector3 pos3D = default(Vector3))
        {
            Sys_AudioEntity entity = GameEntry.DataTable.Sys_AudioDBModel.Get(audioId);
            if (entity != null)
            {
                return PlayAudio(entity.AssetPath, entity.volume, parameterName, value, entity.Is3D == 1, pos3D);
            }
            else
            {
                GameEntry.LogError("Audio������ID={0}", audioId);
                return -1;
            }
        }

        /// <summary>
        /// ������Ч
        /// </summary>
        /// <param name="eventPath">·��</param>
        /// <param name="volume">����</param>
        /// <param name="parameterName">��������</param>
        /// <param name="value">����ֵ</param>
        /// <param name="is3D">�Ƿ�3D</param>
        /// <param name="pos3D">3Dλ��</param>
        /// <returns></returns>
        public int PlayAudio(string eventPath, float volume = 1, string parameterName = null, float value = 0,
            bool is3D = false, Vector3 pos3D = default(Vector3))
        {
            if (string.IsNullOrEmpty(eventPath))
            {
                return -1;
            }

            EventInstance eventInstance = RuntimeManager.CreateInstance(eventPath);
            eventInstance.setVolume(volume);
            if (!string.IsNullOrEmpty(parameterName))
            {
                eventInstance.setParameterByName(parameterName, value);
            }

            if (is3D)
            {
                eventInstance.set3DAttributes(pos3D.To3DAttributes());
            }

            eventInstance.start();
            int serialId = m_Serial++;
            m_CurrAudioEventsDic[serialId] = eventInstance;
            return serialId;
        }

        /// <summary>
        /// ��ͣĳ����Ч
        /// </summary>
        /// <param name="serialId">��Чʵ�����</param>
        /// <param name="paused">�Ƿ���ͣ</param>
        /// <returns></returns>
        public bool PauseAudio(int serialId, bool paused = true)
        {
            EventInstance eventInstance;
            if (m_CurrAudioEventsDic.TryGetValue(serialId,out eventInstance))
            {
                if (eventInstance.isValid())
                {
                    return eventInstance.setPaused(paused) == FMOD.RESULT.OK;
                }
            }

            return false;
        }

        /// <summary>
        /// ֹͣĳ����Ч
        /// </summary>
        /// <param name="serialId">��Чʵ�����</param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public bool StopAudio(int serialId, FMOD.Studio.STOP_MODE mode = FMOD.Studio.STOP_MODE.IMMEDIATE)
        {
            EventInstance eventInstance;
            if (m_CurrAudioEventsDic.TryGetValue(serialId,out eventInstance))
            {
                if (eventInstance.isValid())
                {
                    var result = eventInstance.stop(mode);
                    eventInstance.release();
                    m_CurrAudioEventsDic.Remove(serialId);
                    return result == FMOD.RESULT.OK;
                }
            }

            return false;
        }

        /// <summary>
        /// �ͷ�
        /// </summary>
        private void Release()
        {
            var lst = m_CurrAudioEventsDic.GetEnumerator();
            while (lst.MoveNext())
            {
                EventInstance eventInstance = lst.Current.Value;
                if (!eventInstance.isValid())
                {
                    continue;;
                }

                PLAYBACK_STATE state;
                eventInstance.getPlaybackState(out state);
                if (state == PLAYBACK_STATE.STOPPED)
                {
                    m_NeedRemoveList.AddLast(lst.Current.Key);
                    eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    eventInstance.release();
                }
            }
            
            //
            LinkedListNode<int> currNode = m_NeedRemoveList.First;
            while (currNode != null)
            {
                LinkedListNode<int> next = currNode.Next;
                int serialId = currNode.Value;
                m_CurrAudioEventsDic.Remove(serialId);
                m_NeedRemoveList.Remove(currNode);
                currNode = next;
            }
        }

        public void Dispose()
        {
            
        }

        #endregion
    }
}
