using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using YouYou;

public class SelectRoleSceneCtrl : MonoBehaviour
{
    /// <summary>
    /// 当前场景相机
    /// </summary>
    [SerializeField]
    private UniversalAdditionalCameraData m_CameraData;

    [SerializeField]
    private Material m_SkyboxMaterial;
    
    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox = m_SkyboxMaterial;

        GameEntry.CameraCtrl.Close();
        m_CameraData.cameraStack.Add(GameEntry.CameraCtrl.UICamera);
    }
}
