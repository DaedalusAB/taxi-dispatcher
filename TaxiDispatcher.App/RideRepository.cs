using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiDispatcher.App
{
    public class RideRepository
    {
        public IEnumerable<Ride> Rides =>
            InMemoryRideDatabase.Rides;

        public void SaveRide(Ride ride) =>
            InMemoryRideDatabase.SaveRide(ride);
    }
}
