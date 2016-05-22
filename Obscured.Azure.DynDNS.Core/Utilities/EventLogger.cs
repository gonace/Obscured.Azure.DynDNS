using System.Diagnostics;

namespace Obscured.Azure.DynDNS.Core.Utilities
{
    public class EventLogger : IEventLogger
    {
        public static EventLog _eventLog;
        public static ISettings _settings;

        public EventLogger(ISettings settings)
        {
            _eventLog = new EventLog();
            _settings = settings;
            var sourceName = _settings.Obscured.LogName;
            
            if (!EventLog.SourceExists(sourceName))
            {
                EventLog.CreateEventSource(sourceName, "Obscured");
            }

            _eventLog.Source = sourceName;
            _eventLog.Log = "Obscured";
        }

        public void LogMessage(string message)
        {
            _eventLog.WriteEntry(message);
        }

        public void LogMessage(string message, EventLogEntryType type)
        {
            if (IsEnabledFor(type))
                _eventLog.WriteEntry(message, type);
        }

        public void LogMessage(string message, EventLogEntryType type, int eventID)
        {
            if (IsEnabledFor(type))
                _eventLog.WriteEntry(message, type, eventID);
        }

        public void LogMessage(string message, EventLogEntryType type, int eventID, short category)
        {
            if (IsEnabledFor(type))
                _eventLog.WriteEntry(message, type, eventID, category);
        }

        public void LogMessage(string message, EventLogEntryType type, int eventID, short category, byte[] rawData)
        {
            if(IsEnabledFor(type))
                _eventLog.WriteEntry(message, type, eventID, category, rawData);
        }

        private static bool IsEnabledFor(EventLogEntryType type)
        {
            return type <= _settings.Obscured.LogLevel;
        }
    }
}
