using System;

namespace TaxiDispatcher.App
{
    public class Taxi
    {
        public int TaxiDriverId { get; set; }
        public string DriverName { get; set; }
        public string CompanyName { get; set; }
        public int Location { get; set; }

        public int ProximityToLocation(int location) =>
            Math.Abs(Location - location);
    }
}
