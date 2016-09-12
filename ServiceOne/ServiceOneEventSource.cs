﻿using System.Diagnostics.Tracing;
using static System.Net.WebUtility;

namespace ServiceOne
{
    [EventSource(Name = "Adamson-ServiceFabric-ServiceOne")]
    public sealed class ServiceOneEventSource : EventSource
    {
        public static ServiceOneEventSource Current = new ServiceOneEventSource();

        private ServiceOneEventSource() : base(EventSourceSettings.EtwSelfDescribingEventFormat | EventSourceSettings.ThrowOnEventWriteErrors) { }

        [Event(1, Level = EventLevel.Informational, Message = "{0}", Version = 0)]
        public void Log(string message)
        {
            var htmlEncoded = HtmlEncode(message);
            this.WriteEvent(1, htmlEncoded);
        }

        [Event(2, Level = EventLevel.Informational, Version = 1, Opcode = EventOpcode.Start)]
        public void RequestStart(string message)
        {
            var htmlEncoded = HtmlEncode(message);
            WriteEvent(2, htmlEncoded);
        }

        [Event(3, Level = EventLevel.Informational, Message = "[Stopped]:{0}", Version = 2, Opcode = EventOpcode.Stop)]
        public void RequestStop(string message)
        {
            var htmlEncoded = HtmlEncode(message);
            WriteEvent(3, htmlEncoded);
        }
    }
}
