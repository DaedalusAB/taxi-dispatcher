using System;
using System.Collections.Generic;
using TaxiDispatcher.App;
using Xunit;

namespace TaxiDispatcher.Tests
{
    public class TaxiTests
    {
        private TaxiDriverRepo _taxiDriverRepo = new TaxiDriverRepo(new List<Taxi>()
        {
            new Taxi { TaxiDriverId = 1, DriverName = "Predrag", CompanyName = "Naxi", Location = 1 },
            new Taxi { TaxiDriverId = 2, DriverName = "Nenad", CompanyName = "Naxi", Location = 4 },
            new Taxi { TaxiDriverId = 3, DriverName = "Dragan", CompanyName = "Alfa", Location = 6 },
            new Taxi { TaxiDriverId = 4, DriverName = "Goran", CompanyName = "Gold", Location = 7 }
        });

        [Fact]
        public void RideFrom5To0()
        {
            Scheduler scheduler = new Scheduler(_taxiDriverRepo);
            var ride = scheduler.OrderRide(5, 0, Constants.City, new DateTime(2018, 1, 1, 23, 0, 0));
            scheduler.AcceptRide(ride);

            Assert.Equal(100, ride.Price);
            Assert.Equal("Nenad", ride.Taxi.DriverName);
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
            Assert.Equal("Nenad", ride.Taxi.DriverName);
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
            Assert.Equal("Dragan", ride.Taxi.DriverName);
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
