using System.Diagnostics.Tracing;

namespace ServiceTwo
{
    [EventSource(Name = "Adamson-ServiceFabric-ServiceTwo")]
    public sealed class ServiceTwoEventSource : EventSource
    {
        public static ServiceTwoEventSource Current = new ServiceTwoEventSource();

        static ServiceTwoEventSource() { }

        [Event(1, Level = EventLevel.Informational, Message = "[VeryUpdated]:{0}", Version = 1)]
        public void Log(string message)
        {
            WriteEvent(1, message);
        }
    }
}
