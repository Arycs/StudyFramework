
//===================================================
//作    者：边涯  http://www.u3dol.com
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using System.Collections;
using System.Collections.Generic;
using System;
using YouYou;

/// <summary>
/// DTPVPSceneMonsterPoint数据管理
/// </summary>
public partial class DTPVPSceneMonsterPointDBModel : DataTableDBModelBase<DTPVPSceneMonsterPointDBModel, DTPVPSceneMonsterPointEntity>
{
    /// <summary>
    /// 文件名称
    /// </summary>
    public override string DataTableName { get { return "DTPVPSceneMonsterPoint"; } }

    /// <summary>
    /// 加载列表
    /// </summary>
    protected override void LoadList(MMO_MemoryStream ms)
    {
        int rows = ms.ReadInt();
        int columns = ms.ReadInt();

        for (int i = 0; i < rows; i++)
        {
            DTPVPSceneMonsterPointEntity entity = new DTPVPSceneMonsterPointEntity();
            entity.Id = ms.ReadInt();
            entity.Desc = ms.ReadUTF8String();
            entity.SceneId = ms.ReadInt();
            entity.MonsterId = ms.ReadInt();
            entity.BornPos_1 = ms.ReadFloat();
            entity.BornPos_2 = ms.ReadFloat();
            entity.BornPos_3 = ms.ReadFloat();
            entity.IsFixTime = ms.ReadBool();
            entity.FixTime_Hour = ms.ReadInt();
            entity.FixTime_Minute = ms.ReadInt();
            entity.Interval = ms.ReadInt();
            entity.PatrolX_1 = ms.ReadFloat();
            entity.PatrolY_1 = ms.ReadFloat();
            entity.PatrolZ_1 = ms.ReadFloat();
            entity.PatrolX_2 = ms.ReadFloat();
            entity.PatrolY_2 = ms.ReadFloat();
            entity.PatrolZ_2 = ms.ReadFloat();
            entity.PatrolX_3 = ms.ReadFloat();
            entity.PatrolY_3 = ms.ReadFloat();
            entity.PatrolZ_3 = ms.ReadFloat();
            entity.PatrolX_4 = ms.ReadFloat();
            entity.PatrolY_4 = ms.ReadFloat();
            entity.PatrolZ_4 = ms.ReadFloat();

            m_List.Add(entity);
            m_Dic[entity.Id] = entity;
        }
    }
}