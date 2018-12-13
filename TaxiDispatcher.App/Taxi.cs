using System;

namespace TaxiDispatcher.App
{
    public class Taxi
    {
        public int TaxiDriverId { get; }
        public string DriverName { get; }
        public TaxiCompany Company { get; }
        public int Location { get; }

        public Taxi(int id, string driverName, TaxiCompany company, int location)
        {
            TaxiDriverId = id;
            DriverName = driverName;
            Company = company;
            Location = location;
        }

        public int ProximityToLocation(int location) =>
            Math.Abs(Location - location);
    }
}
