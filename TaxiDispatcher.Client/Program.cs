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

            Console.WriteLine("Ordering ride from 5 to 0...");
            var ride1 = scheduler.OrderCityRide(5, 0, new DateTime(2018, 1, 1, 23, 0, 0));
            scheduler.AcceptRide(ride1);
            Console.WriteLine("");


            Console.WriteLine("Ordering ride from 0 to 12...");
            var ride2 = scheduler.OrderInnerCityRide(0, 12, new DateTime(2018, 1, 1, 9, 0, 0));
            scheduler.AcceptRide(ride2);
            Console.WriteLine("");


            Console.WriteLine("Ordering ride from 5 to 0...");
            var ride3 = scheduler.OrderCityRide(5, 0, new DateTime(2018, 1, 1, 11, 0, 0));
            scheduler.AcceptRide(ride3);
            Console.WriteLine("");


            Console.WriteLine("Ordering ride from 35 to 12...");
            var ride4 = scheduler.OrderCityRide(35, 12, new DateTime(2018, 1, 1, 11, 0, 0));
            scheduler.AcceptRide(ride4);
            Console.WriteLine("");

            Console.WriteLine("Driver with ID = 2 earned today:");
            foreach (var r in scheduler.RidesOfTaxiDriver(2))
            {
                Console.WriteLine("Price: " + r.Price);
            }
            Console.WriteLine("Total: " + scheduler.EarningsOfTaxiDriver(2));
        }
    }
}
