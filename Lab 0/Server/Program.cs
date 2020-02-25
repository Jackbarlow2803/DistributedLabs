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
            Console.WriteLine("Listening at port " + port + "...");
            do
            {
                TcpClient tcpClient = server.AcceptTcpClient();
                NetworkStream nStream = tcpClient.GetStream();
                byte[] header = new byte[6]; nStream.Read(header, 0, 6);
                int messageType = header[3];
                int remainingLength = header[4] * 256 + header[5];
                Console.WriteLine("Message type: " + messageType);
                Console.WriteLine("Length: " + remainingLength);
                byte[] rawData = new byte[remainingLength];
                nStream.Read(rawData, 0, remainingLength);
                int offset = rawData[0];
                int dataLength = rawData[1] * 256 + rawData[2];
                byte[] byteMessage = new byte[remainingLength];
                Array.Copy(rawData, 3, byteMessage, 0, dataLength);
                string Message = System.Text.Encoding.ASCII.GetString(rawData, 3, dataLength);
                Console.WriteLine("Encryption key: " + offset);
                Console.WriteLine("Data length: " + dataLength);
                Console.WriteLine("Received message:");
                Console.WriteLine(Message);

                for (int i = 0; i < Message.Length; i++)
                {
                    byteMessage[i] = (byte)((int)byteMessage[i] + (int)offset); if
                   (byteMessage[i] > 126)
                    {
                        byteMessage[i] = (byte)((int)byteMessage[i] - 126 + 31);
                    }
                }
                string EncryptedMessage = System.Text.Encoding.ASCII.GetString(byteMessage, 0,
                dataLength);
                Console.WriteLine("Encrypted message:");
                Console.WriteLine(EncryptedMessage);
            }
            while (true);
        }
    }
}
