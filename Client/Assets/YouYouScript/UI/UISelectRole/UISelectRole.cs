using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using SuperScrollView;
using UnityEngine;
using UnityEngine.UI;
using YouYou;
using YouYou.Proto;

public class UISelectRole : UIFormBase
{
    [SerializeField] private Button btnEnterGame;
    [SerializeField] private Button btnCreateRole;
    [SerializeField] private LoopListView2 loopListView;

    private RepeatedField<WS2C_ReturnRoleList.Types.WS2C_ReturnRoleList_Item> roleList;
    private long currSelectRoleId;
    private int currSelectJobId;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        btnEnterGame.onClick.AddListener(EnterGame);
        btnCreateRole.onClick.AddListener(GoToCreateRole);
        loopListView.InitListView(0, OnGetItemByIndex);
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        roleList = GameEntry.Data.UserDataManager.ReturnRoleListData.RoleList;
        
        currSelectRoleId = 0;
        currSelectJobId = 0;
        loopListView.SetListItemCount(roleList.Count);
        OnSelectRoleHandler(roleList[0].RoleId);
    }


    protected override void OnClose()
    {
        base.OnClose();
    }

    protected override void OnBeforeDestroy()
    {
        base.OnBeforeDestroy();
    }

    private void OnSelectRoleHandler(long roleId)
    {
        if (currSelectRoleId == roleId)
        {
            return;
        }

        currSelectRoleId = roleId;
        foreach (var item in roleList)
        {
            if (item.RoleId != currSelectRoleId) continue;
            currSelectJobId = item.JobId;
            break;
        }
        VarInt varInt = GameEntry.Pool.DequeueClassObject<VarInt>();
        varInt.Value = currSelectJobId;
        GameEntry.Event.CommonEvent.Dispatch(CommonEventId.OnSelectJobComplete,varInt);
    }
    
    
    private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= roleList.Count)
        {
            return null;
        }

        var itemData = GetItemDataByIndex(index);
        if (itemData == null)
        {
            return null;
        }

        var item = listView.NewListViewItem("ItemPrefab");
        var itemScript = item.GetComponent<UISelectRole_RoleItemView>();
        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
            itemScript.Init();
        }
        itemScript.SetItemData(itemData,OnSelectRoleHandler);
        return item;
    }

    /// <summary>
    /// 根据所以获取滑动列表里的数据
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private WS2C_ReturnRoleList.Types.WS2C_ReturnRoleList_Item GetItemDataByIndex(int index)
    {
        if (index < 0 || index >= roleList.Count)
        {
            return null;
        }
    
        return roleList[index];
    }
    
    /// <summary>
    /// 进入创建角色界面
    /// </summary>
    private void GoToCreateRole()
    {
        GameEntry.UI.OpenUIForm(SysUIFormId.UI_CreateRole, 1);
        Close();
    }

    /// <summary>
    /// 进入游戏
    /// </summary>
    private void EnterGame()
    {
        GameEntry.Data.UserDataManager.SetCurrJobId(currSelectJobId);
        GameEntry.Data.UserDataManager.SetCurrRoleId(currSelectRoleId);
        GameEntry.Procedure.ChangeState(ProcedureState.EnterGame);
        Close();
    }
}