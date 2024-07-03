using BusinessObjects;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class LocationRepo : ILocationRepo
    {
        public Location? GetLocationById(string id)
        {
            return LocationDAO.GetLocationById(id);
        }

        public List<Location> GetLocations()
        {
            return LocationDAO.GetLocations();
        }
    }
}
