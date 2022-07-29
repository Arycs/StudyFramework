using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleCtrl : MonoBehaviour
{
    /// <summary>
    /// 皮肤编号
    /// </summary>
    private int m_SkinId = 0;
    
    public void Init(int skinId)
    {
        m_SkinId = skinId;
    }
}
