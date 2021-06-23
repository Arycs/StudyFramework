游戏框架入口 : 

GameEntry (注册所有的组件 : 15个常用组件)

## 组件 : 

所有组件 都继承自YouYouBaseComponent 

YouYouBaseComponent 继承自YouYouComponent 

YouYouBaseComponent : 只有基础组件来继承这个组件, 其中包含注册进入GameEntry的基础组件

YouYouComponent : 非基础组件用来继承该组件, 比如角色组件

Manager : 

每个组件都继承自 ManagerBase, 它是一个抽象类  

TODO : 为什么继承自ManagerBase,他作为抽象类其中什么都没写 有什么具体意义

每个组件有一个对应的管理器 XXManager, 组件中包含对应的管理类,然后管理类进行操作,组件用来给对外的调用

组件 是暴露在外的, 通过GameEntry.对应组件来进行调用



- Update优化 : 

创建一个IUpdateComponent 的接口, 让需要调用Update的组件实现其接口,然后各个组件的Awake中进行注册(使用GameEntry.RegisterUpdateComponent,将其添加进一个LinkedList), 然后在Update 循环该 LinkedList, 以此来达到 组件循环的效果



- Event : 事件组件

根据最开始的架构需求, Component 是暴漏在外面的给其他地方调用的组件, 因此 在Component中需要声明 对应的Manager, 通过Manager的方法,进行一层封装,以此来供外部调用

Event 事件中包含通用事件(CommonEvent) 以及 套接字事件(SocketEvent)

因此在EventManager中分别声明  SocketEvent 以及 CommonEvent 通过GameEntry.EventComponent.SocketEvent  来进行添加监听 ,移除监听等

GameEntry.EventComponent.CommonEvent

事件是通过 一个委托(个人理解类似于回调), 用一个Dic 和List<Delegate> 来进行存储, 一个Key可能对应多个委托, 移除监听则是通过删除Dic中的对应key-Value进行处理, 派发则是通过Key寻找到对应的List<>然后一次遍历执行即可

```c#
//添加监听, CommonEventId.RegComplete 为键,OnRegComplete为回调,也即Dic中的List<Delegate> 
GameEntry.Event.CommonEvent.AddEventListener(CommonEventId.RegComplete, OnRegComplete);
//同一个key 可以注册多个事件
GameEntry.Event.CommonEvent.AddEventListener(CommonEventId.RegComplete, OnTestComplete);
//派发监听, 即传入一个参数对应调用 CommonEventId.RegComplete对应中的方法
GameEntry.Event.CommonEvent.Dispatch(CommonEventId.RegComplete,123);

	private void OnRegComplete(object userdata)
    {
        Debug.Log("注册成功 :" + userdata);
    }

    private void OnTestComplete(object userdata)
    {
        Debug.Log("测试 : " + userdata);
    }
```

派发之后 输出结果为 注册成功 : 123 和 测试 : 123

- Time : 事件组件

Component 里对应存储一个 Manager  , 具体操作在Manager中进行, Component只是封装了一层对外的方法

TimeManager 中存储一个链表,用来记录所有的定时器 LinkedList<TimeAction> 

TimeManager中包含 注册,移除,监听(OnUpdate,因为Time需要不断的循环,因此TimeComponent还需要同时实现IUpdateComponent 接口,即在OnAwake中 

​	GameEntry.RegisterUpdateComponent(this);

)

其中TimeAction 是单独的计时器,其中包括 

初始化(Init) : ,传入对应的延迟时间,间隔,循环次数,开始回调,运行回调,结束回调等系列方法,

开始运行(Run): 将自身注册到TimeManager链表中,才能进行Update循环(TimeComponent实现IUpdateComponent接口,调用TimeManager中链表中的每一个TimeAction的Onupdate)

暂停(Pause) : 将IsRunning(是否正在运行标志)设置为false, TODO 如何使用暂停, 为什么课程中没有ReStart, 具体该如何使用

停止(Stop) : 执行完成过回调,然后将IsRunning(是否正在运行标志)设置为false, 移除TimeManager链表中的 对应TimeAction

OnUpdate(每帧执行) : TimeManager中循环遍历链表中的TimeAction的OnUpdate, 由TimeComponent来调用

