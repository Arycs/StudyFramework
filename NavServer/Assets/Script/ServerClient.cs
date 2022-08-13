using System;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.AI;
using YouYou.Proto;
using YouYouServer.Core;

/// <summary>
/// 服务器客户端,
/// 相当于一个中转站,
/// GameServer和GateWayServer连接到WorldServer时,WorldServer先创建ServerClient然后根据对应的消息实例化对应的服务器
/// </summary>
public class ServerClient
{
    /// <summary>
    /// Socket时间监听派发器
    /// </summary>
    public EventDispatcher EventDispatcher { get; private set; }

    /// <summary>
    /// Socket连接器
    /// </summary>
    public ClientSocket ClientSocket { get; private set; }

    public ServerClient(Socket socket)
    {
        EventDispatcher = new EventDispatcher();

        ClientSocket = new ClientSocket(socket, EventDispatcher);
        ClientSocket.OnConnectFail = () => { NavManager.Instance.RemoveServerClient(this); };
        ClientSocket.OnDisConnect = () => { NavManager.Instance.RemoveServerClient(this); };
        AddEventListener();
    }

    public void OnUpdate()
    {
        ClientSocket.OnUpdate();
    }

    /// <summary>
    /// 监听服务器客户端 连接到服务器
    /// </summary>
    private void AddEventListener()
    {
        EventDispatcher.AddEventListener(ProtoIdDefine.Proto_GS2NS_GetNavPath, OnGS2NS_GetNavPath);
    }

    private void OnGS2NS_GetNavPath(byte[] buffer)
    {
        GS2NS_GetNavPath proto = GS2NS_GetNavPath.Parser.ParseFrom(buffer);

        Debug.LogFormat($"Scene Id => {proto.SceneId}");
        Debug.LogFormat($"Task Id => {proto.TaskId}");
        Debug.LogFormat($"Begin Pos => {proto.BeginPos.X}, {proto.BeginPos.Y}, {proto.BeginPos.Z}");
        Debug.LogFormat($"End Pos => {proto.EndPos.X}, {proto.EndPos.Y}, {proto.EndPos.Z}");

        NS2GS_ReturnNavPath retProto = new NS2GS_ReturnNavPath();

        NavMeshPath path = NavManager.Instance.GetNavPath(proto.SceneId,
            new Vector3(proto.BeginPos.X, proto.BeginPos.Y, proto.BeginPos.Z),
            new Vector3(proto.EndPos.X, proto.EndPos.Y, proto.EndPos.Z));

        retProto.TaskId = proto.TaskId;
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            retProto.Valid = true;
            Vector3[] vector3s = path.corners;
            int len = vector3s.Length;
            for (int i = 0; i < len; i++)
            {
                Vector3 vector = vector3s[i];
                retProto.Path.Add(new NS2GS_Vector3() {X = vector.x, Y = vector.y, Z = vector.z});
            }
        }
        
        ClientSocket.SendMsg(retProto);
    }
}