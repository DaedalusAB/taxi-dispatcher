using System;
using System.Collections.Generic;
using TaxiDispatcher.App;
using Xunit;

namespace TaxiDispatcher.Tests
{
    public class TaxiTests
    {
        private TaxiDriverRepo _taxiDriverRepo = new TaxiDriverRepo(new List<TaxiDriver>()
        {
            new TaxiDriver { TaxiDriverId = 1, Name = "Predrag", CompanyName = "Naxi", Location = 1 },
            new TaxiDriver { TaxiDriverId = 2, Name = "Nenad", CompanyName = "Naxi", Location = 4 },
            new TaxiDriver { TaxiDriverId = 3, Name = "Dragan", CompanyName = "Alfa", Location = 6 },
            new TaxiDriver { TaxiDriverId = 4, Name = "Goran", CompanyName = "Gold", Location = 7 }
        });

        [Fact]
        public void RideFrom5To0()
        {
            Scheduler scheduler = new Scheduler(_taxiDriverRepo);
            var ride = scheduler.OrderRide(5, 0, Constants.City, new DateTime(2018, 1, 1, 23, 0, 0));
            scheduler.AcceptRide(ride);

            Assert.Equal(100, ride.Price);
            Assert.Equal("Nenad", ride.TaxiDriverName);
        }

        [Fact]
        public void RideFrom0to12()
        {
            Scheduler scheduler = new Scheduler(_taxiDriverRepo);
            var ride = scheduler.OrderRide(5, 0, Constants.City, new DateTime(2018, 1, 1, 23, 0, 0));
            scheduler.AcceptRide(ride);
            ride = scheduler.OrderRide(0, 12, Constants.InterCity, new DateTime(2018, 1, 1, 9, 0, 0));
            scheduler.AcceptRide(ride);

            Assert.Equal(240, ride.Price);
            Assert.Equal("Nenad", ride.TaxiDriverName);
        }

        [Fact]
        public void RideFrom5To0WhileFirstDriverIsBusy()
        {
            Scheduler scheduler = new Scheduler(_taxiDriverRepo);

            //  setup (make a driver busy)
            var ride = scheduler.OrderRide(5, 0, Constants.City, new DateTime(2018, 1, 1, 23, 0, 0));
            scheduler.AcceptRide(ride);

            
            ride = scheduler.OrderRide(5, 0, Constants.City, new DateTime(2018, 1, 1, 11, 0, 0));
            scheduler.AcceptRide(ride);

            Assert.Equal(75, ride.Price);
            Assert.Equal("Dragan", ride.TaxiDriverName);
        }

        [Fact]
        public void OrderWhenDriversAreTooFar()
        {
            Scheduler scheduler = new Scheduler(_taxiDriverRepo);
            var exception = Assert.Throws<Exception>(() => scheduler.OrderRide(35, 12, Constants.City, new DateTime(2018, 1, 1, 11, 0, 0)));
            Assert.Equal("There are no available taxi vehicles!", exception.Message);
            
        }

        [Fact]
        public void DailyEarningsOfDriver2()
        {
            Scheduler scheduler = new Scheduler(_taxiDriverRepo);

            //  setup
            var ride = scheduler.OrderRide(5, 0, Constants.City, new DateTime(2018, 1, 1, 23, 0, 0));
            scheduler.AcceptRide(ride);
            ride = scheduler.OrderRide(0, 12, Constants.InterCity, new DateTime(2018, 1, 1, 9, 0, 0));
            scheduler.AcceptRide(ride);
            ride = scheduler.OrderRide(5, 0, Constants.City, new DateTime(2018, 1, 1, 11, 0, 0));
            scheduler.AcceptRide(ride);

            int total = 0;
            foreach (var r in scheduler.GetRideList(2))
            {
                total += r.Price;
            }

            Assert.Equal(340, total);
        }
    }
}
