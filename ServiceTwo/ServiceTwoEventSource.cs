using System.Diagnostics.Tracing;
using static System.Net.WebUtility;

namespace ServiceTwo
{
    [EventSource(Name = "Adamson-ServiceFabric-ServiceTwo")]
    public sealed class ServiceTwoEventSource : EventSource
    {
        public static ServiceTwoEventSource Current = new ServiceTwoEventSource();

        private ServiceTwoEventSource() : base(EventSourceSettings.EtwSelfDescribingEventFormat | EventSourceSettings.ThrowOnEventWriteErrors)
        { }

        [Event(1, Level = EventLevel.Informational, Message = "{0}", Version = 0)]
        public void Log(string message)
        {
            var htmlEncoded = HtmlEncode(message);
            this.WriteEvent(1, htmlEncoded);
        }

        [Event(2, Level = EventLevel.Informational, Message = "[Request Started]:{0}", Version = 1, Opcode = EventOpcode.DataCollectionStart)]
        public void RequestStart(string message)
        {
            var htmlEncoded = HtmlEncode(message);
            this.WriteEvent(2, htmlEncoded);
        }

        [Event(3, Level = EventLevel.Informational, Message = "[Request Stopped]:{0}", Version = 1, Opcode = EventOpcode.DataCollectionStop)]
        public void RequestStop(string message)
        {
            var htmlEncoded = HtmlEncode(message);
            WriteEvent(3, htmlEncoded);
        }
    }
}
