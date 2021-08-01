using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Core.Logger;
using YouYouServer.Model.DBModels;
using YouYouServer.Model.Entitys;
using YouYouServer.Model.Logic.DBModels;
using YouYouServer.Model.Logic.Entitys;
using static YouYouServer.Common.ConstDefine;

namespace YouYouServer.Model.Test
{
    public class TestMongoDB
    {
        /// <summary>
        /// 测试添加
        /// </summary>
        public static void TestAdd()
        {
            RoleDBModel roleDBModel = new RoleDBModel();
            UniqueIDGameServer uniqueIDGameServer = new UniqueIDGameServer();
            for (int i = 0; i < 8890; i++)
            {
                RoleEntity roleEntity = new RoleEntity();
                roleEntity.YFId = uniqueIDGameServer.GetUniqueID((int)CollectionType.Role);
                roleEntity.NickName = "傲然于尘世丶" + new Random().Next(1, 888);
                roleEntity.Level = 1;
                roleEntity.CreateTime = DateTime.Now;
                roleEntity.UpdateTime = DateTime.Now;

                roleEntity.TaskList.Add(new TaskEntity() { TaskId = 1, CurrStatus = 0 });
                roleEntity.TaskList.Add(new TaskEntity() { TaskId = 2, CurrStatus = 1 });

                roleEntity.SkillDic[1] = new SkillEntity() { SkillId = 1, CurrLevel = 2 };
                roleEntity.SkillDic[2] = new SkillEntity() { SkillId = 2, CurrLevel = 1 };
                roleEntity.SkillDic[3] = new SkillEntity() { SkillId = 3, CurrLevel = 1 };

                roleDBModel.Add(roleEntity);

                LoggerMgr.Log(Core.LoggerLevel.Log, 0, "Add Role YFID = {0}", roleEntity.YFId);
            }
            Console.WriteLine("添加完毕");
        }

        /// <summary>
        /// 测试查找
        /// </summary>
        public static void TestSearch()
        {
            RoleDBModel roleDBModel = new RoleDBModel();
            // 等于条件的 查询  t=>t.YFId , 1000  
            //FilterDefinition<RoleEntity> filter =  Builders<RoleEntity>.Filter.Eq(t=>t.YFId ,25);
            //// 多语句 And 查询语句
            //filter = Builders<RoleEntity>.Filter.And(
            //        Builders<RoleEntity>.Filter.Eq(t => t.YFId, 100),
            //        Builders<RoleEntity>.Filter.Eq(t => t.NickName, "傲然于尘世丶703")
            //    );
            //// 多语句 Or 查询
            //filter = Builders<RoleEntity>.Filter.Or(
            //        Builders<RoleEntity>.Filter.Eq(t => t.YFId, 88),
            //        Builders<RoleEntity>.Filter.Eq(t => t.NickName, "傲然于尘世丶703"),
            //        Builders<RoleEntity>.Filter.Eq(t => t.YFId, 83),
            //        Builders<RoleEntity>.Filter.Eq(t => t.YFId, 81),
            //        Builders<RoleEntity>.Filter.Eq(t => t.YFId, 55)
            //    );

            RoleEntity roleEntity = roleDBModel.GetEntity(25);
            if (roleEntity != null)
            {
                SkillEntity skillEntity = null;
                if (roleEntity.SkillDic.TryGetValue(1, out skillEntity))
                {
                    Console.WriteLine("角色技能等级: " + roleEntity.SkillDic[1].CurrLevel);
                }
                else
                {
                 
                    Console.WriteLine("该角色没有技能");
                    roleEntity.SkillDic[1] = new SkillEntity() { SkillId = 1, CurrLevel = 1 };
                    roleDBModel.Update(roleEntity);
                }
            }

            // Ascending 升序  Descending降序  继续在后面.Ascending(XX) 先以第一个升序排序,然后在一第二个属性升序排列
            //SortDefinition<RoleEntity> sort = Builders<RoleEntity>.Sort.Ascending(t => t.YFId);

            //long count;
            //List <RoleEntity> lst = roleDBModel.GetListByPage(filter,10,2,out count,field:new string[] {"YFId","NickName"}, sort);
            //Console.WriteLine("数量=" + count);
            //for (int i = 0; i < lst.Count; i++)
            //{
            //    Console.WriteLine("角色ID: "+ lst[i].YFId +"角色名: " + lst[i].NickName);
            //}

        }


        public static void TestUniqueID()
        {
            // UniqueIDGameServer uniqueIDGameServer = new UniqueIDGameServer();
            //for (int i = 0; i < 82; i++)
            //{
            //    long roleId = uniqueIDGameServer.GetUniqueID((int)CollectionType.Role);
            //    Console.WriteLine("角色编号=" + roleId);
            //}

        }
    }
}
