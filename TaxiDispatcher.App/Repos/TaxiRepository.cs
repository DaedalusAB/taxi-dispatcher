using System;
using System.Collections.Generic;
using System.Linq;
using TaxiDispatcher.App.Models;

namespace TaxiDispatcher.App.Repos
{
    public class TaxiRepository
    {
        public List<Taxi> TaxiDrivers { get; }

        public TaxiRepository(List<Taxi> taxiDrivers)
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