通过Run 来进行开始执行

创建定时器,涉及到接下来的 对象池组件管理,后续在说

```c#
//创建了一个定时器
TimeAction action = GameEntry.Time.CreateTimeAction();
Debug.Log("创建了定时器1");
//初始化
action.Init(1, 1, 2,
   () => { Debug.Log("定时器1开始运行"); },
   (int loop) => { Debug.Log("定时器1 运行中 剩余次数 = " + loop); },
   () => { Debug.Log("定时器1运行完毕"); }
);
//开始执行
action.Run();
```

Pool组件

对象池组件,用来管理和回收对象的

对象池 : 就是在Unity中经常用到同一个Prefab多次,反复实例化很小号资源,因此在加载的过程中就把一批Prefab实例化好,存入对象池,用的时候在拿出来,不用的时候放回去

存入对象池的元素应具有如下特征 : 1. 场景中大量使用 2. 存在一定的生命周期,会较为频繁的申请和释放

当前框架的对象池目前有两种 :

1. 游戏物体对象池(GameObjectPool)用来队Prefab进行缓存的
2. 类对象池(ClassObjectPool)用来管理类, 比如定时器,StringBuilder 这种类

首先来说类对象池 :

​	ClassObjectPool 中存在一个 

​		类对象池中的常驻数量 : 用字典来表示, Key 为 类的Hash值, 值为byte类型的数量

​		类对象池的一个字典 : Key为类的Hash值, 值为一个Queue<object>队列,用来存储对应的类

​		一个在Inspector的字典 : 用于编辑器扩展使用的根据类的类型,显示类的数量

​	和传统的对象池一样, 无非就是取出对象(Dequeue),存入对象(Enqueue), 释放对象池(Clear)

​	Dequeue(取出对象) : 通过泛型T ,来找到对应的哈希值(typeof(T).GetHashCode), 通过这个Key找到对应字典里的Queue, 如果没有则new 一个队列, 然后判断队列里是否有数量,有就直接取出一个,反之则直接new 一个

​	Enqueue(存入对象) : 也是通过泛型T, 找到对应的池中的Queue,然后入队, 放入对象池的时候一定要移除对应的监听, 比如Time类, 

​	Clear(清空对象池) : 遍历对象池然后拿到对应的队列, 通过对比常驻数量(多余的)来进行置空(一个一个Dequeue) ,最后 GC 回收一次即可

示例代码 

```c#
 		if (Input.GetKeyUp(KeyCode.A))
        {
            StringBuilder sbr = GameEntry.Pool.DequeueClassObject<StringBuilder>();
            sbr.Length = 0; // 初始化
            sbr.Append("123");
            Debug.Log(sbr.ToString());
            GameEntry.Pool.EnqueueClassObject(sbr);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            StringBuilder sbr = GameEntry.Pool.DequeueClassObject<StringBuilder>();
            Debug.Log(sbr.ToString());  //如果此时取出的StringBuilder 不进行初始化,则会输出 123
        }
```



类对象池 使用注意事项:

1. 使用前 重置或者回池前重置, 例如StringBuilder 这种从池中调用时要初始化,否则上次的数据会被保存 TODO:思考为什么会有这种原因
2. 不能使用带参数的构造函数类,如果想用带参数的,单独写Init方法,作为初始化方法 TODO:思考为什么不能使用带参数的类

游戏物体对象池(GameObjectPool):

​	游戏物体对象池使用的是PoolManager的插件

​	GameObjectPool 中保存一个 字典 :  Key 为 byte类型的Id, Value 为GameObjectPoolEntity类型的对象池实体

​	GameObjectPoolEntity: 包含 编号(PoolId),  名字(PoolName), 是否开启缓存池自动清理模式(cullDespawned), 缓存池自动清理,保留几个对象不清理(CullAbove), 清理间隔(CullDelay), 每次清理几个(CullMaxPerPass) , 对应游戏物体对象池(Pool,  这个是利用PoolManager插件生成出来的对应对象池,用来存放符合ID的物品,自动清理等功能也都是该插件自带)

​	GameObjectPool 中有以下几个方法:

