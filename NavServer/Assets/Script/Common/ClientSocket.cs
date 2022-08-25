using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using YouYou;

namespace YouYouServer.Core
{
    public class ClientSocket
    {
        //事件派发器
        private EventDispatcher m_EventDispatcher;

        //客户端Socket
        public Socket m_Socket;

        //作为服务器端监听客户端时候接收数据的线程
        private Thread m_ReceiveThread;


        #region 接收消息所需变量
        //接收数据包的字节数组缓冲区
        private byte[] m_ReceiveBuffer = new byte[2048];

        //接受消息的队列
        private Queue<byte[]> m_ReceiveQueue = new Queue<byte[]>();
        
        //接收数据包的缓冲数据流
        private MMO_MemoryStream m_ReceiveMS = new MMO_MemoryStream();

        /// <summary>
        /// 发送用的MS
        /// </summary>
        private MMO_MemoryStream m_SocketSendMS = new MMO_MemoryStream();

        /// <summary>
        /// 接收用的MS
        /// </summary>
        private MMO_MemoryStream m_SocketReceiveMS = new MMO_MemoryStream();

        #endregion

        #region 发送消息所需变量
        //发送消息队列
        private Queue<byte[]> m_SendQueue = new Queue<byte[]>();

        //压缩数组的长度界限
        private const int m_CompressLen = 512;
        #endregion

        /// <summary>
        /// 这个构造函数 是当做客户端使用时候使用
        /// </summary>
        /// <param name="eventDispatcher"></param>
        public ClientSocket(EventDispatcher eventDispatcher)
        {
            m_EventDispatcher = eventDispatcher;
        }


        /// <summary>
        /// 这个构造函数 是当做服务器端时候使用
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="role"></param>
        public ClientSocket(Socket socket, EventDispatcher eventDispatcher)
        {
            m_Socket = socket;
            m_EventDispatcher = eventDispatcher;

            //启动线程 进行接收数据
            m_ReceiveThread = new Thread(ReceiveMsg);
            m_ReceiveThread.Start();
        }

        #region ReceiveMsg 接收数据
        /// <summary>
        /// 接收数据
        /// </summary>
        private void ReceiveMsg()
        {
            //异步接收数据
            m_Socket.BeginReceive(m_ReceiveBuffer, 0, m_ReceiveBuffer.Length, SocketFlags.None, ReceiveCallBack, m_Socket);
        }
        #endregion

        #region IsSocketConnected 判断socket是否连接
        /// <summary>
        /// 判断socket是否连接
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        bool IsSocketConnected(Socket s)
        {
            return !((s.Poll(1000, SelectMode.SelectRead) && (s.Available == 0)) || !s.Connected);
        }
        #endregion

