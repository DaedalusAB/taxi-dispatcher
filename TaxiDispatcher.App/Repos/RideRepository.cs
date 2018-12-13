using System.Collections.Generic;
using TaxiDispatcher.App.Models;

namespace TaxiDispatcher.App.Repos
{
    public class RideRepository
    {
        public IEnumerable<Ride> Rides =>
            InMemoryRideDatabase.Rides;

        public void SaveRide(Ride ride) =>
            InMemoryRideDatabase.SaveRide(ride);
    }
}
