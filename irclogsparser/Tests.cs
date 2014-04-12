using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace irclogsparser
{
    public class Tests
    {
        [Fact]
        public void CanParseTheSimplestLog()
        {
            var logFile =
@"--- Log opened Tue Dec 24 00:26:23 2013
00:35 < Nyctef> I said-a hey
00:35 < Nyctef> what's goin on";

            var expected = new List<LogMessage> 
            {
                new LogMessage(new DateTime(2013, 12, 24, 0, 35, 0, 0), "Nyctef", "I said-a hey"),
                new LogMessage(new DateTime(2013, 12, 24, 0, 35, 0, 0), "Nyctef", "what's goin on"),
            };
            var actual = new LogParser().Parse(logFile).ToList();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanParseMessageFromMod()
        {
            var logFile = @"00:58 <&Rainbow Dash> NEVER FORGET";

            var logMessage = new LogParser().Parse(logFile).Single();
            Assert.Equal("Rainbow Dash", logMessage.Speaker);
        }

        [Fact]
        public void CanParseDelayedMessage()
        {
            var logFile = @"00:26 [2013-12-24 00:15] < Nightmare Moon> BEEP BEEP MOTHERFUCKERS!!!";

            var expected = new LogMessage(new DateTime(2013, 12, 24, 0, 15, 0), "Nightmare Moon", "BEEP BEEP MOTHERFUCKERS!!!");

            var logMessage = new LogParser().Parse(logFile).Single();
            Assert.Equal(expected, logMessage);
        }

        [Fact]
        public void CanParseKick()
        {
            var logFile = 
@"--- Log opened Tue Dec 24 00:26:23 2013
04:28 -!- :owl was kicked from general@conference.friendshipismagicsquad.com by general@conference.friendshipismagicsquad.com [:sweetiestare:]";

            var expected = new KickedMessage(new DateTime(2013, 12, 24, 4, 28, 0), ":owl", ":sweetiestare:");
            var logMessage = new LogParser().Parse(logFile).Cast<KickedMessage>().Single();

            Assert.Equal(expected, logMessage);
        }

        [Fact]
        public void CanParseDateChanged()
        {
            var logFile =
@"--- Log opened Tue Dec 24 00:26:23 2013
23:55 < Sweetiebot> Ask Fluffle Puff, I gave her the pomf.
--- Day changed Wed Dec 25 2013
00:00 < Nyctef> MERRY CHRISTMAS BRITFAGS :santashy:";

            var expected = new List<LogMessage>
            {
                new LogMessage(new DateTime(2013, 12, 24, 23, 55, 0), "Sweetiebot", "Ask Fluffle Puff, I gave her the pomf"),
                new LogMessage(new DateTime(2013, 12, 25, 0, 0, 0), "Nyctef", "MERRY CHRISTMAS BRITFAGS :santashy:"),
            };

            var logMessages = new LogParser().Parse(logFile).ToList();

            Assert.Equal(expected, logMessages);
        }
    }
}