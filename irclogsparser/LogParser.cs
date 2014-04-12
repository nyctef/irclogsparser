﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace irclogsparser
{
    class LogParser
    {
        internal IEnumerable<LogMessage> Parse(string logFile)
        {
            var lines = logFile.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var currentTime = DateTime.Now;
            foreach (var line in lines)
            {
                if (LogStartedMessage.TryCreate(line, out var logStartedMessage))
                {
                    currentTime = logStartedMessage.Time;
                }
                else if (LogMessage.TryCreate(line, currentTime, out var message))
                {
                    yield return message;
                }
            }
        }
    }
}