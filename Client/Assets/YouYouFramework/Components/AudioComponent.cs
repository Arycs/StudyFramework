using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    public class AudioComponent : YouYouBaseComponent,IUpdateComponent
    {
        [Header("释放间隔")]
        [SerializeField]
        private int m_ReleaseInterval = 120;
        
        /// <summary>
        /// 声音管理器
        /// </summary>
        public AudioManager m_AudioManager { get; private set; }

        protected override void OnAwake()
        {
            base.OnAwake();
            m_AudioManager = new AudioManager(m_ReleaseInterval);
            GameEntry.RegisterUpdateComponent(this);
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

        #region 音效

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioId">音效ID</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="pos3D">3D音效位置</param>
        /// <returns>音效实例编号</returns>
        public int PlayAudio(int audioId, string parameterName = null, float value = 0,
            Vector3 pos3D = default(Vector3))
        {
            Sys_AudioEntity entity = GameEntry.DataTable.DataTableManager.Sys_AudioDBModel.Get(audioId);
            if (entity != null)
            {
                return PlayAudio(entity.AssetPath, entity.volume, parameterName, value, entity.Is3D == 1, pos3D);
            }
            else
            {
                GameEntry.LogError("Audio不存在ID={0}",audioId);
                return -1;
            }
        }

        public int PlayAudio(string eventPath, float volume = 1, string parameterName = null, float value = 0,
            bool is3D = false, Vector3 pos3D = default(Vector3))
        {
            return m_AudioManager.PlayAudio(eventPath, value, parameterName, value, is3D, pos3D);
        }

        /// <summary>
        /// 暂停某个音效
        /// </summary>
        /// <param name="serialId">音效实例编号</param>
        /// <param name="paused">是否暂停</param>
        /// <returns></returns>
        public bool PauseAudio(int serialId, bool paused = true)
        {
            return m_AudioManager.PauseAudio(serialId, paused);
        }

        /// <summary>
        /// 停止某个音效
        /// </summary>
        /// <param name="serialId">音效实例编号</param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public bool StopAudio(int serialId, FMOD.Studio.STOP_MODE mode = FMOD.Studio.STOP_MODE.IMMEDIATE)
        {
            return m_AudioManager.StopAudio(serialId, mode);
        }
        #endregion
        
        public override void Shutdown()
        {
            GameEntry.RemoveUpdateComponent(this);
        }

        public void OnUpdate()
        {
            m_AudioManager.OnUpdate();
        }
    }
}