​	Init : 该方法使用协程, 因为考虑到切换场景销毁需要时间, 初始化对象池,通过传进来的一个数组,为每一个对应的GameObjectPoolEntity 都生成一个对象池(使用插件生成,可以通过场景中的Pool下面的子物体, 每一个子物体都是一个GameObjectPoolEntity.PoolName + "Pool" ,子物体下就存着当前对象池中的物体),有对象池则销毁,然后在一次生成新的对象池 

TODO: 思考为什么切换场景要销毁旧的对象池,然后再生成新的呢

​	Spawn : 从对象池取出对象 , 通过Id 取到对应的entity, 然后通过Prefab利用插件提供的GetPrefabPool 找到对象池, 如果有则调用回调, 反之没有则通过对象池生成一个,设置上对应属性,在执行回调方法

​	Despawn :回池, 通过ID 找到对应的entity,然后调用entity.Pool.Despawn将预制体回收即可,*** 这里的回池只是将Prefab隐藏, 不是销毁

示例代码

```
		 //两个预制体
		public Transform trans1;
    	public Transform trans2;
		if (Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(CreateObj());
        }
        
        private IEnumerator CreateObj()
    	{
        for (int i = 0; i < 20; i++)
            {
                yield return  new WaitForSeconds(0.5f);
		
                GameEntry.Pool.GameObjectSpawn(1,trans1, (Transform instance) =>
                {
                    instance.transform.localPosition += new Vector3(0, 0, i * 2);
                    instance.gameObject.SetActive(true);
                    StartCoroutine(DeSpawn(1, instance));
                });

                GameEntry.Pool.GameObjectSpawn(2,trans2, (Transform instance) => 
                { 
                    instance.transform.localPosition += new Vector3(0, 0, i * 5);
                    instance.gameObject.SetActive(true);
                    StartCoroutine(DeSpawn(2, instance));
                });
            }
    	}
    	//回池, 生成之后20秒之后将其回池
        private IEnumerator DeSpawn(byte poolId ,Transform instance)
        {
            yield return  new WaitForSeconds(20);
            GameEntry.Pool.GameObjectDespawn(poolId,instance);        
        }
```

对象池 使用了编辑器扩展 : 这里对编辑器扩展进行初步的了解和学习

CustomEditor特性 :  自定义编辑器, 描述了用于编辑器实时运行类型的一个编辑器类

```c#
//使用方法 , CustomEditor修饰想要扩展的类,比如PoolComponent, 然后让对应的扩展类继承自Editor , 重写OnInspectorGUI方法 即可 , true 则表示所有继承该类的子类都会在Inspector面板绘制对应属性
[CustomEditor(typeof(PoolComponent),true)]
public class PoolComponentInspector : Editor{}
```

SerializedProperty : 被序列化对象(SerializedObject)对象上的字段, 其中被Private,Protected的字段要加上[SerializeField]才能被序列化,Public的字段则无需处理,属性不能被serializedObject.FindProperty("字段名"); 该API找到,会报空

https://docs.unity3d.com/ScriptReference/SerializedProperty.html

SerializedObject :  被序列化的对象,被CustomEditor 修饰的typeof, 通过FindProperty("字段名")获取到对应的SerialiezdProperty

https://docs.unity3d.com/ScriptReference/SerializedObject.html

EditorGUILayout 是用来在面板上绘制各种内容的. 比如Slider,Space,BeginVertical,Label 等扩展的时候 看一下对应类下面的方法即可

serializedObject.ApplyModifiedProperties() 将对SerializedObject的修改的属性进行应用. 

Repaint() 保持实时刷新

​	Fsm : 状态机组件 

状态机:

由FsmManager(状态机管理器) 管理创建,销毁,停止 每一个状态机

Fsm<T> : 状态机 , T泛型表示对应的拥有者, 比如Fsm<ProcedureManager>, 表示 是流程管理器的状态机, 同理 也可以由Fsm<BaseRole> 角色的状态机 ,在对应的拥有者里 进行切换状态, 最后也会调回Fsm的ChangeState(), 状态机里有 当前状态, 状态字典(有多少种状态),获取状态(通过key,在字典中找到对应状态), OnUpdate(拥有者对应执行当前状态的Update), 改变状态(修改状态机的状态),结束(执行退出当前状态以及关闭所有状态机的功能) 这几个功能

