using System;
using SuperScrollView;
using UnityEngine;
using UnityEngine.UI;
using YouYou;

public class UICreateRole_JobItemView : MonoBehaviour
{
    private int jobId;
    [SerializeField]
    private YouYouImage youyouIconImage;
    [SerializeField]
    private Text textName;
    [SerializeField]
    private Button btnSelectClick;

    private Action<int> OnSelectJobHandler;
    public void Init()
    {
        btnSelectClick.onClick.AddListener(OnSelectJob);
    }

    private void OnSelectJob()
    {
        OnSelectJobHandler?.Invoke(jobId);
    }

    public void SetItemData(DTJobEntity itemData,Action<int> onSelectJobHandler)
    {
        jobId = itemData.Id;
        textName.text = GameEntry.Localization.GetString(itemData.Name);
        youyouIconImage.LoadImage(itemData.HeadPic);
        OnSelectJobHandler = onSelectJobHandler;
    }
    
    
}