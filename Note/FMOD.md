FMOD

![image-20210614164624334](C:\Users\Administrator.Genshin-2020INP\AppData\Roaming\Typora\typora-user-images\image-20210614164624334.png)

Assets : 原始音频资源

Events : 每一个音效事件,音效最基本的单位,会对应一个EventPath以及UUID(一个Event不等于一个原始音频资源,而是可能对多个音频资源进行编辑合成为一个Event)

Banks : 用于存储多个Event,同一个bank将会打包成一个.bank文件或者.bytes文件

Instance : Unity下的概念,相当于一个Event的实例,会进行音效的播放



FMOD加载方式 :

StreamAsset :直接放在包下面,优点 : 不需要加载 缺点 : 不能热更

Assetbundle :AB包加载 优点 : 可以热更 缺点 : 需要加载



FMODStudio使用方法 ： 

1. 将已有资源拖入进Assets,导入进的素材进行解压缩一次，将Assets中的音效CreateEvent到Events界面，其中有选择是2D/3D，再由Events的声音，Assgin To Bank, 最后进行Build，就可以在Unity中听到对应的音乐了![image-20210614170532967](C:\Users\Administrator.Genshin-2020INP\AppData\Roaming\Typora\typora-user-images\image-20210614170532967.png)
2. 
3. 





使用流程 ：

1.　导入包到Unity之中
2.　FMOD -> EditorSetting -> Project(FMOD项目,当前项目选择这个)/SinglePlatformBuild(单一平台)/MultiplePlatformBuild(多平台) -> Studio Project Path (FMOD项目路径,FMOD项目需要在FMOD Studio 中Build一次) -> 选择加载方式(AssetBundle, 注意一点 需要提前把FMOD Asset Folder 中的路径(Download/Audio)设置上, 因为他使用的是Bytes 文件 ,会自动删除更新, 本项目中的表和Lua逻辑都是bytes, 防止他搜索根目录删除错误文件)
3.　Event Browser : 用来查看 FMOD Studio 中的Events,也可以临时播放使用 
4.　 如果需要3D 场景因 需要添加一个 FMOD Studio Listener 脚本
5.　如果需要某物体 发出声音 则需要对应给他添加脚本 Studio Event Emitter(事件发射器脚本), 后续案例跟进