FsmBase : 状态机基类, 所有状态机都共有的属性(状态机编号,拥有者,当前状态,关闭状态机方法), 是个抽象类

FsmState<T> : 状态机的对应状态, 包含了所属状态机 ,进入当前状态,离开当前状态,循环,离开,销毁的方法

Fsm状态机退出的时候,会自己执行shutdown()方法.没必要在FsmManager上再次释放

示例代码

```c#
//创建 FsmState状态 数组,用来记录各个状态	 
FsmState<BaseRoleController>[] states = new FsmState<BaseRoleController>[3];
states[0] = new IdleState();
states[1] = new RunState;
states[2] = new AttackState();
//通过 GgameEntry.Fsm创建对应的状态机
curFsm = GameEntry.Fsm.Create(this, states);
//在各种状态中添加要做的事,比如进入,循环,离开,销毁等
//curFsm进行切换
curFsm.ChangeState(对应的数组下标);
```

​	Procedure : 流程组件

Todo : 思考为什么要用到流程组件, 因为感觉根据状态来判断只有几种流程状态 :  

流程 是根据自己游戏进行思考的, 不能死套 框架, 实际就是状态机,只不过相比于原本的基于场景的管理来说, 用流程控制器是更好的 , 流程写在ProcedureManager之中

其实也是一种状态机,只不过是来控制流程的, 其中涉及到一个传递参数问题, 在Fsm之中传递参数会会导致频繁的拆装箱,因此也涉及到了 自定义类变量的问题.



状态机在切换状态的过程中有时候会需要频繁的传递参数,因此需要在Fsm之中添加传递参数,但是因为

Fsm并不知道具体的类型, 因此需要定义字典 是 Dictionary<string ,object> 在传递过程中就会导致有拆装箱过程 ,所谓传递参数 就是在不同状态机状态过程中 通过键值对进行设置. 比如我在A流程设置一个key 为"name" value 为"Arycs"的值 , 在B流程之中使用 key 就能找到对应的value

装箱 : 值类型隐式转换成引用类型

拆箱 : 引用类型隐式转换成值类型

装箱和拆箱的过程 : 装箱会把值类型转换为引用类型.这个新的引用类型(即箱子)分配在堆上,该值类型的一个副本会存放在引用类型的内部.箱子包含了值类型的副本,并提供了值类型公有接口的功能.若是从箱子中获取数据,那么将创建其中值类型的一个副本并返回. 装箱和拆箱的关键之处是:装箱时存放的是值类型的副本,拆箱时返回的时值类型的另一个副本.所以需要访问该值类型的方法都要穿过该箱子才能访问它



避免拆装箱 : 使用一个箱子(VariableBase)来装值类型/引用类型(包含变量类型,引用计数,

保留记数,引用计数++,释放对象,引用计数-- ) 

使用Variable<T> : VariableBase : 当前存储的真实值, 变量类型,  使用泛型来对应记录

VarInt : Variable<int> : 举例一个VarInt,

​	包含Alloc(分配一个, 从对象池中获取VarInt,然后初始化,引用++)

​	重写运算符 operator int :让VarInt 能和 int 进行直接比较转换

使用Variable变量的注意事项 : 

1. 每一个Variable变量,声明(Alloc)一定要和一个释放(Release)对应上, 否则会导致变量一直在池中无法被回收,也就没有意义了,其中Alloc 写在使用变量的最开始,Release写在试用结束后

2. 在协程中使用的话,需要在最开始就进行一次保留(Retain,引用次数++),防止被释放掉

​	DataTable : 表格组件

TODO : Task 相关内容 , 异步加载

​	Data : 数据组件

没有对应的Manager 脚本, 只是用来存储数据的 ,清空数据的时机需要自行根据条件判断

CacheData (临时缓存数据) :  临时缓存,跨场景清空

SysData(系统相关数据) : 游戏周期内可以不清空

UserData(用户相关数据) : 退出登录时清空

