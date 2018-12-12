using System;
using System.Collections.Generic;
using System.Linq;

namespace TaxiDispatcher.App
{
    public class TaxiDriverRepo
    {
        public List<Taxi> TaxiDrivers { get; }

        public TaxiDriverRepo(List<Taxi> taxiDrivers)
        {
            TaxiDrivers = taxiDrivers;
        }

        public Taxi BestTaxiForLocation(int locationFrom)
        {
            if (!TaxiDrivers.Any())
            {
                throw new Exception("There are no taxi drivers registered with the Scheduler");
            }

            return TaxiDrivers.Aggregate(
                (curBest, t) => (curBest == null || t.ProximityToLocation(locationFrom) < curBest.ProximityToLocation(locationFrom) ? t : curBest)
            );
        }
    }
}
