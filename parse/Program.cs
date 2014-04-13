using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using irclogsparser;
using System.IO;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System.Threading;

namespace parse
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new LogParser();
            var messages = parser.Parse(File.ReadAllText(args[0])).ToList();
            Console.WriteLine("parsed " + messages.Count() + " messages");

            if (args[1] == "kicks")
            {
                ListKicks(messages);
                return;
            }

            if (args[1] == "pushtobus")
            {
                PushToBus(args, messages);
            }
        }

        private static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }

        private static void PushToBus(string[] args, List<LogMessage> messages)
        {
            var connectionString = args[2];
            var topic = args[3];
            var server = args[4];
            var room = args[5];

            var jsonMessages = messages.Select(x => JsonConvert.SerializeObject(new
            {
                timestamp = x.DateTime.ToString("o"),
                message = x.Message,
                speaker = x.Speaker,
                room = room,
                server = server,
            })).ToList();

            var client = TopicClient.CreateFromConnectionString(connectionString, topic);

            foreach (var jsonMessage in jsonMessages)
            {
                while (true)
                {
                    try
                    {
                        BrokeredMessage brokerMessage;
                        using (var stream = GenerateStreamFromString(jsonMessage))
                        {
                            brokerMessage = new BrokeredMessage(stream);

                            client.Send(brokerMessage);
                        }
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e);
                        Thread.Sleep(5000);
                    }
                }
            }
        }

        private static void ListKicks(List<LogMessage> messages)
        {
            foreach (var kick in messages.OfType<KickedMessage>())
            {
                Console.WriteLine(kick);
            }
        }
    }
}
