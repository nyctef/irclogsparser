using System;
using System.Text.RegularExpressions;

namespace irclogsparser
{
    internal class KickedMessage : LogMessage
    {
        public string KickedPerson { get { return Speaker; } }

        private static readonly Regex regex = new Regex(@"^(\d\d):(\d\d) -!- (.*?) was kicked from [^ ]+ by [^ ]+ \[(.*)\]$", RegexOptions.Compiled);

        public KickedMessage(DateTime dateTime, string kickedPerson, string kickmessage)
            :base(dateTime, kickedPerson, kickmessage)
        {
        }

        public static bool TryCreate(string input, DateTime currentTime, out KickedMessage message)
        {
            var match = regex.Match(input);
            if (match.Success)
            {
                var hours = int.Parse(match.Groups[1].Value);
                var minutes = int.Parse(match.Groups[2].Value);
                var time = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, hours, minutes, 0);
                var speaker = match.Groups[3].Value;
                var text = match.Groups[4].Value;
                message = new KickedMessage(time, speaker, text);
                return true;
            }
            message = null;
            return false;
        }
    }
}