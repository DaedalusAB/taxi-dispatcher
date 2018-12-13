using System;

namespace TaxiDispatcher.App.Models
{
    public class Taxi   //  maybe should be renamed to TaxiDriver
    {
        public int TaxiDriverId { get; }
        public string DriverName { get; }
        public TaxiCompany Company { get; }
        public int Location { get; private set; }

        public Taxi(int id, string driverName, TaxiCompany company, int location)
        {
            TaxiDriverId = id;
            DriverName = driverName;
            Company = company;
            Location = location;
        }

        public int ProximityToLocation(int location) =>
            Math.Abs(Location - location);

        public void MoveToLocation(int location) =>
            Location = location;
    }
}
