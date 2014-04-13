using System;
using System.Collections.Generic;

namespace irclogsparser
{
    internal class DeowlTracker
    {
        public DeowlTracker()
        {
        }

        internal IEnumerable<Deowl> Run(List<LogMessage> messages)
        {
            string lastDeowlAttemptUsername = null;
            for (int i=0; i<messages.Count;i++)
            {
                var message = messages[i];
                if (message.GetType() == typeof(LogMessage))
                {
                    if (message.Message.ToLower().StartsWith("sweetiebot") && 
                        message.Message.ToLower().Contains("deowl"))
                    {
                        lastDeowlAttemptUsername = message.Speaker;
                    }
                    else if (message.Speaker.ToLower() == "sweetiebot" &&
                        message.Message.ToLower().Contains("maybe another time") &&
                        lastDeowlAttemptUsername != null)
                    {
                        yield return new Deowl(lastDeowlAttemptUsername, false);
                        lastDeowlAttemptUsername = null;
                    }

                }
                else if ((var kickedMessage = message as KickedMessage) != null)
                {
                    if (i >= 1 && lastDeowlAttemptUsername != null && 
                        kickedMessage.KickedPerson == ":owl" && 
                        kickedMessage.Message == ":sweetiestare:")
                    {
                        yield return new Deowl(lastDeowlAttemptUsername, true);
                        lastDeowlAttemptUsername = null;
                    }
                }
            }
        }
    }
}