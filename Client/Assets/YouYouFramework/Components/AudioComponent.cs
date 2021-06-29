using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    public class AudioComponent : YouYouBaseComponent
    {
        /// <summary>
        /// 声音管理器
        /// </summary>
        public AudioManager m_AudioManager { get; private set; }

        protected override void OnAwake()
        {
            base.OnAwake();
            m_AudioManager = new AudioManager();
        }

        #region BGM

        /// <summary>
        /// 加载Banks
        /// </summary>
        /// <param name="onComplete"></param>
        public void LoadBanks(BaseAction onComplete)
        {
            m_AudioManager.LoadBanks(onComplete);
        }

        /// <summary>
        /// 播放BGM
        /// </summary>
        /// <param name="audioId"></param>
        public void PlayBGM(int audioId)
        {
            Sys_AudioEntity entity = GameEntry.DataTable.DataTableManager.Sys_AudioDBModel.Get(audioId);
            if (entity != null)
            {
                PlayBGM(entity.AssetPath, entity.volume);
            }
            else
            {
                GameEntry.LogError("BGM不存在ID={0}", audioId);
            }
        }

        /// <summary>
        /// 播放BGM
        /// </summary>
        /// <param name="bgmPath">路径</param>
        /// <param name="volume">音量</param>
        public void PlayBGM(string bgmPath, float volume = 1)
        {
            m_AudioManager.PlayBGM(bgmPath, volume);
        }

        /// <summary>
        /// BGM切换参数
        /// </summary>
        /// <param name="switchName"></param>
        /// <param name="value"></param>
        public void BGMSwitch(string switchName, float value)
        {
            m_AudioManager.BGMSwitch(switchName, value);
        }

        /// <summary>
        /// 设置BGM音量
        /// </summary>
        /// <param name="value"></param>
        public void SetBGMVolume(float value)
        {
            m_AudioManager.SetBGMVolume(value);
        }

        /// <summary>
        /// 暂停BGM
        /// </summary>
        /// <param name="pause"></param>
        public void PauseBGM(bool pause)
        {
            m_AudioManager.PauseBGM(pause);
        }

        /// <summary>
        /// 停止BGM
        /// </summary>
        public void StopBGM()
        {
            m_AudioManager.StopBGM();
        }

        /// <summary>
        /// 开始播放BGM
        /// </summary>
        public void StartBGM()
        {
            m_AudioManager.StartBGM();
        }

        #endregion

        public override void Shutdown()
        {
        }
    }
}