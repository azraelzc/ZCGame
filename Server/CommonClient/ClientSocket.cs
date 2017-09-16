using Azrael.Common;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class ClientSocket
{
    private static byte[] result = new byte[1024];
    private static Socket clientSocket = null;
    //是否已连接的标识  
    public bool IsConnected = false;

    private Queue<string> sendMessageQueue = new Queue<string>();

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
        //服务器下发数据长度  
        int receiveLength = clientSocket.Receive(result);
        ByteBuffer buffer = new ByteBuffer(result);
        int len = buffer.ReadShort();
        string data = buffer.ReadString();
    }

    private void ReciveMessage()
    {
        while (true)
        {
            int receiveLength = clientSocket.Receive(result);
            ByteBuffer buffer = new ByteBuffer(result);
            int len = buffer.ReadShort();
            string data = buffer.ReadString();
            //reciveMessageQueue.Enqueue(data);
        }
    }

    public void SendToPool(string msg)
    {
        sendMessageQueue.Enqueue(msg);
    }

    /// <summary>  
    /// 发送数据给服务器  
    /// </summary>  
    public void SendMessage(string data)
    {
        if (IsConnected == false)
            return;
        try
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteString(data);
            clientSocket.Send(WriteMessage(buffer.ToBytes()));
        }
        catch
        {
            IsConnected = false;
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
    }

    public void Update()
    {
        while (sendMessageQueue.Count > 0)
        {
            SendMessage(sendMessageQueue.Dequeue());
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