用来存储所有数据相关的, 如果有需要还需额外建立,比如UserData 有点类似于公司项目的MyDataMgr, 其余不同的类

 从服务器接受下来的数据都是在XXXDataManager 里进行处理, 对应协议生成对应实体(比如服务器任务协议, 就要对应有一个任务实体,然后 在UserDataManger 中队Proto协议进行处理 数据)

GameEntry.Data.UserDataManager.ReceiveTask(Proto xx) ; 在接受消息的地方调用 



等 需要什么类型的数据就添加什么数据 

​	Http : 网络连接组件

Todo : UnityWebRequest 类 , Post 请求 , MD5加密, WWWForm

HttpRoutine: Http访问器, 其中Get请求 是向Url中请求数据, Post方法是向Url中提交要被处理的数据,每一个访问都要实例化一个对应的访问器,因此该访问器应该使用类对象池来进行管理. 回池时间在请求完数据之后

LitJson : 相关内容查看

​	Socket : 套接字组件

压缩使用的是Zlib.dll , 加密使用Crc16

SocketTcpRoutine : SocketTcp访问器 , 类似功能和Http访问器,进行加密,发送信息等 

SocketManager : 用链表来管理所有的SocketTcpRoutine ,注册,移除,更新 所有的SocketTcpRoutine

SocketComponent : 依旧Manager,注册自身Update,对外提供Manager的注册移除方法 , 其上有个MainSocket ,用来连接服务器?

使用Proto作为通信协议 

SocketProtoListener : Socket协议监听,用来管理所有的Socket监听的, 是由工具生成对应的,在SocketComponent Start 开始监听, ShutDown 移除监听 , 使用XXXhandler用来处理Proto回包的

利用YouYou工具来进行生成 协议,以及Handler, 协议都实现Iproto 接口

todo : CSDN文章 , TCP UDP数据包大小的限制 

​	Localization : 本地化组件

​	Scene : 场景组件

​	Setting : 设置组件

​	GameObj : 游戏物体组件

​	Resource : 加载资源组件

​	Download : 下载组件

​	UI : UI组件

UI加载过程 : 配合系统表 来进行加载

​	系统表 : Sys_prefab(预设) , Sys_Effect(特效) , Sys_UIForm(UI窗体), Sys_

UIRoot : 整个场景只有一个UIRoot, UI方便进行优化, 切换场景的时候不需要销毁,隐藏即可

UIGroup :

​	UI分组, 将不同的界面 分别放在不同的分组 , 初步设置为4个分组, 在Unity面板中显示,UIComponent中添加对应字典 ,对外提供给根据Id找到对应UIGroup

UI适配 : 

​	UICamera 的Size 应该由 分辨率的Y / 2  (即1280 X 720 为例, Size 为360) 

​	常用的屏幕比例 : ( 5:4  4:3  3:2  16:10  16:9(标准分辨率) 2436:1152  17:9  18:9  19:8) , 

​	UILoading界面的适配 : 调整画布(UIRoot)的CanvasScaler 中的Match ,大于标准分辨率的 设置为0 , 小于标准分辨率的, 利用标准分辨率的比值(1.777) 减去对应分辨率的比值,剩下的值为 Match的值

​	全屏窗口的适配 : 默认CanvasScaler的Match 为1, 利用锚点来进行控制适配

​	普通窗口的适配 : 大于或等于 标准比值(16:9) Match缩放为1 小于标准比值 缩放为 0

UI框架 : 

​	通过系统表来进行加载UI相关内容,在预加载流程,ProcedurePreload 

- 在UIManager 中进行对UI的操作
  - 打开UI 窗体(OpenUIForm , 1.判断是否已经打开 2.进行本地化处理 3.加载镜像到内存 4. 实例化对应物体,设置父物体 5. 获取到挂载脚本,初始化,所有的都是继承自UIFormBase,调用Init传入所有相关数据)
  - 关闭UI 窗体, 调用 UIbaseForm 的ToClose 来执行关闭时相关的方法
  - 利用一个 链表来记录打开的界面,用来处理只打开一个(打开加入,关闭移除)
  - 

