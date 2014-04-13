using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using irclogsparser;
using System.IO;

namespace parse
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new LogParser();
            var messages = parser.Parse(File.ReadAllText(args[0])).ToList();
            Console.WriteLine("parsed " + messages.Count() + " messages");
            foreach (var kick in messages.OfType<KickedMessage>())
            {
                Console.WriteLine(kick);
            }
        }
    }
}
