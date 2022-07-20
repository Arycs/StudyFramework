using System.Collections;
using System.Collections.Generic;
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
}
