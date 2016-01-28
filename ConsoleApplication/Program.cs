using System;
using SystemOut.SmartInspect.Extensions;
using Gurock.SmartInspect;

namespace ConsoleApplication
{
    class Program
    {
        private static string host = "";
        private static string from = "";
        private static string to = "";
        private static int port;

        static void Main(string[] args)
        {
            ProtocolFactory.RegisterProtocol("mail", typeof(MailProtocol));
            var builder = new ConnectionsBuilder();
            builder.BeginProtocol("mail");
            builder.AddOption("host", host);
            builder.AddOption("to", to);
            builder.AddOption("from", from);
            builder.AddOption("port", port);
            builder.EndProtocol();
            
            SiAuto.Si.Connections = builder.Connections;
            SiAuto.Si.Enabled = true;

            SiAuto.Main.LogMessage("Debug", Level.Debug);
            SiAuto.Main.LogMessage("Error", Level.Error);
            SiAuto.Main.LogMessage("Fatal", Level.Fatal);


            Console.ReadLine();
        }
    }
}
