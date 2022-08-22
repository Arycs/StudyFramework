﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.Collections.Generic;
using YouYou.Proto;
using YouYouServer.Core;

namespace YouYouServer.Common
{
    /// <summary>
    /// 角色实体
    /// </summary>
    public class RoleEntity :YFMongoEntityBase
    {
        public RoleEntity()
        {
            TaskList = new List<TaskEntity>();
            SkillDic = new Dictionary<int, SkillEntity>();
        }

        /// <summary>
        /// 账号ID
        /// </summary>
        public long AccountId;

        /// <summary>
        /// 职业编号
        /// </summary>
        public byte JobId;

        /// <summary>
        /// 性别
        /// </summary>
        public byte Sex;

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName;

        /// <summary>
        /// 等级
        /// </summary>
        public int Level;

        /// <summary>
        /// 所在场景编号
        /// </summary>
        public int CurrSceneId;

        /// <summary>
        /// 当前位置
        /// </summary>
        public Vector3 PosData;

        /// <summary>
        /// Y轴旋转
        /// </summary>
        public float RotationY;
        
        /// <summary>
        /// 任务列表
        /// </summary>
        public List<TaskEntity> TaskList;

        /// <summary>
        /// 技能字典, MongoDB 需要对应的类型可序列化,Dictionary不能序列化,但是Bson自带了特性支持Dic
        /// </summary>
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, SkillEntity> SkillDic;
    }
}
