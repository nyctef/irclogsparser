using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace irclogsparser
{
    internal class LogStartedMessage
    {
        private readonly DateTime time;

        public LogStartedMessage(DateTime time)
        {
            this.time = time;
        }

        public DateTime Time { get { return time; } }

        public static bool TryCreate(string input, out LogStartedMessage message)
        {
            var match = new Regex(@"^--- Log opened (.*)$").Match(input);
            if (match.Success && DateTime.TryParseExact(match.Groups[1].Value, "ddd MMM dd hh:mm:ss yyyy", null, DateTimeStyles.None, out var time))
            {
                message = new LogStartedMessage(time);
                return true;
            }
            message = null;
            return false;
        }
    }
}