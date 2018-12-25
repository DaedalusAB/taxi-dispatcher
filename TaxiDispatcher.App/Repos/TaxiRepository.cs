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

            return TaxiDrivers
                .OrderBy(t => t.ProximityToLocation(locationFrom))
                .First();
        }
    }
}
