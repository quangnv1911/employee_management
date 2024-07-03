using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class LocationDAO
    {
        public static List<Location> GetLocations()
        {
            EmployeeManagementContext context = new EmployeeManagementContext();
            return context.Locations.ToList();
        }

        public static Location? GetLocationById(string id)
        {
            EmployeeManagementContext context = new EmployeeManagementContext();
            return context.Locations.FirstOrDefault(l => l.LocationId == id);
        }
    }
}
