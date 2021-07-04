# 基本服务器架构： 

Nginx 反向代理 ： 做web负载，

SHM共享缓存:Redis + MongoDB 当缓存没有数据时查询DB,然后反写入Redis, 其中WebServer,WorldServer,GameServer 都会访问

网关服务器(GatewayServer,GateServer) 多节点部署，客户端统一连接到网关服务器，多个网关可以有效负载分流玩家

中心服务器(WorldServer,CenterServer) 单节点 与所有Server(GatewayServer GameServer) 通讯，维护全局数据， 处理全局逻辑： 邮件 好友 组队 帮会等

游戏服务器(GameServer,NodeServer) 多节点部署，基于场景管理，一个服有若干场景，多线程

# 服务器线程模型：

网关服务器(GatewayServer,GateServer) : 

- 客户端连接管理器
- 区服通讯管理器
- 日志管理器

中心服务器(WorldServer,CenterServer) :

- 本区服务器管理器
- SHM数据管理器
- 区服通讯管理器
- 日志管理器
- 玩家管理器
- 榜单管理器
- 帮会管理器

游戏服务器(GameServer,NodeServer) :

- 场景管理器
- SHM数据管理器
- 日志管理器
- 区服通讯管理器
- 玩家管理器->处理玩家业务逻辑

# 创建项目

1. 创建空白的解决方案

2.  需要的内容：

   - YouYouServer.WebAccount (账号服务器集群) --->引用Model

   

   ​		这三个服务器两两相互连接, 都引用Model

   - YouYouServer.GatewayServer(网关服务器，多节点控制台)
   - YouYouServer.GameServer(游戏服, 多节点控制台)
   - YouYouServer.WorldServer(中心服务器, 单节点控制台)

   

   - YouYouServer.Core(公共基础类库) ---> 公共基础代码
   - YouYouServer.Model(实体模型类库) ---> 数据相关 --> 引用Core 和CSRedis
   - YouYouServer.CSRedis(Redis类库) ---> 访问CSRedis

3. .Net Core类库，相比于.Net Framework 可以跨平台

4. YouYouServer.Core使用NuGet, 安装 MongoDB.Bson，MongoDB.Driver, MongoDB.Driver.Core, Newtonsoft.Json


# MongoDB基础入门

作用 ： 用来存储数据，高并发的写数据和查询数据

1. 新建一个MongoDB项目目录(包含 config,db,dblog 三个文件夹)
2. 把安装目录的mongod.cfg拷贝到项目目录的config目录中
3. 在path环境变量里添加MongoDB的环境变量 X盘:\MongoDB\Server\版本\bin
4. 使用CMD执行 ： mongod --config "新建项目的mongod.cfg路径"
5.  把 mongod.cfg 中的 dbPath 修改为新文件夹的db文件夹，SystemLog的Path 修改为dblog文件夹下的mongod.log文件， port: 端口 bindIp:127.0.0.1
6. 运行mogoDB之后，使用robo3T 就可以连接对应mogoDB

```c#
//连接 MongoDB
MongoClient client = new MongoClient("mongodb://127.0.0.1");
//获取对应名字的库
IMongoDatabase mongoDataBase = client.GetDatabase("DBAccount");
//获取对应表里对应名字的集合，可以对集合进行操作，增删改查
IMongoCollection<T> collection = mongoDataBase.GetCollection<T>("Account");
```



- 如何在.net core 中多条件查询MongoDB

```c#
//Filter 过滤器，后面相当于条件
// 等于条件的 查询  t=>t.YFId , 1000
FilterDefinition<RoleEntity> filter =  Builders<RoleEntity>.Filter.Eq(t=>t.YFId ,25);
// 多语句 And 查询语句
filter = Builders<RoleEntity>.Filter.And(
	Builders<RoleEntity>.Filter.Eq(t => t.YFId, 100),
	Builders<RoleEntity>.Filter.Eq(t => t.NickName, "傲然于尘世丶703")
);
// 多语句 Or 查询
filter = Builders<RoleEntity>.Filter.Or(
       Builders<RoleEntity>.Filter.Eq(t => t.YFId, 88),
       Builders<RoleEntity>.Filter.Eq(t => t.NickName, "傲然于尘世丶703"),
       Builders<RoleEntity>.Filter.Eq(t => t.YFId, 83),
       Builders<RoleEntity>.Filter.Eq(t => t.YFId, 81),
       Builders<RoleEntity>.Filter.Eq(t => t.YFId, 55)
 );
// Ascending 升序  Descending降序  继续在后面.Ascending(XX) 先以第一个升序排序,然后在一第二个属性升序排列
SortDefinition<RoleEntity> sort = Builders<RoleEntity>.Sort.Ascending(t => t.YFId);
// 分页查询
long count;
List <RoleEntity> lst = roleDBModel.GetListByPage(filter,10,2,out count,field:new string[] {"YFId","NickName"}, sort);
```

# Redis基础入门

作用 ：共享缓存，用来做缓存

- Redis 中的数据结构: String(字符串), Hash(散列), List(列表), Set(集合), Sorted Set(有序集合)

  

#  启动顺序和逻辑

1. 启动WorldServer 监听网关服务器和游戏服务器的连接
2. 启动GameServer 连接到中心服务器
3. 中心服务器收到所有的有媳妇连接，存储游戏服列表
4. 启动GatewayServer 连接到中心服务器
5. 网关服务器从中心服务器拿到所有游戏服的列表
6. 网关服务器连接到所有的游戏服
7. 游戏服收到所有的网关服务器连接，存储网关服务器列表
8. 中心服务器判断本区准备就绪，通知web服更新状态
9. 客户端登录web服务器，通过算法拿到这个区的一个GatewayServer进行连接



使用IIS部署

- 打开Windows功能，将其中的Internet Information Services 以及Internet Information Services可承载的Web核心勾选上， 具体勾选哪些内容如图
- 下载.net Core 的SDK(x64) 和ASP.Net Core Runtime(Hosting Bundle)  https://dotnet.microsoft.com/download/dotnet
- 打开IIS管理器 -> 模块查询是否有AspNetCoreModuleV2，有则说明可以部署站点了
- 点击IIS管理器网站，添加网站，物理路径选择之前由WebServer发布出去的位置然后设置端口就可以正常访问了

nginx反向代理设置

- 官网上下载对应的版本 https://nginx.org/ ，项目工程中使用的是nginx1.16.1，下载好是一个zip的压缩包，解压导项目中的Release之中

- 修改conf中的nginx.conf文件

-  第一块红色 ：配置服务器集群

   第二块红色 ：配置反向代理

  ![image-20210624144503288.png](https://i.loli.net/2021/06/24/JzEWOvMLq9V7CbA.png)

  ```yaml
  #服务器集群名称为YouYouAccount
  upstream YouYouAccount {
  	server localhost:8001;
  	server localhost:8002;
  	server localhost:8003;
  }
  	
  #其中YouYouAccount 对应着upstream设置的集群名称
  proxy_pass http://YouYouAccount;
  #设置主机头和客户端真实地址,以便服务器获取客户端真是IP
  proxy_set_header	Host	$host;
  proxy_set_header	X-Real-IP	$remote_addr;
  proxy_set_header	X-Forwarded-For	$proxy_add_x_forwarded_for;
  ```

- 使用Nginx.bat进行启动和管理



# 部署CDN 

在IIS 中添加网站，物理路径选择当前项目的AsetBundle路径即可，端口为服务器中ChannelConfig.xml 中配置的SourceUrl 

在MIME 类型中添加 一个.*的扩展名， MIME类型为 application/octet-stream













