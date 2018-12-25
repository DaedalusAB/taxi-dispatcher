using System;
using System.Collections.Generic;
using TaxiDispatcher.App;
using TaxiDispatcher.App.Models;
using TaxiDispatcher.App.Repos;
using Xunit;

namespace TaxiDispatcher.Tests
{
    public class TaxiTests
    {
        private readonly RideRepository _rideRepository = new RideRepository();
        private readonly RidePriceCalculator _ridePriceCalculator = new RidePriceCalculator();
        private readonly Logger _logger = new Logger();
        private readonly TaxiRepository _taxiRepository = new TaxiRepository(new List<Taxi>()
        {
            new Taxi(1, "Predrag", new TaxiCompany("Naxi", 10), 1),
            new Taxi(2, "Nenad", new TaxiCompany("Naxi", 10), 4),
            new Taxi(3, "Dragan", new TaxiCompany("Alfa", 15), 6),
            new Taxi(4, "Goran", new TaxiCompany("Gold", 13), 7)
        });

        [Fact]
        public void RideFrom5To0()
        {
            var scheduler = new Scheduler(_rideRepository, _taxiRepository, _ridePriceCalculator, _logger);
            var ride = scheduler.OrderRide(5, 0, RideTypeEnum.City, new DateTime(2018, 1, 1, 23, 0, 0));
            scheduler.AcceptRide(ride);

            Assert.Equal(100, ride.Price);
            Assert.Equal("Nenad", ride.Taxi.DriverName);
        }

        [Fact]
        public void RideFrom0To12()
        {
            var scheduler = new Scheduler(_rideRepository, _taxiRepository, _ridePriceCalculator, _logger);
            var ride = scheduler.OrderRide(5, 0, RideTypeEnum.City, new DateTime(2018, 1, 1, 23, 0, 0));
            scheduler.AcceptRide(ride);
            ride = scheduler.OrderRide(0, 12, RideTypeEnum.InnerCity, new DateTime(2018, 1, 1, 9, 0, 0));
            scheduler.AcceptRide(ride);

            Assert.Equal(240, ride.Price);
            Assert.Equal("Nenad", ride.Taxi.DriverName);
        }

        [Fact]
        public void RideFrom5To0WhileFirstDriverIsBusy()
        {
            var scheduler = new Scheduler(_rideRepository, _taxiRepository, _ridePriceCalculator, _logger);

            //  setup (make a driver busy)
            var ride = scheduler.OrderRide(5, 0, RideTypeEnum.City, new DateTime(2018, 1, 1, 23, 0, 0));
            scheduler.AcceptRide(ride);

            
            ride = scheduler.OrderRide(5, 0, RideTypeEnum.City, new DateTime(2018, 1, 1, 11, 0, 0));
            scheduler.AcceptRide(ride);

            Assert.Equal(75, ride.Price);
            Assert.Equal("Dragan", ride.Taxi.DriverName);
        }

        [Fact]
        public void OrderWhenDriversAreTooFar()
        {
            var scheduler = new Scheduler(_rideRepository, _taxiRepository, _ridePriceCalculator, _logger);
            var ride = scheduler.OrderRide(35, 12, RideTypeEnum.City, new DateTime(2018, 1, 1, 11, 0, 0));

            Assert.Null(ride);
            
        }

        [Fact]
        public void DailyEarningsOfDriver2()
        {
            var scheduler = new Scheduler(_rideRepository, _taxiRepository, _ridePriceCalculator, _logger);

            //  setup
            var ride = scheduler.OrderRide(5, 0, RideTypeEnum.City, new DateTime(2018, 1, 1, 23, 0, 0));
            scheduler.AcceptRide(ride);
            ride = scheduler.OrderRide(0, 12, RideTypeEnum.InnerCity, new DateTime(2018, 1, 1, 9, 0, 0));
            scheduler.AcceptRide(ride);
            ride = scheduler.OrderRide(5, 0, RideTypeEnum.City, new DateTime(2018, 1, 1, 11, 0, 0));
            scheduler.AcceptRide(ride);


            Assert.Equal(340, scheduler.EarningsOfTaxiDriver(2));
        }
    }
}
