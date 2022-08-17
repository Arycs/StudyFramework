using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YouYou;
using YouYou.Proto;

public class UISelectRole_RoleItemView : MonoBehaviour
{
    private long roleId;
    [SerializeField]
    private YouYouImage youyouIconImage;
    [SerializeField]
    private Text txtName;
    [SerializeField]
    private Button btnClick;
    
    private Action<long> OnSelectRoleHandler;
    public void Init()
    {
        btnClick.onClick.AddListener(OnSelectRole);
    }

    private void OnSelectRole()
    {
        OnSelectRoleHandler?.Invoke(roleId);
    }

    public void SetItemData(WS2C_ReturnRoleList.Types.WS2C_ReturnRoleList_Item data,Action<long> onSelectRoleHandler )
    {
        roleId = data.RoleId;
        txtName.text = data.NickName;
        //根据职业获取头像
        var rowJob = GameEntry.DataTable.JobList.Get(data.JobId);
        youyouIconImage.LoadImage(rowJob.HeadPic);
        OnSelectRoleHandler = onSelectRoleHandler;
    }
}
