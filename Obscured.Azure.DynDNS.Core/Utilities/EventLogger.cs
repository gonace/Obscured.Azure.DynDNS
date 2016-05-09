using System.Diagnostics;

namespace Obscured.Azure.DynDNS.Core.Utilities
{
    public class EventLogger : IEventLogger
    {
        private static EventLog _eventLog;
        private static string _sourceName;

        public EventLogger(string sourceName)
        {
            _sourceName = sourceName;
            _eventLog = new EventLog();

            if (!EventLog.SourceExists(_sourceName))
            {
                EventLog.CreateEventSource(_sourceName, "Obscured");
            }

            _eventLog.Source = _sourceName;
            _eventLog.Log = "Obscured";
        }

        public void LogMessage(string message)
        {
            _eventLog.WriteEntry(message);
        }

        public void LogMessage(string message, EventLogEntryType type)
        {
            _eventLog.WriteEntry(message, type);
        }

        public void LogMessage(string message, EventLogEntryType type, int eventID)
        {
            _eventLog.WriteEntry(message, type, eventID);
        }

        public void LogMessage(string message, EventLogEntryType type, int eventID, short category)
        {
            _eventLog.WriteEntry(message, type, eventID, category);
        }

        public void LogMessage(string message, EventLogEntryType type, int eventID, short category, byte[] rawData)
        {
            _eventLog.WriteEntry(message, type, eventID, category, rawData);
        }
    }
}
