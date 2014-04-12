using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace irclogsparser
{
    class LogMessage
    {
        private DateTime dateTime;
        private string p1;
        private string p2;

        public LogMessage(DateTime dateTime, string p1, string p2)
        {
            // TODO: Complete member initialization
            this.dateTime = dateTime;
            this.p1 = p1;
            this.p2 = p2;
        }
    }
}
