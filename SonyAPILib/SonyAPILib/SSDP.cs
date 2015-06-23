//Dr Gadgit from the Code project http://www.codeproject.com/Articles/893791/DLNA-made-easy-and-Play-To-for-any-device
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace SonyAPILib
{
    //This class is used to broadcast a SSDP message using UDP on port 1900 and to then wait for any replies send back on the LAN
    /// <summary>
    /// SSDP Clas for sending Broadcast for DLNA/UPnP devices available on the network.
    /// </summary>
    public static class SSDP
    {
        private static Socket UdpSocket = null;

        /// <summary>
        /// Provides a List of Description Document URL's for each device loacted
        /// </summary>
        public static List<string> Servers = new List<string>();
        private static string NewServer = "";
        private static Thread THSend = null;
        private static bool Running = false;

        /// <summary>
        /// Starts the Execution of the SSDP Broadcast
        /// </summary>
        public static void Start()
        {//Stop should be called in about 12 seconds which will kill the thread
            if (Running) return;
            Running = true;
            NewServer = "";
            Thread THSend = new Thread(SendRequest);
            THSend.Start();
            Thread TH = new Thread(Stop);
            TH.Start();
        }

        /// <summary>
        /// Stops the Execution of the SSDP Broadcast
        /// </summary>
        public static void Stop()
        {//OK time is up so lets return our DLNA server list
            Thread.Sleep(9000);
            Running = false;
            try
            {
                Thread.Sleep(1000);
                if (UdpSocket != null)
                    UdpSocket.Close();
                if (THSend != null)
                    THSend.Abort();
            }
            catch { ;}
            if (NewServer.Length > 0) Servers.Add(NewServer.Trim());//Bank in our new servers
        }

        private static void SendRequest()
        {
            try { SendRequestNow(); }
            catch { ;}
        }

        private static void SendRequestNow()
        {//Uses UDP Multicast on 239.255.255.250 with port 1900 to send out invitations that are slow to be answered
            IPEndPoint LocalEndPoint = new IPEndPoint(IPAddress.Any, 6000);
            IPEndPoint MulticastEndPoint = new IPEndPoint(IPAddress.Parse("239.255.255.250"), 1900);//SSDP port
            Socket UdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            UdpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            UdpSocket.Bind(LocalEndPoint);
            UdpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(MulticastEndPoint.Address, IPAddress.Any));
            UdpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 2);
            UdpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastLoopback, true);
            string SearchString = "M-SEARCH * HTTP/1.1\r\nHOST:239.255.255.250:1900\r\nMAN:\"ssdp:discover\"\r\nST:ssdp:all\r\nMX:3\r\n\r\n";
            UdpSocket.SendTo(Encoding.UTF8.GetBytes(SearchString), SocketFlags.None, MulticastEndPoint);
            byte[] ReceiveBuffer = new byte[4000];
            int ReceivedBytes = 0;
            int Count = 0;
            while (Running && Count < 100)
            {//Keep loopping until we timeout or stop is called but do wait for at least ten seconds 
                Count++;
                if (UdpSocket.Available > 0)
                {
                    ReceivedBytes = UdpSocket.Receive(ReceiveBuffer, SocketFlags.None);
                    if (ReceivedBytes > 0)
                    {
                        string Data = Encoding.UTF8.GetString(ReceiveBuffer, 0, ReceivedBytes);
                        if (Data.ToUpper().IndexOf("LOCATION: ") > -1)
                        {//ChopOffAfter is an extended string method added in Helper.cs
                            Data = Data.ChopOffBefore("LOCATION: ").ChopOffAfter(Environment.NewLine);
                            if (Servers.IndexOf(Data.ToLower()) == -1)
                                Servers.Add(Data.Trim());
                        }
                    }
                }
                else
                    Thread.Sleep(100);
            }
            if (NewServer.Length > 0) Servers.Add(NewServer.Trim());//Bank in our new servers nice and quick with minute risk of thread error due to not locking
            UdpSocket.Close();
            THSend = null;
            UdpSocket = null;
        }
    }
}

