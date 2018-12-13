using System;
using System.Collections.Generic;
using System.Linq;

namespace TaxiDispatcher.App
{
    public class Scheduler
    {
        private const int MaxAcceptableDistance = 15;

        private readonly RideRepository _rideRepository;
        private readonly TaxiRepository _taxiRepository;

        public Scheduler(RideRepository rideRepository, TaxiRepository taxiRepository)
        {
            _rideRepository = rideRepository;
            _taxiRepository = taxiRepository;
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
            var bestTaxi = _taxiRepository.BestTaxiForLocation(locationFrom);

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
            _rideRepository.SaveRide(ride);
            _taxiRepository.TaxiDrivers.Find(t => t.TaxiDriverId == ride.Taxi.TaxiDriverId).MoveToLocation(ride.LocationTo);

            Console.WriteLine("Ride accepted, waiting for driver: " + ride.Taxi.DriverName);
        }

        public List<Ride> GetRideList(int driverId) => 
            _rideRepository.Rides.Where(r => r.Taxi.TaxiDriverId == driverId).ToList();
    }
}
