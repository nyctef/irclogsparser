using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace irclogsparser
{
    internal class DayChangedMessage
    {
        private readonly DateTime time;
        private static readonly Regex regex = new Regex(@"^--- Day changed (.*)$", RegexOptions.Compiled);

        public DayChangedMessage(DateTime time)
        {
            this.time = time;
        }

        public DateTime Time { get { return time; } }

        public static bool TryCreate(string input, out DayChangedMessage message)
        {
            var match = regex.Match(input);
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