using System.Diagnostics;

namespace Obscured.Azure.DynDNS.Core.Utilities
{
    public interface IEventLogger
    {
        void LogMessage(string message);
        void LogMessage(string message, EventLogEntryType type);
        void LogMessage(string message, EventLogEntryType type, int eventID);
        void LogMessage(string message, EventLogEntryType type, int eventID, short category);
        void LogMessage(string message, EventLogEntryType type, int eventID, short category, byte[] rawData);
    }
}
