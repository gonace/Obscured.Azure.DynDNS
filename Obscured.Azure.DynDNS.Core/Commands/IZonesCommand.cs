using System.Collections.Generic;
using Obscured.Azure.DynDNS.Core.Models;

namespace Obscured.Azure.DynDNS.Core.Commands
{
    public interface IZonesCommand
    {
        IList<Zone> List();
        Zone Get(string name);
        Zone Create(Zone zone);
        Zone Update(Zone zone);
        bool Remove(Zone zone);
    }
}
