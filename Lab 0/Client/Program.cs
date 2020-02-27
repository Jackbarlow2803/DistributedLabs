using System;
using System.Net.Sockets;
using System.Text;


namespace Client
{
    internal static class Program
    {
        private const String Hostname = "localhost";
        private const int Length = 256;
        private const ushort Port = 5002;

        private static int Main(String[] args)
        {
            Console.Write("Please enter a message: ");
            String message = Console.ReadLine();

            if (message.Length > Program.Length)
            {
                message = message.Substring(0, Program.Length);
            }

            int encryptionKey = 1;
            byte[] rawData = Program.Serialize(message, encryptionKey);

            using (TcpClient client = new TcpClient())
            {
                client.Connect(Program.Hostname, Program.Port);

                using (NetworkStream ns = client.GetStream())
                {
                    ns.Write(rawData, 0, rawData.Length);
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
            return 0;
        }

        private static byte[] Serialize(String message, int encryptionKey)
        {
            int index = 0;
            byte[] ascii = Encoding.ASCII.GetBytes(message);
            int messageLength = ascii.Length + 9;
            byte[] rawData = new byte[messageLength];
            rawData[index++] = 0;
            rawData[index++] = 0;
            rawData[index++] = 0;
            rawData[index++] = 1;
            int remainingLength = ascii.Length + 3;
            rawData[index++] = (byte)((remainingLength & 0xFF00) >> 8);
            rawData[index++] = (byte)(remainingLength & 0xFF);
            rawData[index++] = (byte)encryptionKey;
            int bodyLength = ascii.Length;
            rawData[index++] = (byte)((bodyLength & 0xFF00) >> 8);
            rawData[index++] = (byte)(bodyLength & 0xFF);
            Array.Copy(ascii, 0, rawData, index, ascii.Length);
            return rawData;
        }
    }
}