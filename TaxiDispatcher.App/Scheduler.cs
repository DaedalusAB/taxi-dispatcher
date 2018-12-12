using System;
using System.Collections.Generic;

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

        public Ride OrderRide(int locationFrom, int locationTo, RideTypeEnum rideType, DateTime time)
        {
            var bestTaxi = FindBestTaxi(locationFrom);
            var ride = CreateRide(locationFrom, locationTo, bestTaxi);
            ride.Price = CalculatePrice(locationFrom, locationTo, rideType, time, ride);    // todo check this

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

        private Ride CreateRide(int locationFrom, int locationTo, Taxi bestTaxi) =>
            new Ride { Taxi = bestTaxi, LocationFrom = locationFrom, LocationTo = locationTo };

        private int CalculatePrice(int locationFrom, int locationTo, RideTypeEnum rideTypeEnum, DateTime time, Ride ride)
        {
            var price = ride.Taxi.Company.PricePerUnitOfDistance * Math.Abs(locationFrom - locationTo);

            if (rideTypeEnum == RideTypeEnum.InnerCity)
            {
                price *= 2;
            }

            if (time.Hour < 6 || time.Hour > 22)
            {
                price *= 2;
            }

            return price;
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
