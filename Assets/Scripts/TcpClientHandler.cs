using UnityEngine;
using System.Threading;
using System.Net.Sockets;
using System;
using System.Text;

public class TcpClientHandler : MonoBehaviour
{

    public string hostName = "localhost";
    public int port = 4445;

    private TcpClient m_client;
    private Thread clientThread;
    
    private void Start()
    {
        ConnectToServer();
    }

    public void ConnectToServer()
    {
        try
        {
            clientThread = new Thread(new ThreadStart(ReceiveData));
            clientThread.IsBackground = true;
            clientThread.Start();
        }
        catch (Exception e)
        {
            Debug.Log("Exception: " + e);
        }
    }

    private void ReceiveData()
    {
        try
        {
            m_client = new TcpClient(hostName, port);
            byte[] bytes = new byte[1024];
            using (NetworkStream stream = m_client.GetStream())
            {
                int length;
                while (true)
                {
                    if ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var data = new byte[length];
                        Array.Copy(bytes, 0, data, 0, length);
                        string serverMessage = Encoding.ASCII.GetString(data);
                        Debug.Log("Received message: " + serverMessage);
                    }
                    else
                    {
                        break;
                    }
                }
            } 
        }
        catch(SocketException e)
        {
            Debug.Log("Socket Exception: " + e);
        }
    }
}
