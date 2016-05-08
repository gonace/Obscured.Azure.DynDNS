using System.Diagnostics;

namespace Obscured.Azure.DynDNS.Core.Utilities
{
    public interface IEventLogger
    {
        void LogMessage(string message);
        void LogMessage(string message, EventLogEntryType type);
    }
}
