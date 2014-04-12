using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace irclogsparser
{
    class LogMessage : IEquatable<LogMessage>
    {
        private DateTime dateTime;
        private string speaker;
        private string message;

        public string Speaker { get { return speaker; } }

        public LogMessage(DateTime dateTime, string speaker, string message)
        {
            this.dateTime = dateTime;
            this.speaker = speaker;
            this.message = message;
        }

        public bool Equals(LogMessage other)
        {
            return this.dateTime == other.dateTime &&
                this.speaker == other.speaker &&
                this.message == other.message;
        }

        public override bool Equals(object obj)
        {
            return ((var other = obj as LogMessage) != null) && Equals(other);
        }

        public override int GetHashCode()
        {
            return speaker.GetHashCode() + message.GetHashCode() + dateTime.GetHashCode();
        }

        public override string ToString()
        {
            return this.dateTime.ToString() + '/' + this.speaker + '/' + this.message;
        }

        public static bool TryCreate(string input, DateTime currentTime, out LogMessage message) 
        {
            var match = new Regex(@"^(\d\d):(\d\d) <[ &]([^>]+)> (.*)$").Match(input);
            if (match.Success)
            {
                var hours = int.Parse(match.Groups[1].Value);
                var minutes = int.Parse(match.Groups[2].Value);
                var time = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, hours, minutes, 0);
                var speaker = match.Groups[3].Value;
                var text = match.Groups[4].Value;
                message = new LogMessage(time, speaker, text);
                return true;
            }
            message = null;
            return false;
        }

        public static bool TryCreateDelayed(string input, out LogMessage message)
        {
            var match = new Regex(@"^\d\d:\d\d \[(\d{4})-(\d\d)-(\d\d) (\d\d):(\d\d)\] <[ &]([^>]+)> (.*)$").Match(input);
            if (match.Success)
            {
                var year = int.Parse(match.Groups[1].Value);
                var month = int.Parse(match.Groups[2].Value);
                var day = int.Parse(match.Groups[3].Value);
                var hours = int.Parse(match.Groups[4].Value);
                var minutes = int.Parse(match.Groups[5].Value);
                var time = new DateTime(year, month, day, hours, minutes, 0);
                var speaker = match.Groups[6].Value;
                var text = match.Groups[7].Value;
                message = new LogMessage(time, speaker, text);
                return true;
            }
            message = null;
            return false;
        }
    }
}
