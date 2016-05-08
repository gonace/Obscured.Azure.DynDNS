using System.Diagnostics;

namespace Obscured.Azure.DynDNS.Core.Utilities
{
    public class EventLogger : IEventLogger
    {
        private static string _sourceName;

        public EventLogger(string sourceName)
        {
            _sourceName = sourceName;

            if (!EventLog.SourceExists(_sourceName))
            {
                EventLog.CreateEventSource(_sourceName, "Obscured");
            }
        }

        public void LogMessage(string message)
        {
            EventLog.WriteEntry(_sourceName, message);
        }

        public void LogMessage(string message, EventLogEntryType type)
        {
            EventLog.WriteEntry(_sourceName, message, type);
        }
    }
}
