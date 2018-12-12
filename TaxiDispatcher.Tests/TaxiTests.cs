using System;
using TaxiDispatcher.App;
using Xunit;

namespace TaxiDispatcher.Tests
{
    public class TaxiTests
    {
        [Fact]
        public void RideFrom5To0()
        {
            Scheduler scheduler = new Scheduler();
            var ride = scheduler.OrderRide(5, 0, Constants.City, new DateTime(2018, 1, 1, 23, 0, 0));
            scheduler.AcceptRide(ride);

            Assert.Equal(100, ride.Price);
        }

        [Fact]
        public void RideFrom0to12()
        {
            Scheduler scheduler = new Scheduler();
            var ride = scheduler.OrderRide(0, 12, Constants.InterCity, new DateTime(2018, 1, 1, 9, 0, 0));
            scheduler.AcceptRide(ride);

            Assert.Equal(240, ride.Price);
        }

        [Fact]
        public void RideFrom5To0WhileFirstDriverIsBusy()
        {
            Scheduler scheduler = new Scheduler();

            //  setup (make a driver busy)
            var ride = scheduler.OrderRide(5, 0, Constants.City, new DateTime(2018, 1, 1, 23, 0, 0));
            scheduler.AcceptRide(ride);

            
            ride = scheduler.OrderRide(5, 0, Constants.City, new DateTime(2018, 1, 1, 11, 0, 0));
            scheduler.AcceptRide(ride);

            Assert.Equal(75, ride.Price);
        }

        [Fact]
        public void OrderWhenDriversAreTooFar()
        {
            Scheduler scheduler = new Scheduler();
            var exception = Assert.Throws<Exception>(() => scheduler.OrderRide(35, 12, Constants.City, new DateTime(2018, 1, 1, 11, 0, 0)));
            Assert.Equal("There are no available taxi vehicles!", exception.Message);
            
        }

        [Fact]
        public void DailyEarnings()
        {
            Scheduler scheduler = new Scheduler();

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
