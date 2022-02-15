using DST.Common.Helper;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DST.PIMS.Framework
{
    public class SocketServer
    {
        public delegate void SocketServerDeletegate(string message);
        public event SocketServerDeletegate ReceiveMessage;

        private bool isClose = false;
        private Socket socketWatch;
        private Socket socketSend;
        private Dictionary<string, Socket> dicSocket = new Dictionary<string, Socket>();

        public static SocketServer Instance { get; } = new SocketServer();

        private SocketServer()
        {
        }

        /// <summary>
        /// 启动监听
        /// </summary>
        /// <returns></returns>
        public bool StartListen()
        {
            bool result = true;
            try
            {
                string path = ExtendAppContext.Current.ConfigurationIniPath;
                string tmpPort = IniHelper.CreateInstance(path).IniReadValue(IniSectionConst.CommonConfig, "SocketPort", "7523");
                int port = 7523;
                int.TryParse(tmpPort, out port);

                string ip = IniHelper.CreateInstance(path).IniReadValue(IniSectionConst.CommonConfig, "SocketServerIP", this.GetLocalIp().ToString());

                this.socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint point = new IPEndPoint(IPAddress.Parse(ip), port);
                this.socketWatch.Bind(point);
                this.socketWatch.Listen(20);

                Thread th = new Thread(this.Accept);
                th.IsBackground = true;
                th.Start(this.socketWatch);
            }
            catch (Exception ex)
            {
                result = false;
                Logger.Error("SocketServer启动监听失败：" + ex.Message);
            }

            return result;
        }

        public void StopListen()
        {
            this.isClose = true;

            if(this.socketWatch != null)
            {
                this.socketWatch.Close();
                this.socketWatch.Dispose();
                this.socketWatch = null;
            }

            if(this.socketSend != null)
            {
                this.socketSend.Close();
                this.socketSend.Dispose();
                this.socketSend = null;
            }

            if(this.dicSocket != null)
            {
                this.dicSocket.Clear();
                this.dicSocket = null;
            }
        }

        /// <summary>
        /// 监听socket 接收到消息
        /// </summary>
        /// <param name="socketObj"></param>
        private void Accept(object socketObj)
        {
            try
            {
                Socket socketWatch = socketObj as Socket;
                while (true)
                {
                    socketSend = socketWatch.Accept();
                    dicSocket.Add(socketSend.RemoteEndPoint.ToString(), socketSend);

                    Thread th = new Thread(this.Receive);
                    th.IsBackground = true;
                    th.Start();

                    if(this.isClose)
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 实际处理接收到的数据
        /// </summary>
        private void Receive()
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[1024 * 1024 * 5];
                    int r = socketSend.Receive(buffer);
                    if (r == 0)
                    {
                        break;
                    }

                    string strMsg = Encoding.UTF8.GetString(buffer, 0, r);
                    if(null != this.ReceiveMessage)
                    {
                        this.ReceiveMessage(strMsg);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 发送消息到客户端
        /// </summary>
        /// <param name="clientSocket">客户端socket</param>
        /// <param name="msg"></param>
        public void SendMsgToClient(Socket clientSocket, string msg)
        {
            if(clientSocket != null && !string.IsNullOrEmpty(msg))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(msg);
                clientSocket.Send(buffer);
            }
        }


        /// <summary>
        /// 获取当前IP，不能是127.0.0.1
        /// </summary>
        /// <returns></returns>
        public IPAddress GetLocalIp()
        {
            try
            {
                IPAddress[] ipArray;
                ipArray = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
                IPAddress localIp = ipArray.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
                return localIp;
            }
            catch (Exception ex)
            {
                Logger.Error("获取本地IP失败：" + ex.Message);
            }

            return null;
        }
    }
}
