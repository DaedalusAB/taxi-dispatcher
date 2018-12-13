using System;
using System.Collections.Generic;
using TaxiDispatcher.App;
using Xunit;

namespace TaxiDispatcher.Tests
{
    public class TaxiTests
    {
        private readonly RideRepository _rideRepository = new RideRepository();
        private readonly RidePriceCalculator _ridePriceCalculator = new RidePriceCalculator();
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
            Scheduler scheduler = new Scheduler(_rideRepository, _taxiRepository, _ridePriceCalculator);
            var ride = scheduler.OrderRide(5, 0, RideTypeEnum.City, new DateTime(2018, 1, 1, 23, 0, 0));
            scheduler.AcceptRide(ride);

            Assert.Equal(100, ride.Price);
            Assert.Equal("Nenad", ride.Taxi.DriverName);
        }

        [Fact]
        public void RideFrom0To12()
        {
            Scheduler scheduler = new Scheduler(_rideRepository, _taxiRepository, _ridePriceCalculator);
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
            Scheduler scheduler = new Scheduler(_rideRepository, _taxiRepository, _ridePriceCalculator);

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
            Scheduler scheduler = new Scheduler(_rideRepository, _taxiRepository, _ridePriceCalculator);
            var exception = Assert.Throws<Exception>(() => scheduler.OrderRide(35, 12, RideTypeEnum.City, new DateTime(2018, 1, 1, 11, 0, 0)));
            Assert.Equal("There are no available taxi vehicles!", exception.Message);
            
        }

        [Fact]
        public void DailyEarningsOfDriver2()
        {
            Scheduler scheduler = new Scheduler(_rideRepository, _taxiRepository, _ridePriceCalculator);

            //  setup
            var ride = scheduler.OrderRide(5, 0, RideTypeEnum.City, new DateTime(2018, 1, 1, 23, 0, 0));
            scheduler.AcceptRide(ride);
            ride = scheduler.OrderRide(0, 12, RideTypeEnum.InnerCity, new DateTime(2018, 1, 1, 9, 0, 0));
            scheduler.AcceptRide(ride);
            ride = scheduler.OrderRide(5, 0, RideTypeEnum.City, new DateTime(2018, 1, 1, 11, 0, 0));
            scheduler.AcceptRide(ride);

            int total = 0;
            foreach (var r in scheduler.RidesOfTaxi(2))
            {
                total += r.Price;
            }

            Assert.Equal(340, total);
        }
    }
}