        #region ReceiveCallBack 接收数据回调
        /// <summary>
        /// 接收数据回调
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                if (IsSocketConnected(m_Socket))
                {
                    int len = m_Socket.EndReceive(ar);

                    if (len > 0)
                    {
                        //已经接收到数据

                        //把接收到数据 写入缓冲数据流的尾部
                        m_ReceiveMS.Position = m_ReceiveMS.Length;

                        //把指定长度的字节 写入数据流
                        m_ReceiveMS.Write(m_ReceiveBuffer, 0, len);

                        //如果缓存数据流的长度>2 说明至少有个不完整的包过来了
                        //为什么这里是2 因为我们客户端封装数据包 用的ushort 长度就是2
                        if (m_ReceiveMS.Length > 2)
                        {
                            //进行循环 拆分数据包
                            while (true)
                            {
                                //把数据流指针位置放在0处
                                m_ReceiveMS.Position = 0;

                                //currMsgLen = 包体的长度
                                int currMsgLen = m_ReceiveMS.ReadUShort();

                                //currFullMsgLen 总包的长度=包头长度+包体长度
                                int currFullMsgLen = 2 + currMsgLen;

                                //如果数据流的长度>=整包的长度 说明至少收到了一个完整包
                                if (m_ReceiveMS.Length >= currFullMsgLen)
                                {
                                    //至少收到一个完整包

                                    //定义包体的byte[]数组
                                    byte[] buffer = new byte[currMsgLen];

                                    //把数据流指针放到2的位置 也就是包体的位置
                                    m_ReceiveMS.Position = 2;

                                    //把包体读到byte[]数组
                                    m_ReceiveMS.Read(buffer, 0, currMsgLen);

                                    lock (m_ReceiveQueue)
                                    {
                                        m_ReceiveQueue.Enqueue(buffer);
                                    }
                                }

                                //==============处理剩余字节数组===================

                                //剩余字节长度
                                int remainLen = (int)m_ReceiveMS.Length - currFullMsgLen;
                                if (remainLen > 0)
                                {
                                    //把指针放在第一个包的尾部
                                    m_ReceiveMS.Position = currFullMsgLen;

                                    //定义剩余字节数组
                                    byte[] remainBuffer = new byte[remainLen];

                                    //把数据流读到剩余字节数组
                                    m_ReceiveMS.Read(remainBuffer, 0, remainLen);

                                    //清空数据流
                                    m_ReceiveMS.Position = 0;
                                    m_ReceiveMS.SetLength(0);

                                    //把剩余字节数组重新写入数据流
                                    m_ReceiveMS.Write(remainBuffer, 0, remainBuffer.Length);

                                    remainBuffer = null;
                                }
                                else
                                {
                                    //没有剩余字节

                                    //清空数据流
                                    m_ReceiveMS.Position = 0;
                                    m_ReceiveMS.SetLength(0);

                                    break;
                                }
                            }
                        }

                        //进行下一次接收数据包
                        ReceiveMsg();
                    }
                    else
                    {
                        //客户端断开连接
                        OnDisConnect?.Invoke();
                    }
                }
                else
                {
                    //客户端断开连接
                    OnDisConnect?.Invoke();
                }

            }
            catch (Exception ex)
            {
                //客户端断开连接
                OnDisConnect?.Invoke();
            }
        }


        #endregion

        private int m_ReceiveCount = 0; //处理的包数量

        public void OnUpdate()
        {
            #region 从队列中获取数据

            while (true)
            {
                if (m_ReceiveCount <= 50)
                {
                    m_ReceiveCount++;
                    lock (m_ReceiveQueue)
                    {
                        if (m_ReceiveQueue.Count > 0)
                        {
                            //得到队列中的数据包
                            byte[] buffer = m_ReceiveQueue.Dequeue();

                            MMO_MemoryStream ms = m_SocketReceiveMS;
                            ms.SetLength(0);
                            ms.Write(buffer, 0, buffer.Length);
                            ms.Position = 0;

                            byte header = (byte) ms.ReadByte();
                            bool isCompress = BitUtil.GetBit(header, 0) == 1;
                            bool isEncrypt = BitUtil.GetBit(header, 1) == 1;

                            //协议编号
                            ushort protoId = ms.ReadUShort();
                            ProtoCategory protoCategory = (ProtoCategory) ms.ReadByte();

                            //定义小包体 也就是真正的protobuf协议数据
                            byte[] protoData = new byte[buffer.Length - 4];
                            Array.Copy(buffer, 4, protoData, 0, protoData.Length);
                            if (isEncrypt)
                            {
                                //解密
                                protoData = XXTEAUtil.Decrypt(protoData);
                            }

                            if (isCompress)
                            {
                                //解压
                                protoData = ZlibUtil.DeCompressBytes(protoData);
                            }

                            m_EventDispatcher.Dispatch(protoId, protoData);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    m_ReceiveCount = 0;
                    break;
                }
            }

            #endregion
        }

        //========================================================


        #region OnCheckSendQueueCallBack 检查队列的委托回调
        /// <summary>
        /// 检查队列的委托回调
        /// </summary>
        private void OnCheckSendQueueCallBack()
        {
            lock (m_SendQueue)
            {
                //如果队列中有数据包 则发送数据包
                if (m_SendQueue.Count > 0)
                {
                    //发送数据包
                    Send(m_SendQueue.Dequeue());
                }
            }
        }
        #endregion

        #region MakeData 封装数据包

        /// <summary>
        /// 封装数据包
        /// </summary>
        /// <param name="proto"></param>
        /// <returns></returns>
        private byte[] MakeData(IProto proto)
        {
            byte header = 0; //byte，1-2-3-4-5-6-7-8 |1-是否压缩|2-是否加密

            byte[] buffer = proto.ToByteArray();
            bool isCompress = buffer.Length > m_CompressLen;
            if (isCompress)
            {
                header = BitUtil.SetBit(header, 0);
                ZlibUtil.CompressBytes(buffer);
            }

            //2.加密
            header = BitUtil.SetBit(header, 1);
            buffer = XXTEAUtil.Encrypt(buffer);

            MMO_MemoryStream ms = this.m_SocketSendMS;
            ms.SetLength(0);

            ms.WriteUShort((ushort) (buffer.Length + 4)); //4=header 1 + ProtoId 2 + Category 1

            ms.WriteByte(header);
            ms.WriteUShort(proto.ProtoId);
            ms.WriteByte((byte) proto.Category);

            ms.Write(buffer, 0, buffer.Length);

            var retBuffer = ms.ToArray();
            return retBuffer;
        }

        /// <summary>
        /// 封装数据包
        /// </summary>
        /// <param name="protoId"></param>
        /// <param name="category"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private byte[] MakeData(ushort protoId, byte category, byte[] buffer)
        {
            byte header = 0; //byte，1-2-3-4-5-6-7-8 |1-是否压缩|2-是否加密

            bool isCompress = buffer.Length > m_CompressLen;
            if (isCompress)
            {
                header = BitUtil.SetBit(header, 0);
                ZlibUtil.CompressBytes(buffer);
            }

            //2.加密
            header = BitUtil.SetBit(header, 1);
            buffer = XXTEAUtil.Encrypt(buffer);

            MMO_MemoryStream ms = this.m_SocketSendMS;
            ms.SetLength(0);

            ms.WriteUShort((ushort) (buffer.Length + 4)); //4=header 1 + ProtoId 2 + Category 1

            ms.WriteByte(header);
            ms.WriteUShort(protoId);
            ms.WriteByte(category);

            ms.Write(buffer, 0, buffer.Length);

            var retBuffer = ms.ToArray();
            return retBuffer;
        }

        #endregion

        #region SendMsg 发送消息 把消息加入队列
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="proto"></param>
        public void SendMsg(IProto proto)
        {
            //得到封装后的数据包
            byte[] sendBuffer = MakeData(proto);

            lock (m_SendQueue)
            {
                //把数据包加入队列
                m_SendQueue.Enqueue(sendBuffer);

                //检查队列
                OnCheckSendQueueCallBack();
            }
        }

        public void SendMsg(byte[] protobuffer)
        {
            lock (protobuffer)
            {
                //把数据包加入队列
                m_SendQueue.Enqueue(protobuffer);

                //检查队列
                OnCheckSendQueueCallBack();
            }
        }
        
        public void SendMsg(ushort protoId, byte category, byte[] buffer)
        {
            //得到封装后的数据包
            byte[] sendBuffer = MakeData(protoId, category, buffer);

            lock (m_SendQueue)
            {
                //把数据包加入队列
                m_SendQueue.Enqueue(sendBuffer);

                //检查队列
                OnCheckSendQueueCallBack();
            }
        }
        #endregion

        #region Send 真正发送数据包到服务器
        /// <summary>
        /// 真正发送数据包到服务器
        /// </summary>
        /// <param name="buffer"></param>
        private void Send(byte[] buffer)
        {
            m_Socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallBack, m_Socket);
        }
        #endregion

        #region SendCallBack 发送数据包的回调
        /// <summary>
        /// 发送数据包的回调
        /// </summary>
        /// <param name="ar"></param>
        private void SendCallBack(IAsyncResult ar)
        {
            m_Socket.EndSend(ar);

            //继续检查队列
            OnCheckSendQueueCallBack();
        }
        #endregion

        //===================================================

        /// <summary>
        /// 连接成功
        /// </summary>
        public Action OnConnectSuccess;

        /// <summary>
        /// 连接失败
        /// </summary>
        public Action OnConnectFail;

        /// <summary>
        /// 断开连接
        /// </summary>
        public Action OnDisConnect;

        /// <summary>
        /// 处理中转协议
        /// </summary>
        public Action OnCarryProto;

        #region Connect 连接到socket服务器
        /// <summary>
        /// 连接到socket服务器
        /// </summary>
        /// <param name="ip">ip</param>
        /// <param name="port">端口号</param>
        public void Connect(string ip, int port)
        {
            //如果socket已经存在 并且处于连接中状态 则直接返回
            if (m_Socket != null && m_Socket.Connected) return;
            AddressFamily addressFamily = AddressFamily.InterNetwork;
            m_Socket = new Socket(addressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                m_Socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port), ConnectCallBack, m_Socket);

            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 连接完毕
        /// </summary>
        /// <param name="ar"></param>
        private void ConnectCallBack(IAsyncResult ar)
        {
            if (m_Socket.Connected)
            {
                OnConnectSuccess?.Invoke();
                ReceiveMsg();
            }
            else
            {
                OnConnectFail?.Invoke();
            }
            m_Socket.EndConnect(ar);
        }
        #endregion

        #region DisConnect 断开连接
        /// <summary>
        /// 断开连接
        /// </summary>
        public void DisConnect()
        {
            if (m_Socket != null && m_Socket.Connected)
            {
                m_Socket.Shutdown(SocketShutdown.Both);
                m_Socket.Close();
            }
        }
        #endregion
    }
}
