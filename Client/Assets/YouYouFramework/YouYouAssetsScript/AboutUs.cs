using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AboutUs : ScriptableObject
{
    [BoxGroup("AboutUs")]
    [HorizontalGroup("AboutUs/Split", 80)]
    [VerticalGroup("AboutUs/Split/Left")]
    [HideLabel,PreviewField(80,ObjectFieldAlignment.Center)]
    public Texture Icon;

    [HorizontalGroup("AboutUs/Split",LabelWidth = 70)]
    [VerticalGroup("AboutUs/Split/Right")]
    [DisplayAsString]
    [LabelText("�������")]
    [GUIColor(2,6,6,1)]
    public string Name = "YouYouFramework";

    //[PropertySpace] ���һ�пո�
    [VerticalGroup("AboutUs/Split/Right")]
    [DisplayAsString]
    [LabelText("�汾��")]
    [GUIColor(2, 6, 6, 1)]
    public string Version = "1.1";

    [VerticalGroup("AboutUs/Split/Right")]
    [DisplayAsString]
    [LabelText("����")]
    [GUIColor(2, 6, 6, 1)]
    public string Author = "Arycs";

    [VerticalGroup("AboutUs/Split/Right")]
    [DisplayAsString]
    [LabelText("��ϵ��ʽ")]
    [GUIColor(2, 6, 6, 1)]
    public string Contact = "QQ : 786859650";


    [BoxGroup("Models")]
    [Title("������",bold:false)]
    [HideLabel]
    [DisplayAsString]
    public string MacroSettings = "�����ã��ܹ��������ȫ������";

    [PropertySpace(10)]
    [BoxGroup("Models")]
    [Title("��������", bold: false)]
    [HideLabel]
    [DisplayAsString]
    public string ParamsSetting = "ȫ�ֲ���";

}
