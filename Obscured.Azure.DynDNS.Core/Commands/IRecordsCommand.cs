using System.Collections.Generic;
using Obscured.Azure.DynDNS.Core.Models;

namespace Obscured.Azure.DynDNS.Core.Commands
{
    public interface IRecordsCommand
    {
        IList<Record> List();
        Record Get(string name, string type);
        Record Create(Record record, string zoneName);
        Record Update(Record record, string zoneName);
        bool Remove(Record record);
    }
}
