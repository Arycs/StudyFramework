using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{


    public class TestSocket : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.A))
            {
                GameEntry.Socket.ConnectToMainSocket("127.0.0.1",8001);
            }
            if (Input.GetKeyUp(KeyCode.B))
            {
                //发送消息 , 格式如下, 声明协议,发送协议的字节流过去
                //XXXProto proto = new XXXProto();
                //GameEntry.Socket.SendMsg(proto.ToArray());
            }
        }
    }
}