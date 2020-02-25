using System;
using System.Net.Sockets;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (TcpClient client = new TcpClient("localhost", 5000))
            {

            }
        }
    }
}
