using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace Server
{
    internal static class Program
    {
        private const int Length = 256;
        private const ushort Port = 5002;

        private static bool Listening = true;

        private static void HandleExit(Object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Received signal interrupt");
            Program.Listening = false;
            e.Cancel = true;
        }

        private static void Init(TcpListener server)
        {
            try
            {
                TcpClient client = server.AcceptTcpClient();
                byte[] header = new byte[6], rawData;
                int remainingLength;

                using (NetworkStream ns = client.GetStream())
                {
                    ns.Read(header, 0, 6);
                    int messageType = header[3];
                    remainingLength = header[4] * Program.Length + header[5];
                    Console.WriteLine("Received message with type: " + messageType + " and length: " + remainingLength);
                    rawData = new byte[remainingLength];
                    ns.Read(rawData, 0, remainingLength);
                }

                int offset = rawData[0];
                int dataLength = rawData[1] * Program.Length + rawData[2];
                byte[] message = new byte[remainingLength];
                Array.Copy(rawData, 3, message, 0, dataLength);
                String decodedMessage = Encoding.ASCII.GetString(rawData, 3, dataLength);
                Console.WriteLine("Encryption key: " + offset);
                Console.WriteLine("Data length: " + dataLength);
                Console.WriteLine("Decrypted message: " + decodedMessage);

                for (int i = 0; i < decodedMessage.Length; i++)
                {
                    if ((message[i] = (byte)((int)message[i] + offset)) > 126)
                    {
                        message[i] = (byte)((int)message[i] - 126 + 31);
                    }
                }

                String encodedMessage = Encoding.ASCII.GetString(message, 0, dataLength);
                Console.WriteLine("Encrypted message: " + encodedMessage);
            }

            catch
            {
                return;
            }
        }

        private static int Main(String[] args)
        {
            Console.CancelKeyPress += new ConsoleCancelEventHandler(Program.HandleExit);
            TcpListener server = new TcpListener(IPAddress.Any, Program.Port);
            server.Start();
            Console.WriteLine("Server now listening at port " + Program.Port);

            do
            {
                Program.Init(server);
            } while (Program.Listening);

            server.Stop();
            Console.WriteLine("Server shutdown successfully");
            return 0;
        }
    }
}