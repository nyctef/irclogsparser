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
            }
            else if (args[1] == "pushtobus")
            {
                PushToBus(args, messages);
            }
            else if (args[1] == "deowls")
            {
                PrintDeowls(messages);
            }
            else if (args[1] == "pushdeowls")
            {
                PushDeowls(args, messages);
            }
            else if (args[1] == "dumpmessagetext")
            {
                DumpMessages(messages);
            }
        }

        private static void DumpMessages(List<LogMessage> messages)
        {
            var output = new StringBuilder();
            foreach (var message in messages.Where(m => m.GetType() == typeof(LogMessage)))
            {
                output.AppendLine(message.Message);
            }
            File.WriteAllText("output.txt", output.ToString());
        }

        private static void PushDeowls(string[] args, List<LogMessage> messages)
        {
            var connectionString = args[2];
            var topic = args[3];
            var server = args[4];
            var room = args[5];

            var deowls = GetDeowls(messages);

            var jsonMessages = deowls.Select(x => JsonConvert.SerializeObject(new
            {
                speaker = x.Name,
                deowl = true,
                success = x.Success,
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
            };
        }

        private static void PrintDeowls(List<LogMessage> messages)
        {
            IEnumerable<Deowl> deowls = GetDeowls(messages);
            foreach (var deowl in deowls)
            {
                Console.WriteLine("{0} {1}", deowl.Name, deowl.Success);
            }
        }

        private static IEnumerable<Deowl> GetDeowls(List<LogMessage> messages)
        {
            var tracker = new DeowlTracker();
            var deowls = tracker.Run(messages);
            return deowls;
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
