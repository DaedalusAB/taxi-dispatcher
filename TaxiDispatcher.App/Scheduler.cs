using System;
using System.Collections.Generic;
using System.Linq;
using TaxiDispatcher.App.Models;
using TaxiDispatcher.App.Repos;

namespace TaxiDispatcher.App
{
    public class Scheduler
    {
        private const int MaxAcceptableDistance = 15;
        private const int ExpensiveHoursStart = 22;
        private const int ExpensiveHoursEnd = 6;

        private readonly RideRepository _rideRepository;
        private readonly TaxiRepository _taxiRepository;
        private readonly RidePriceCalculator _ridePriceCalculator;

        public Scheduler(RideRepository rideRepository, TaxiRepository taxiRepository, RidePriceCalculator ridePriceCalculator)
        {
            _rideRepository = rideRepository;
            _taxiRepository = taxiRepository;
            _ridePriceCalculator = ridePriceCalculator;
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

        private int CalculatePrice(int locationFrom, int locationTo, RideTypeEnum rideType, DateTime time, TaxiCompany company) =>
            _ridePriceCalculator.CalculatePrice(locationFrom, locationTo, rideType, time, company);

        public void AcceptRide(Ride ride)
        {
            _rideRepository.SaveRide(ride);
            _taxiRepository.TaxiDrivers.Find(t => t.TaxiDriverId == ride.Taxi.TaxiDriverId).MoveToLocation(ride.LocationTo);

            Console.WriteLine("Ride accepted, waiting for driver: " + ride.Taxi.DriverName);
        }

        public List<Ride> RidesOfTaxi(int driverId) =>
            _rideRepository.Rides.Where(r => r.Taxi.TaxiDriverId == driverId).ToList();
    }
}
