using Common;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;


namespace CommonClient
{
    public class ClientSocket
    {
        
        private static Socket clientSocket = null;
        //是否已连接的标识  
        public bool IsConnected = false;

        public ClientSocket()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>  
        /// 连接指定IP和端口的服务器  
        /// </summary>  
        /// <param name="ip"></param>  
        /// <param name="port"></param>  
        public void ConnectServer(string ip, int port)
        {
            IPAddress mIp = IPAddress.Parse(ip);
            IPEndPoint ip_end_point = new IPEndPoint(mIp, port);

            try
            {
                clientSocket.Connect(ip_end_point);
                IsConnected = true;
            }
            catch
            {
                IsConnected = false;
                return;
            }
        }

        private void ReciveMessage()
        {
            while (true)
            {
                byte[] result = new byte[1024];
                int receiveLength = clientSocket.Receive(result);
                //ByteBuffer buffer = new ByteBuffer(result);
                //int len = buffer.ReadShort();
                //string data = buffer.ReadString();
                ClientManager.Instance.C2B(ByteToObject.Bytes2Object(result));
            }
        }

        /// <summary>  
        /// 发送数据给服务器  
        /// </summary>  
        public void SendMessage(object obj)
        {
            if (IsConnected == false)
                return;
            try
            {
                clientSocket.Send(ByteToObject.Object2Bytes(obj));
            }
            catch
            {
                IsConnected = false;
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
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

        public void Dispose()
        {
            if (clientSocket != null)
            {
                IsConnected = false;
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
        }

    }
}
