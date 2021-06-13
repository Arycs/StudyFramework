using System;
using System.Collections.Generic;
using System.Text;

namespace YouYouServer.Model.Test
{
    public class TestRedis
    {
        /// <summary>
        /// 测试字符串
        /// </summary>
        public static void TestString()
        {
            //RedisHelper.Set("Name", "Arycs");

            //string str = RedisHelper.Get("Name");
            // Cache 类似于一个判断, 去取 某个键, 然后如果获得对应值,则返回,反之则写入. 第一个参数为键, 第二个参数为过期时间,第三个参数为写入的内容 ,通常取MongoDB数据库中进行选择然后写入 
            string str = RedisHelper.CacheShell("Name2", -1, () => { return "Arycs3"; });
            Console.WriteLine("得到了一个Key的值:" + str);
            Console.WriteLine("写入了一个Key");
        }

        /// <summary>
        /// 测试 hash
        /// </summary>
        public static void TestHash()
        {
            string key = "RoleHash";
            //RedisHelper.HSet(key, "1", "Arycs1");
            //RedisHelper.HSet(key, "2", "Arycs2");
            //RedisHelper.HSet(key, "3", "Arycs3");
            //RedisHelper.HSet(key, "4", "Arycs4");
            //RedisHelper.HSet(key, "5", "Arycs5");

            //string str = RedisHelper.HGet(key, "6");

            // 如果使用了cache 的话 插入的值 会生成一个时间戳. 使用cache Hash的话 在使用传统HGet 获取值就会带上时间戳,导致出现问题
            //string str =  RedisHelper.CacheShell(key, "6", -1, () => { return "Arycs6"; });

            //var ret = RedisHelper.HScan(key, 0);
            //var arr = ret.Items;
            //Console.WriteLine("得到一个哈希的值 field="+ arr[0].field);
            //Console.WriteLine("得到一个哈希的值 value=" + arr[0].value);
            //Console.WriteLine("得到了一个Hash的值:" + str);

            long len = RedisHelper.HLen(key);
            Console.WriteLine("哈希的数量= " + len);
        }


        /// <summary>
        /// 测试 List
        /// </summary>
        public static void TestList()
        {
            string key = "list";
            // LPush  RPush  都是对列表的操作
            //RedisHelper.LPush(key,"元素2","元素3","元素4","元素5");
            //RedisHelper.RPush(key, "元素6", "元素7", "元素8", "元素9 ");

            // 获取长度
            //long len = RedisHelper.LLen(key);
            //Console.WriteLine("得到一个列表长度=" + len);

            //获取 对应索引
            //string str =  RedisHelper.LIndex(key, 0);
            //Console.WriteLine("得到0个索引的内容=" + str);

            //获取 一个范围的内容
            //string[] arr = RedisHelper.LRange(key, 0, 4);
            //foreach (var item in arr)
            //{
            //    Console.WriteLine("Name = " + item);
            //}

            //根据元素 插入 其后面, 根据索引的对应可以找到元素名,然后插入
            //RedisHelper.LInsertAfter(key,"元素3","元素3After");

            //根据索引 修改元素
            //RedisHelper.LSet(key, 0, "元素4修改");

            //队列
            string str = RedisHelper.RPop(key);
            Console.WriteLine("从列表中Pop元素 = " + str);
        }

        /// <summary>
        /// 测试 Set(集合)
        /// </summary>
        public static void TestSet()
        {
            string key = "nickName";

            //RedisHelper.SAdd(key, "傲然于尘世丶3", "傲然于尘世丶4", "傲然于尘世丶5", "傲然于尘世丶6", "傲然于尘世丶7", "傲然于尘世丶8");

            // 有重复的数据 返回 0  没有重复 返回1
            //long ret = RedisHelper.SAdd(key, "傲然于尘世丶9");

            //判断是否 有这个数据 
            //bool isExis = RedisHelper.SIsMember(key, "傲然于尘世丶9");

            //查询数量
            //long count =  RedisHelper.SCard(key);

            //取出数据,并且移除 , 有重载, 参数2为 几条数据
            //string str = RedisHelper.SPop(key);

            //移除对应元素
            RedisHelper.SRem(key, "傲然于尘世丶9");

            //返回集合中的所有成员,尽量少用
            string[] arr = RedisHelper.SMembers(key);
            foreach (var item in arr)
            {
                Console.WriteLine(item);
            }
            //Console.WriteLine("插入结果 = " + str);
        }

        /// <summary>
        /// 测试有序集合
        /// </summary>
        public static void TestZSet()
        {
            string key = "rank_fatting";

            //删除key,(List, String, Set ,Hash)
            //RedisHelper.Del(key);

            //RedisHelper.ZAdd(key, (100, 1), (80, 2));

            //for (int i = 0; i < 30; i++)
            //{
            //    int roleId = i;
            //    double score = new Random(i).Next(1, 100);
            //    // 注意 Score 在前面, 根据Score排序, 并且score可以重复,但是roleId 不可重复
            //    RedisHelper.ZAdd(key, (score, roleId));
            //}

            //返回的是角色编号 ,取0-20范围内的
            //string[] arr = RedisHelper.ZRange(key, 0, 20);
            //foreach (var item in arr)
            //{
            //    Console.WriteLine(item);
            //}

            //最常用排序 返回 键值 , 取0-20范围 ZRevRangeWithScores 是倒叙, ZrangeWithScores是正序
            //(string member, double score)[] lst = RedisHelper.ZRevRangeWithScores(key, 0, 30);
            //foreach (var item in lst)
            //{
            //    Console.WriteLine(item.member + ":" + item.score);
            //}

            //查询数量
            //long count = RedisHelper.ZCard(key);

            //根据分数范围 找到对应的 ID
            string[] arr = RedisHelper.ZRangeByScore(key,50,60);
            foreach (var item in arr)
            {
                Console.WriteLine("查询范围"+item);
            }
        }
    }
}
