using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace Server
{
    class Program
    {
        static void Main()
        {
            TcpListener server = null;
            int port = 5000;
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            
        }
    }
}
