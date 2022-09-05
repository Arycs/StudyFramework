using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
using YouYou;
using static MyCommonEnum;

namespace YouYou
{
    public class PlayResourcePlayable : BasePlayableAsset<PlayResourcePlayableBehaviour, PlayResourceEventArgs>
    {
#if UNITY_EDITOR
        [OnValueChanged("OnCurrResourceChanged")]
        public GameObject CurrResource;
#endif

#if UNITY_EDITOR
        private void OnCurrResourceChanged()
        {
            string path = UnityEditor.AssetDatabase.GetAssetPath(CurrResource);
            PrefabPath = path;
        }
#endif

        [Header("Ŀ���")]
        public DynamicTarget Target;

        [Header("Ԥ��·��")]
        public string PrefabPath;

        [Header("��ת")]
        public Vector3 Rotation;

        [Header("����")]
        public Vector3 Scale = Vector3.one;

        [Header("ƫ��")]
        public Vector3 Offset;

        protected override void OnYouYouCreatePlayable(ScriptPlayable<PlayResourcePlayableBehaviour> playable)
        {
            base.OnYouYouCreatePlayable(playable);

            CurrPlayableBehaviour.CurrArgs.Target = Target;
            CurrPlayableBehaviour.CurrArgs.PrefabPath = PrefabPath;
            CurrPlayableBehaviour.CurrArgs.Rotation = Rotation;
            CurrPlayableBehaviour.CurrArgs.Scale = Scale;
            CurrPlayableBehaviour.CurrArgs.Offset = Offset;
        }
    }
}