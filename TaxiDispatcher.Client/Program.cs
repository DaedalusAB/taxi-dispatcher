using System;
using System.Collections.Generic;
using TaxiDispatcher.App;
using TaxiDispatcher.App.Models;
using TaxiDispatcher.App.Repos;

namespace TaxiDispatcher.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var rideRepo = new RideRepository();
            var ridePriceCalc = new RidePriceCalculator();
            var logger = new Logger();
            var taxiRepository = new TaxiRepository(new List<Taxi>()
            {
                new Taxi(1, "Predrag", new TaxiCompany("Naxi", 10), 1),
                new Taxi(2, "Nenad", new TaxiCompany("Naxi", 10), 4),
                new Taxi(3, "Dragan", new TaxiCompany("Alfa", 15), 6),
                new Taxi(4, "Goran", new TaxiCompany("Gold", 13), 7)
            });
            

            var scheduler = new Scheduler(rideRepo, taxiRepository, ridePriceCalc, logger);

            try
            {
                Console.WriteLine("Ordering ride from 5 to 0...");
                var ride = scheduler.OrderRide(5, 0, RideTypeEnum.City, new DateTime(2018, 1, 1, 23, 0, 0));
                scheduler.AcceptRide(ride);
                Console.WriteLine("");
            }
            catch (Exception e)
            {
                if (e.Message == "There are no available taxi vehicles!")
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("");
                }
                else
                    throw;
            }

            try
            {
                Console.WriteLine("Ordering ride from 0 to 12...");
                Ride ride = scheduler.OrderRide(0, 12, RideTypeEnum.InnerCity, new DateTime(2018, 1, 1, 9, 0, 0));
                scheduler.AcceptRide(ride);
                Console.WriteLine("");
            }
            catch (Exception e)
            {
                if (e.Message == "There are no available taxi vehicles!")
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("");
                }
                else
                    throw;
            }

            try
            {
                Console.WriteLine("Ordering ride from 5 to 0...");
                Ride ride = scheduler.OrderRide(5, 0, RideTypeEnum.City, new DateTime(2018, 1, 1, 11, 0, 0));
                scheduler.AcceptRide(ride);
                Console.WriteLine("");
            }
            catch (Exception e)
            {
                if (e.Message == "There are no available taxi vehicles!")
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("");
                }
                else
                    throw;
            }

            try
            {
                Console.WriteLine("Ordering ride from 35 to 12...");
                Ride ride = scheduler.OrderRide(35, 12, RideTypeEnum.City, new DateTime(2018, 1, 1, 11, 0, 0));
                scheduler.AcceptRide(ride);
                Console.WriteLine("");
            }
            catch (Exception e)
            {
                if (e.Message == "There are no available taxi vehicles!")
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("");
                }
                else
                    throw;
            }

            Console.WriteLine("Driver with ID = 2 earned today:");
            foreach (var r in scheduler.RidesOfTaxiDriver(2))
            {
                Console.WriteLine("Price: " + r.Price);
            }
            Console.WriteLine("Total: " + scheduler.EarningsOfTaxiDriver(2));
        }
    }
}
