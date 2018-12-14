using System.Collections.Generic;
using System.Linq;
using TaxiDispatcher.App.Models;

namespace TaxiDispatcher.App.Repos
{
    public static class InMemoryRideDatabase
    {
        public static List<Ride> Rides = new List<Ride>();

        public static void SaveRide(Ride ride)
        {
            ride.RideId = Rides.Any() ? Rides.Max(r => r.RideId) + 1 : 1;
            Rides.Add(ride);
        }

        public static Ride GetRide(int id) =>
            Rides.FirstOrDefault(r => r.RideId == id);

        public static List<int> GetRideIds() =>         //  this can go
            Rides.Select(r => r.RideId).ToList();
    }
}
