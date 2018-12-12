using System;
using System.Collections.Generic;
using System.Linq;

namespace TaxiDispatcher.App
{
    public class Scheduler
    {
        private readonly TaxiDriverRepo _taxiDriverRepo;

        public Scheduler(TaxiDriverRepo taxiDriverRepo)
        {
            _taxiDriverRepo = taxiDriverRepo;
        }

        public Ride OrderRide(int locationFrom, int locationTo, int rideType, DateTime time)
        {
            var bestTaxi = FindBestTaxi(locationFrom);
            var ride = CreateRide(locationFrom, locationTo, bestTaxi);
            CalculatePrice(locationFrom, locationTo, rideType, time, bestTaxi, ride);

            Console.WriteLine("Ride ordered, price: " + ride.Price);

            return ride;
        }

        private TaxiDriver FindBestTaxi(int locationFrom)
        {
            if (!_taxiDriverRepo.TaxiDrivers.Any())
            {
                throw new Exception("There are no taxi drivers registered with the Scheduler");
            }
            
            var bestTaxi = _taxiDriverRepo.TaxiDrivers
                .Aggregate((curBest, t) => (curBest == null || t.ProximityToLocation(locationFrom) < curBest.ProximityToLocation(locationFrom) ? t : curBest));


            if (Math.Abs(bestTaxi.Location - locationFrom) > 15)
                throw new Exception("There are no available taxi vehicles!");

            return bestTaxi;
        }

        private static Ride CreateRide(int locationFrom, int locationTo, TaxiDriver bestTaxiDriver)
        {
            Ride ride = new Ride();
            ride.TaxiDriverId = bestTaxiDriver.TaxiDriverId;
            ride.LocationFrom = locationFrom;
            ride.LocationTo = locationTo;
            ride.TaxiDriverName = bestTaxiDriver.Name;
            return ride;
        }

        private static void CalculatePrice(int locationFrom, int locationTo, int rideType, DateTime time, TaxiDriver bestTaxiDriver, Ride ride)
        {
            switch (bestTaxiDriver.CompanyName)
            {
                case "Naxi":
                    {
                        ride.Price = 10 * Math.Abs(locationFrom - locationTo);
                        break;
                    }
                case "Alfa":
                    {
                        ride.Price = 15 * Math.Abs(locationFrom - locationTo);
                        break;
                    }
                case "Gold":
                    {
                        ride.Price = 13 * Math.Abs(locationFrom - locationTo);
                        break;
                    }
                default:
                    {
                        throw new Exception("Ilegal company");
                    }
            }

            if (rideType == Constants.InterCity)
            {
                ride.Price *= 2;
            }

            if (time.Hour < 6 || time.Hour > 22)
            {
                ride.Price *= 2;
            }
        }

        public void AcceptRide(Ride ride)
        {
            InMemoryRideDataBase.SaveRide(ride);
            _taxiDriverRepo.TaxiDrivers.Find(t => t.TaxiDriverId == ride.TaxiDriverId).Location = ride.LocationTo;

            Console.WriteLine("Ride accepted, waiting for driver: " + ride.TaxiDriverName);
        }

        public List<Ride> GetRideList(int driverId)
        {
            List<Ride> rides = new List<Ride>();
            List<int> ids = InMemoryRideDataBase.GetRideIds();
            foreach (int id in ids)
            {
                Ride ride = InMemoryRideDataBase.GetRide(id);
                if (ride.TaxiDriverId == driverId)
                    rides.Add(ride);
            }

            return rides;
        }
    }
}
