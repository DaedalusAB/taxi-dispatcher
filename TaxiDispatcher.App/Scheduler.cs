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
            var price = CalculatePrice(locationFrom, locationTo, rideType, time, bestTaxi.Company);
            var ride = new Ride(locationFrom, locationTo, rideType, time, bestTaxi, price);

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

        private int CalculatePrice(int locationFrom, int locationTo, RideTypeEnum rideTypeEnum, DateTime time, TaxiCompany company)
        {
            var price = company.PricePerUnitOfDistance * Math.Abs(locationFrom - locationTo);

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
            InMemoryRideDatabase.SaveRide(ride);
            _taxiDriverRepo.TaxiDrivers.Find(t => t.TaxiDriverId == ride.Taxi.TaxiDriverId).Location = ride.LocationTo;

            Console.WriteLine("Ride accepted, waiting for driver: " + ride.Taxi.DriverName);
        }

        public List<Ride> GetRideList(int driverId)
        {
            List<Ride> rides = new List<Ride>();
            List<int> ids = InMemoryRideDatabase.GetRideIds();
            foreach (int id in ids)
            {
                Ride ride = InMemoryRideDatabase.GetRide(id);
                if (ride.Taxi.TaxiDriverId == driverId)
                    rides.Add(ride);
            }

            return rides;
        }
    }
}
