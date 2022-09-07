
//===================================================
//作    者：边涯  http://www.u3dol.com
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using System.Collections;
using System.Collections.Generic;
using System;
using YouYou;

/// <summary>
/// DTSkill数据管理
/// </summary>
public partial class DTSkillDBModel : DataTableDBModelBase<DTSkillDBModel, DTSkillEntity>
{
    /// <summary>
    /// 文件名称
    /// </summary>
    public override string DataTableName { get { return "DTSkill"; } }

    /// <summary>
    /// 加载列表
    /// </summary>
    protected override void LoadList(MMO_MemoryStream ms)
    {
        int rows = ms.ReadInt();
        int columns = ms.ReadInt();

        for (int i = 0; i < rows; i++)
        {
            DTSkillEntity entity = new DTSkillEntity();
            entity.Id = ms.ReadInt();
            entity.SkillName = ms.ReadUTF8String();
            entity.SkillDesc = ms.ReadUTF8String();
            entity.SkillPic = ms.ReadUTF8String();
            entity.LevelLimit = ms.ReadInt();
            entity.IsPassive = ms.ReadInt();

            m_List.Add(entity);
            m_Dic[entity.Id] = entity;
        }
    }
}