using System;

namespace TaxiDispatcher.App
{
    public class TaxiDriver
    {
        public int TaxiDriverId { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public int Location { get; set; }

        public int ProximityToLocation(int location) =>
            Math.Abs(Location - location);
    }
}
