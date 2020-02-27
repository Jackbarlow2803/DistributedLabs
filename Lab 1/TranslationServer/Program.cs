using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;


namespace TranslationServer
{
    internal static class Program
    {
        private const ushort DefaultPort = 5002;

        private static int Main(String[] args)
        {
            TcpChannel channel = new TcpChannel(Program.DefaultPort); //Channels are objects that transport messages between apps across remoting boundaries - can listen on ean endpoint for inbound/outbound messages
            ChannelServices.RegisterChannel(channel, false); //Register the channel with the operating system
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(Translator), "Translate", WellKnownObjectMode.SingleCall); //Tells the remoting service which methods are available over the established channel
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
            return 0;
        }
    }
}
