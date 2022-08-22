using System.Collections;
using System.Collections.Generic;
using SuperScrollView;
using UnityEngine;
using UnityEngine.UI;
using YouYou;
using YouYou.Proto;

public class UICreateRole : UIFormBase
{
    [SerializeField] private Button btnClose;
    [SerializeField] private Button btnCreateRole;
    [SerializeField] private InputField inputNickName;
    [SerializeField] private LoopListView2 loopListView;

    private List<DTJobEntity> jobList;
    private int currSelectJobId;
    private List<LoopListViewItem2> loopItemPool;
    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        loopItemPool = new List<LoopListViewItem2>();
        btnCreateRole.onClick.AddListener(CreateRole);
        btnClose.onClick.AddListener(GoToSelectRole);
        loopListView.InitListView(0,OnGetItemByIndex);
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        int roleCount = (int) userData;
        if (roleCount == 0)
        {
            btnClose.gameObject.SetActive(false);
        }

        jobList = GameEntry.DataTable.JobList.GetList();
        currSelectJobId = 0;
        loopListView.SetListItemCount(jobList.Count);
        OnSelectJobHandler(1);
    }

    protected override void OnClose()
    {
        base.OnClose();
    }

    protected override void OnBeforeDestroy()
    {
        base.OnBeforeDestroy();
    }
    
    private void CreateRole()
    {
        C2WS_CreateRole proto = new C2WS_CreateRole();
        proto.JobId = currSelectJobId;
        proto.Sex = 0;
        proto.NickName = inputNickName.text;
        
        GameEntry.Data.UserDataManager.SetCurrJobId(proto.JobId);
        GameEntry.Socket.SendMainMsg(proto);
    }

    private void GoToSelectRole()
    {
        GameEntry.UI.OpenUIForm(global::UIFormId.UI_SelectRole);
        this.Close();
    }

    private void OnSelectJobHandler(int jobId)
    {
        currSelectJobId = jobId;
        VarInt varInt = GameEntry.Pool.DequeueClassObject<VarInt>();
        varInt.Value = currSelectJobId;
        GameEntry.Event.CommonEvent.Dispatch(CommonEventId.OnSelectJobComplete,varInt);
        GameEntry.Pool.EnqueueClassObject(varInt);
    }

    /// <summary>
    /// 初始化滑动列表
    /// </summary>
    /// <param name="listView"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= jobList.Count)
        {
            return null;
        }
    
        DTJobEntity itemData = GetItemDataByIndex(index);
        if (itemData == null)
        {
            return null;
        }
    
        LoopListViewItem2 item = listView.NewListViewItem("ItemPrefab");
        UICreateRole_JobItemView itemScript = item.GetComponent<UICreateRole_JobItemView>();
        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
            itemScript.Init();
        }
    
        itemScript.SetItemData(itemData,OnSelectJobHandler);
        return item;
    }
    
    /// <summary>
    /// 根据所以获取滑动列表里的数据
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private DTJobEntity GetItemDataByIndex(int index)
    {
        if (index < 0 || index >= jobList.Count)
        {
            return null;
        }
    
        return jobList[index];
    }

}