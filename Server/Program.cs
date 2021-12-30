
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Program
    {
        private static Socket socket;
        private static List<Socket> sockets;
        static void Main(string[] args)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345));
            socket.Listen(10);
            sockets = new List<Socket>();
            while (true)
            {
                var sck = socket.Accept();
                sockets.Add(sck);
                Console.WriteLine("Connection established from {0}",sck.RemoteEndPoint.ToString());

                Thread th = new Thread(new ParameterizedThreadStart(HandleReceive));
                Thread th1 = new Thread(new ParameterizedThreadStart(HandleSend));
                th.Start(sck);
                th1.Start(sck);
            }
        }

        private static void HandleSend(object obj)
        {
            Socket sck = (Socket)obj;
            while (true)
            {
                string message = Console.ReadLine();
                byte[] buffer = Encoding.UTF8.GetBytes(message);
             
                foreach (var item in sockets)
                {
                    item.Send(buffer, 0, buffer.Length, 0);
                }
            }

        }
        
        private static void HandleReceive(object obj)
        {
            Socket clientSck = (Socket)obj;
            try
            {
                while (true)
                {     
                    byte[] buffer = new byte[1024];
                    int rec = clientSck.Receive(buffer, 0, buffer.Length, 0);
                    Array.Resize<byte>(ref buffer, rec);                   
                    string message = Encoding.UTF8.GetString(buffer);                  
                    Console.WriteLine("{0} : {1}",clientSck.RemoteEndPoint,message);                 
                }
                
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
                var result = sockets.Find(x => x.RemoteEndPoint == clientSck.RemoteEndPoint);
                sockets.Remove(result);
                result.Close();
                
            }
        }
    }
}
