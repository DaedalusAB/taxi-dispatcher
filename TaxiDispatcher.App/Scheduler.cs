using System;
using System.Collections.Generic;
using System.Linq;

namespace TaxiDispatcher.App
{
    public class Scheduler
    {
        private const int MaxAcceptableDistance = 15;

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

        private Taxi FindBestTaxi(int locationFrom)
        {
            var bestTaxi = _taxiDriverRepo.BestTaxiForLocation(locationFrom);

            if (Math.Abs(bestTaxi.Location - locationFrom) > MaxAcceptableDistance)
                throw new Exception("There are no available taxi vehicles!");

            return bestTaxi;
        }

        private Ride CreateRide(int locationFrom, int locationTo, Taxi bestTaxi)
        {
            var ride = new Ride
            {
                Taxi = bestTaxi,
                LocationFrom = locationFrom,
                LocationTo = locationTo
            };

            return ride;
        }

        private void CalculatePrice(int locationFrom, int locationTo, int rideType, DateTime time, Taxi bestTaxi, Ride ride)
        {
            switch (bestTaxi.CompanyName)
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
            _taxiDriverRepo.TaxiDrivers.Find(t => t.TaxiDriverId == ride.Taxi.TaxiDriverId).Location = ride.LocationTo;

            Console.WriteLine("Ride accepted, waiting for driver: " + ride.Taxi.DriverName);
        }

        public List<Ride> GetRideList(int driverId)
        {
            List<Ride> rides = new List<Ride>();
            List<int> ids = InMemoryRideDataBase.GetRideIds();
            foreach (int id in ids)
            {
                Ride ride = InMemoryRideDataBase.GetRide(id);
                if (ride.Taxi.TaxiDriverId == driverId)
                    rides.Add(ride);
            }

            return rides;
        }
    }
}
