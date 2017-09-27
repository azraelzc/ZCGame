using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using Common;
using System.Collections.Generic;

namespace CommonServer
{
    public class ServerSocket
    {
        private static byte[] result = new byte[1024];
        private const int port = 8088;
        private static string IpStr = "127.0.0.1";
        private static Socket serverSocket;

        private const int mUpdateInterval = 33;
        public void Init()
        {
            IPAddress ip = IPAddress.Parse(IpStr);
            IPEndPoint ipEndPoint = new IPEndPoint(ip, port);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ipEndPoint);
            serverSocket.Listen(10);
            Console.WriteLine("启动监听{0}成功", serverSocket.LocalEndPoint.ToString());
            Thread thread = new Thread(ClientConnectListen);
            thread.Start();
            ServerManager.Instance.OnSendEvent += SendMessage;
            Timer timer = new Timer((obj) => {
                Update();
            }, null, mUpdateInterval, mUpdateInterval);
            Console.ReadLine();
        }

        /// <summary>  
        /// 客户端连接请求监听  
        /// </summary>  
        private void ClientConnectListen()
        {
            while (true)
            {
                //为新的客户端连接创建一个Socket对象  
                Socket clientSocket = serverSocket.Accept();
                Console.WriteLine("客户端{0} ,{1}成功连接", clientSocket.RemoteEndPoint.ToString(), clientSocket.RemoteEndPoint.GetHashCode());
                ServerManager.Instance.AddClient(clientSocket);
                //向连接的客户端发送连接成功的数据  
                ServerManager.Instance.C2B(clientSocket.GetHashCode(),CommonNet.NetState.CONNECTED);
                //每个客户端连接创建一个线程来接受该客户端发送的消息  
                Thread thread = new Thread(RecieveMessage);
                thread.Start(clientSocket);
            }
        }

        public void SendMessage(Socket clientSocket,object obj)
        {
            //ByteBuffer buffer = new ByteBuffer();
            //buffer.WriteString(obj.ToString());
            //clientSocket.Send(WriteMessage(buffer.ToBytes()));
            clientSocket.Send(ByteToObject.Object2Bytes(obj));
        }

        /// <summary>  
        /// 数据转换，网络发送需要两部分数据，一是数据长度，二是主体数据  
        /// </summary>  
        /// <param name="message"></param>  
        /// <returns></returns>  
        private byte[] WriteMessage(byte[] message)
        {
            MemoryStream ms = null;
            using (ms = new MemoryStream())
            {
                ms.Position = 0;
                BinaryWriter writer = new BinaryWriter(ms);
                ushort msglen = (ushort)message.Length;
                writer.Write(msglen);
                writer.Write(message);
                writer.Flush();
                return ms.ToArray();
            }
        }

        /// <summary>  
        /// 接收指定客户端Socket的消息  
        /// </summary>  
        /// <param name="clientSocket"></param>  
        private void RecieveMessage(object clientSocket)
        {
            Socket mClientSocket = (Socket)clientSocket;
            while (true)
            {
                try
                {
                    byte[] result = new byte[1024];
                    int receiveNumber = mClientSocket.Receive(result);
                    //Console.WriteLine("接收客户端{0},{1}消息， 长度为{2}", mClientSocket.RemoteEndPoint.ToString(), mClientSocket.GetHashCode(), receiveNumber);
                    //ByteBuffer buff = new ByteBuffer(result);
                    ////数据长度  
                    //int len = buff.ReadShort();
                    ////数据内容  
                    //string data = buff.ReadString();
                    
                    ServerManager.Instance.B2C(mClientSocket.GetHashCode(),ByteToObject.Bytes2Object(result));
                    //Console.WriteLine("数据内容：{0}", data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
            }
        }

        //断开客户端连接
        public void Close(Socket mClientSocket)
        {
            Console.WriteLine("===========ShutDownClient===========");
            if (ServerManager.Instance.HasSocket(mClientSocket.GetHashCode()))
            {
                Console.WriteLine("客户端{0} ,{1}断开连接", mClientSocket.RemoteEndPoint.ToString(), mClientSocket.RemoteEndPoint.GetHashCode());
                mClientSocket.Close();
                ServerManager.Instance.RemoveClient(mClientSocket);
            }
        }

        //帧同步更新协议
        public void Update()
        {
            ServerManager.Instance.Update(mUpdateInterval);
        }
    }

    
}

