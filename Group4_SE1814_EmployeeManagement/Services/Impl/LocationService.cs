using BusinessObjects;
using Repositories.Impl;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Impl
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepo locationRepo;

        public LocationService()
        {
            locationRepo = new LocationRepo();
        }

        public Location? GetLocationById(string id)
        {
           return locationRepo.GetLocationById(id);
        }

        public List<Location> GetLocations()
        {
            return locationRepo.GetLocations();
        }
    }
}
