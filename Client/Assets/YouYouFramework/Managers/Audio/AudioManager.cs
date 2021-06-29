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
    public class AudioManager : ManagerBase
    {
        /// <summary>
        /// ����Bank
        /// </summary>
        /// <param name="onComplete"></param>
        public void LoadBanks(BaseAction onComplete) {
#if DISABLE_ASSETBUNDLE
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
            m_CurrBGMTimeAction.Init(0, 0.05f, 100, null, (int loop) =>
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
                m_CurrBGMTimeAction.Init(0,0.05f,100,null, (int loop) =>
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
    }
}
