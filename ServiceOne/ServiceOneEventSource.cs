using System.Diagnostics.Tracing;

namespace ServiceOne
{
    [EventSource(Name = "Adamson-ServiceFabric-ServiceOne")]
    public sealed class ServiceOneEventSource : EventSource
    {
        public static ServiceOneEventSource Current = new ServiceOneEventSource();

        static ServiceOneEventSource() { }

        [Event(1, Level = EventLevel.Informational, Message = "[Changed]:{0}", Version = 0)]
        public void Log(string message)
        {
            this.WriteEvent(1, message);
        }
    }
}
