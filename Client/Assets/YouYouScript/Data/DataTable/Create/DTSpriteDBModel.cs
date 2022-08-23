
//===================================================
//作    者：边涯  http://www.u3dol.com
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using System.Collections;
using System.Collections.Generic;
using System;
using YouYou;

/// <summary>
/// DTSprite数据管理
/// </summary>
public partial class DTSpriteDBModel : DataTableDBModelBase<DTSpriteDBModel, DTSpriteEntity>
{
    /// <summary>
    /// 文件名称
    /// </summary>
    public override string DataTableName { get { return "DTSprite"; } }

    /// <summary>
    /// 加载列表
    /// </summary>
    protected override void LoadList(MMO_MemoryStream ms)
    {
        int rows = ms.ReadInt();
        int columns = ms.ReadInt();

        for (int i = 0; i < rows; i++)
        {
            DTSpriteEntity entity = new DTSpriteEntity();
            entity.Id = ms.ReadInt();
            entity.SpriteType = ms.ReadInt();
            entity.Name = ms.ReadUTF8String();
            entity.Level = ms.ReadInt();
            entity.IsBoss = ms.ReadInt();
            entity.PrefabId = ms.ReadInt();
            entity.AnimGroupId = ms.ReadInt();
            entity.HP = ms.ReadInt();
            entity.MP = ms.ReadInt();
            entity.atk = ms.ReadInt();
            entity.def = ms.ReadInt();
            entity.criticalRate = ms.ReadInt();
            entity.criticalResRate = ms.ReadInt();
            entity.criticalStrengthRate = ms.ReadInt();
            entity.blockRate = ms.ReadInt();
            entity.blockResRate = ms.ReadInt();
            entity.blockStrengthRate = ms.ReadInt();
            entity.injureRate = ms.ReadInt();
            entity.injureResRate = ms.ReadInt();
            entity.eXSkillInjureRate = ms.ReadInt();
            entity.eXSkillInjureResRate = ms.ReadInt();
            entity.IgnoreDefRate = ms.ReadInt();

            m_List.Add(entity);
            m_Dic[entity.Id] = entity;
        }
    }
}