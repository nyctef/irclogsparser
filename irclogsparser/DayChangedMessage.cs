using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace irclogsparser
{
    internal class DayChangedMessage
    {
        private readonly DateTime time;

        public DayChangedMessage(DateTime time)
        {
            this.time = time;
        }

        public DateTime Time { get { return time; } }

        public static bool TryCreate(string input, out DayChangedMessage message)
        {
            var match = new Regex(@"^--- Day changed (.*)$").Match(input);
            if (match.Success && DateTime.TryParseExact(match.Groups[1].Value, "ddd MMM dd yyyy", null, DateTimeStyles.None, out var time))
            {
                message = new DayChangedMessage(time);
                return true;
            }
            message = null;
            return false;
        }
    }
}