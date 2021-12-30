using Business.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ServerManager : IServerService
    {
        public void HandleClient(Socket socket)
        {
            socket.Accept();
            Thread th = new Thread(Receive);
            th.Start(socket);
        }

        private void Receive(object obj)
        {
            Socket sck = (Socket)obj;
            byte[] buffer = new byte[1024];
            int rec = sck.Receive(buffer, 0, buffer.Length, 0);
            Array.Resize<byte>(ref buffer, rec);
            string message = Encoding.UTF8.GetString(buffer);
        }

        public void StartListener()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345));
            socket.Listen(10);
            while (true)
            {
                HandleClient(socket);
            }
        }
    }
}
