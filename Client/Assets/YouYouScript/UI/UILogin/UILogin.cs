using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using YouYou;

public class UILogin : UIFormBase
{
    [SerializeField]
    private Button btnLogin;

    [SerializeField]
    private Button btnReg;

    [SerializeField]
    private InputField inputUserName;

    [SerializeField]
    private InputField inputPassword;
    
    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        btnReg.onClick.AddListener(OnRegClick);
        btnLogin.onClick.AddListener(OnLoginClick);
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
    }

    protected override void OnClose()
    {
        base.OnClose();
    }

    protected override void OnBeforeDestroy()
    {
        base.OnBeforeDestroy();
    }

    private void OnRegClick()
    {
        this.Close();
        GameEntry.UI.OpenUIForm(102);
    }

    private void OnLoginClick()
    {
        string url = GameEntry.Http.RealWebAccountUrl + "/account";
        Dictionary<string, object> dic = GameEntry.Pool.DequeueClassObject<Dictionary<string, object>>();
        dic.Clear();
        dic["ChannelId"] = GameEntry.Data.SysDataManager.CurrChannelConfig.ChannelId;
        dic["InnerVersion"] = GameEntry.Data.SysDataManager.CurrChannelConfig.InnerVersion;
        dic["Type"] = 1;
        dic["UserName"] = inputUserName.text;
        dic["Password"] = inputPassword.text;
        GameEntry.Http.SendData(url,OnLoginCallBack,true,false,dic);
    }

    private void OnLoginCallBack(HttpCallBackArgs args)
    {
        if (!args.HasError)
        {
            RetValue retValue = LitJson.JsonMapper.ToObject<RetValue>(args.Value);
            if (!retValue.HasError)
            {
                JsonData data = LitJson.JsonMapper.ToObject(args.Value);
                JsonData config = LitJson.JsonMapper.ToObject(data["Value"].ToString());
                GameEntry.Data.UserDataManager.AccountId = config["YFId"].ToString().ToLong();

                Debug.Log("用户名 : " + GameEntry.Data.UserDataManager.AccountId );
                this.Close();
                GameEntry.Procedure.ChangeState(ProcedureState.SelectRole);
            }
        }
        else
        {
            GameEntry.LogError("注册 超时 !");
        }
    }
}
