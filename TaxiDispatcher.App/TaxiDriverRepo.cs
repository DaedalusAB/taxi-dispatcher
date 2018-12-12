using System.Collections.Generic;

namespace TaxiDispatcher.App
{
    public class TaxiDriverRepo
    {
        public List<TaxiDriver> TaxiDrivers { get; }

        public TaxiDriverRepo(List<TaxiDriver> taxiDrivers)
        {
            TaxiDrivers = taxiDrivers;
        }
    }
}
