using System;
using System.Collections.Generic;
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

            Assert.Equal(expected, new LogParser().Parse(logFile));
        }
    }
}