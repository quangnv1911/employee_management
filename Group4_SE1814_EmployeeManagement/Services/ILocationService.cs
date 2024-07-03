using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ILocationService
    {
        List<Location> GetLocations();
        Location? GetLocationById(string id);
    }
}
