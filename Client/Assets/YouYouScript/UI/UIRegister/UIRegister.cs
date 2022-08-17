using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using YouYou;

public class UIRegister : UIFormBase
{
    [SerializeField]
    private Button btnReg;

    [SerializeField]
    private Button btnLogin;

    [SerializeField]
    private InputField inputUserName;

    [SerializeField]
    private InputField inputPassword;
    
    
    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        btnReg.onClick.AddListener(OnBtnRegClick);
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
        btnReg = null;
        btnLogin = null;
        inputUserName = null;
        inputPassword = null;
    }

    private void OnBtnRegClick()
    {
        string url = GameEntry.Http.RealWebAccountUrl + "/account";
        Dictionary<string, object> dic = GameEntry.Pool.DequeueClassObject<Dictionary<string, object>>();
        dic.Clear();
        dic["ChannelId"] = GameEntry.Data.SysDataManager.CurrChannelConfig.ChannelId;
        dic["InnerVersion"] = GameEntry.Data.SysDataManager.CurrChannelConfig.InnerVersion;
        dic["Type"] = 0;
        dic["UserName"] = inputUserName.text;
        dic["Password"] = inputPassword.text;
        GameEntry.Http.SendData(url,OnRegCallBack,true,false,dic);
    }

    private void OnRegCallBack(HttpCallBackArgs args)
    {
        if (!args.HasError)
        {
            RetValue retValue = LitJson.JsonMapper.ToObject<RetValue>(args.Value);
            if (!retValue.HasError)
            {
                JsonData data = LitJson.JsonMapper.ToObject(args.Value);
                JsonData config = LitJson.JsonMapper.ToObject(data["Value"].ToString());

                var UserName = config["UserName"];
                var Password = config["Password"];
                
                Debug.Log("用户名 : " + UserName + " ==== 密码 :" + Password);
                
                this.Close();
                GameEntry.Procedure.ChangeState(ProcedureState.SelectRole);
            }
            else
            {
                GameEntry.UI.OpenDialogFormBySysCode(retValue.ErrorCode);
                Debug.Log("错误代码 : " + retValue.ErrorCode);
            }
        }
        else
        {
            GameEntry.LogError("注册 超时 !");
        }
    }
    
    private void OnLoginClick()
    {
        this.Close();
        GameEntry.UI.OpenUIForm(global::UIFormId.UI_Login);
    }
}
