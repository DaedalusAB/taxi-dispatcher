﻿using System;
using System.Collections.Generic;
using System.Linq;
using TaxiDispatcher.App.Models;
using TaxiDispatcher.App.Repos;

namespace TaxiDispatcher.App
{
    public class Scheduler
    {
        private const int MaxAcceptableDistance = 15;

        private readonly RideRepository _rideRepository;
        private readonly TaxiRepository _taxiRepository;
        private readonly RidePriceCalculator _ridePriceCalculator;
        private readonly Logger _logger;

        public Scheduler(RideRepository rideRepository, TaxiRepository taxiRepository, RidePriceCalculator ridePriceCalculator, Logger logger)
        {
            _rideRepository = rideRepository ?? throw new ArgumentNullException(nameof(rideRepository));
            _taxiRepository = taxiRepository ?? throw new ArgumentNullException(nameof(taxiRepository));
            _ridePriceCalculator = ridePriceCalculator ?? throw new ArgumentNullException(nameof(ridePriceCalculator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Ride OrderCityRide(int locationFrom, int locationTo, DateTime time) => 
            OrderRide(locationFrom, locationTo, RideTypeEnum.City, time);

        public Ride OrderInnerCityRide(int locationFrom, int locationTo, DateTime time) => 
            OrderRide(locationFrom, locationTo, RideTypeEnum.InnerCity, time);

        private Ride OrderRide(int locationFrom, int locationTo, RideTypeEnum rideType, DateTime time)
        {
            var bestTaxi = FindBestTaxi(locationFrom);

            if (bestTaxi == null)
            {
                _logger.WriteLine("Could not find a suitable taxi (all are too far)");
                return null;
            }

            var price = _ridePriceCalculator.CalculatePrice(locationFrom, locationTo, rideType, time, bestTaxi.Company);
            var ride = new Ride(locationFrom, locationTo, rideType, time, bestTaxi, price);

            _logger.WriteLine("Ride ordered, price: " + ride.Price);

            return ride;
        }

        private Taxi FindBestTaxi(int locationFrom)
        {
            var bestTaxi = _taxiRepository.BestTaxiForLocation(locationFrom);

            return bestTaxi.ProximityToLocation(locationFrom) > MaxAcceptableDistance
                ? null
                : bestTaxi;
        }

        public void AcceptRide(Ride ride)
        {
            if (ride == null)
            {
                return;
            }

            _taxiRepository.TaxiDrivers.Find(t => t.TaxiDriverId == ride.Taxi.TaxiDriverId).MoveToLocation(ride.LocationTo);
            _rideRepository.SaveRide(ride);

            _logger.WriteLine("Ride accepted, waiting for driver: " + ride.Taxi.DriverName);
        }

        public List<Ride> RidesOfTaxiDriver(int driverId) =>
            _rideRepository.Rides.Where(r => r.Taxi.TaxiDriverId == driverId).ToList();

        public int EarningsOfTaxiDriver(int driverId) =>
            RidesOfTaxiDriver(driverId).Sum(r => r.Price);
    }
}
