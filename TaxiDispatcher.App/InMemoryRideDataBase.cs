using System.Collections.Generic;
using System.Linq;
using static TaxiDispatcher.App.Scheduler;

namespace TaxiDispatcher.App
{
    public static class InMemoryRideDatabase
    {
        public static List<Ride> Rides = new List<Ride>();

        public static void SaveRide(Ride ride)
        {
            //  if Rides could be removed from list, this wouldn't guarantee a unique ID anymore;
            //  but rides aren't removed, so I'm leaving it like this
            ride.RideId = Rides.Count + 1;
            Rides.Add(ride);
        }

        public static Ride GetRide(int id) =>
            Rides.FirstOrDefault(r => r.RideId == id);

        public static List<int> GetRideIds() =>
            Rides.Select(r => r.RideId).ToList();
    }
}
