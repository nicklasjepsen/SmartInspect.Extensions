using System;
using System.Net.Mail;
using Gurock.SmartInspect;

namespace SystemOut.SmartInspect.Extensions
{
    public class MailProtocol : Protocol
    {
        private SmtpClient smtpClient;
        private string host;
        private int port;
        private string from;
        private string to;

        protected override void InternalConnect()
        {
            smtpClient = new SmtpClient(host, port);
        }

        protected override void InternalDisconnect()
        {
            smtpClient.Dispose();
        }

        private MailMessage FormatLogEntry(LogEntry logEntry)
        {
            return new MailMessage(from, to)
            {
                Subject = logEntry.Title,
                Body = $"{logEntry.Level}{Environment.NewLine}{logEntry.Title}"
            };
        }

        protected override void InternalWritePacket(Packet packet)
        {
            switch (packet.PacketType)
            {
                case PacketType.LogEntry:
                    {
                        var logEntry = packet as LogEntry;
                        // TODO: I think there is a bug in SI - level is always set to Message
                        // Only send if level is severe than error and logEntry is not null
                        //if (logEntry?.Level > Level.Error)
                        //{
                        var message = FormatLogEntry(logEntry);
                        smtpClient.Send(message);
                        break;
                    }
            }
        }

        protected override bool IsValidOption(string name)
        {
            switch (name)
            {
                case "host":
                    return true;
                case "port":
                    return true;
                case "to":
                    return true;
                case "from":
                    return true;
                default:
                    return base.IsValidOption(name);
            }
        }

        protected override void LoadOptions()
        {
            base.LoadOptions();
            host = GetStringOption("host", null);
            port = GetIntegerOption("port", 25);
            to = GetStringOption("to", null);
            from = GetStringOption("from", null);
        }

        protected override string Name => "mail";
    }
}
