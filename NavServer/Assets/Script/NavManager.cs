using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class NavManager : MonoBehaviour
{
    /// <summary>
    /// 寻路代理
    /// </summary>
    public NavMeshAgent Agent;

    private NavMeshPath path;

    private LinkedList<ServerClient> m_ServerClientList;

    public string ServerIP = "192.168.0.109";
    public int ServerPort = 29001;

    public static NavManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        m_ServerClientList = new LinkedList<ServerClient>();
        path = new NavMeshPath();

        StarListen();
    }
    
    void Update()
    {
        LinkedListNode<ServerClient> curr = m_ServerClientList.First;
        while (curr != null)
        {
            curr.Value.OnUpdate();
            curr = curr.Next;
        }
    }

    /// <summary>
    /// 获取路径点
    /// </summary>
    /// <param name="sceneId"></param>
    /// <param name="beginPos"></param>
    /// <param name="endPos"></param>
    /// <returns></returns>
    public NavMeshPath GetNavPath(int sceneId, Vector3 beginPos, Vector3 endPos)
    {
        Agent.Warp(beginPos);
        Agent.CalculatePath(endPos, path);
        return path;
    }


    private Socket m_ListenSocket;

    #region StarListen启动监听

    /// <summary>
    /// 启动监听
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void StarListen()
    {
        //实例化Socket
        m_ListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        //向操作系统申请一个可用的IP 和 端口用来通讯
        m_ListenSocket.Bind(new IPEndPoint(IPAddress.Parse(ServerIP),ServerPort));
        m_ListenSocket.Listen(20);
        Debug.LogFormat("寻路服务器 启动监听{0}成功",m_ListenSocket.LocalEndPoint.ToString());

        Thread mThread = new Thread(ListenClientCallBack);
        mThread.Start();
    }

    #endregion

    #region ListenClientCallBack 监听回调

    /// <summary>
    /// 监听回调
    /// </summary>
    /// <param name="obj"></param>
    private void ListenClientCallBack(object obj)
    {
        while (true)
        {
            //接受服务器客户端请求
            Socket socket = m_ListenSocket.Accept();

            IPEndPoint point = (IPEndPoint) socket.RemoteEndPoint;
            Debug.LogFormat("游戏服{0}链接成功",point.ToString());
            
            //实例化一个服务器客户端
            AddServerClient(new ServerClient(socket));
        }
    }
    
    #endregion

    public void AddServerClient(ServerClient serverClient)
    {
        m_ServerClientList.AddLast(serverClient);
    }
    
    public void RemoveServerClient(ServerClient serverClient)
    {
        Debug.Log("游戏服断开链接");
        m_ServerClientList.Remove(serverClient);
    }

}