- 每个UI预制体有一个对应的预制体脚本,用来处理的操作,继承自UIFormBase,
-  UIFormBase : 
   - UIFormId, 记录所有的UIId,方便寻找和处理, 类似于公司项目的config_ui
   - GroupId 分组编号, 用来判断放在那个UI组下面
   - CurrCanvas 当前画布, 用来做缩放和适配
   - CloseTime 关闭事件, 用来针对对象池回收使用
   - DisableUILayer 是否禁用层级管理
   - IsLock  是否锁定 也是配合对象池来使用的
   - UserData 用户数据
   - Awake : 获取画布
   - Init - OnInit: 初始化,初始化以上属性,并且OnInit
   - Open - OnOpen : 打开的时候,判断是否刷新数据, 并且判断是否层级管理
   - OnDestroy - OnBeforDestroy : 销毁的时候做的事情
-  UILayer : 层级管理 
   -  m_UILayerDic : 用一个字典来记录对应UI编号 与层级相对应, 设置UI的sortLayer
-  UIPool : UI对象池, 用来管理关闭的UI ,与之前的对象池不太一样,属于一个简单的对象池,判断关闭就SetActive(false) ,对应添加, 打开就取出然后移除
   -  m_UIFormList : 用一个列表来维护对象池中的UIFormBase
   -  根据UIComponent上的 时间间隔,每隔多少秒进行一次回收,回收的对象进行销毁

多语言 : 

继承xLua框架:

xLua中读表和收发通讯协议:

​	Lua : Lua组件 , LuaComponent 上有一个LuaManager, Manager里面包含一个全局的LuaEnv, 通过这个LuaEnv进行加载Lua

​	Lua的UI界面都要挂在LuaForm(继承自UIFormBase) , 声明4个委托(OnInit,OnOpen,OnClose,OnBeforDestory)

Lua 中读表的方式 : 通过表格工具生成对应的Entity,DBModel(路径xLuaLogic/DataTable/Create), 然后在GameInit中进行读取, 在其他任何地方利用Model.GetList 来获取数据

Lua中Proto通信协议的使用方式 : 通过表格生成对应的Proto消息和Handler(返回消息处理地方), 然后在ProtoCodeDef.bytes 中进行添加对应的Proto协议编号,以及Require对应文件, 在SocketProtoListenerForLua.bytes 中进行集体监听(CS中也有一个,只不过使用Lua监听的话使用这个)



资源管理 : 

​	分为几个大类:  特效角色音效基本上打成散包, 

CusShaders(自定义Shader) : 

xLuaLogic(Lua逻辑脚本):

UI: 

​	UIFont(Ui 字体)

​	UIPrefab(预制体)

​	UIRes(资源) : 图集等一系列其他内容

Scenes(场景): 

Effect(特效): 角色特效(技能..) UI特效, 

Role(角色) : 

Audio(声音) : 

AB包 加载到内存之后 可以通过Profiler中的Memory 里面的Detailed采样查看 

调用UnLoadUnuseAsset 时候, 会卸载掉没使用的资源,但是如果预制体身上本身引用了某个图片或者资源,则不会被卸载,因此在关闭UI界面的时候,要遍历身上的组件,将其引用置空

bundle.Unload : 卸载加载出来的AssetBundle 

Resource.UnloadUnuseAsset : 卸载通过AB 包加载的资源



Texture2D 与 Sprite 关系

Texture2D 中包含多种类型的贴图, 相当于原图, 可以设置成sprite,normalmap 等

sprite中记录了Texture的部分信息,,也记录了Sprite的信息,所以可以通过这些信息去导出精灵

https://www.jianshu.com/p/0d18ac565563  一篇相关引用的文章





做安全防护 : 

1. 服务器进行验证

	2. 通信协议加密
 	3. 防止修改客户端内存(比如DNF 先搜索一下某个属性,比如 我名望10000,然后在更改这个值,在找这个新的数值,就能定为到对应的属性, 然后就可以进行修改) 利用一个SafeIngter 进行处理,防止值 直接被人搜索到         -------------- 使用SafeInteger 的话 必须先赋值







循环字典的方式

定义迭代器

var enumerator = 字典 . GetEnumerator();

遍历

while(enumerator.MoveNext()){

​	循环体

}