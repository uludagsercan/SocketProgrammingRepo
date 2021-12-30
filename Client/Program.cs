using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;


namespace Client
{
    class Program
    {
        private static Socket socket;
        static void Main(string[] args)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            try
            {
                socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345));
                while (true)
                {
                    
                    Thread th = new Thread(new ParameterizedThreadStart(HandleReceive));
                    Thread th1 = new Thread(new ParameterizedThreadStart(HandleSend));
                    th.Start();
                    th1.Start();
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void HandleSend(object obj)
        {
          
            while (true)
            {               
                string message = Console.ReadLine();
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                socket.Send(buffer, 0, buffer.Length, 0);
            }
        }

        private static void HandleReceive(object obj)
        {
           
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[1024];
                    int rec = socket.Receive(buffer, 0, buffer.Length, 0);
                    Array.Resize<byte>(ref buffer, rec);
                    string message = Encoding.UTF8.GetString(buffer);
                    Console.WriteLine("{0} : {1}",socket.RemoteEndPoint,message);
                }
                
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
