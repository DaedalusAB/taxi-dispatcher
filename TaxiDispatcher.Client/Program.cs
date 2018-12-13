using System;
using System.Collections.Generic;
using TaxiDispatcher.App;

namespace TaxiDispatcher.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            var taxiDriverRepo = new TaxiDriverRepo(new List<Taxi>()
            {
                new Taxi(1, "Predrag", new TaxiCompany("Naxi", 10), 1),
                new Taxi(2, "Nenad", new TaxiCompany("Naxi", 10), 4),
                new Taxi(3, "Dragan", new TaxiCompany("Alfa", 15), 6),
                new Taxi(4, "Goran", new TaxiCompany("Gold", 13), 7)
            });

            var scheduler = new Scheduler(taxiDriverRepo);

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
            int total = 0;
            foreach (Ride r in scheduler.GetRideList(2))
            {
                total += r.Price;
                Console.WriteLine("Price: " + r.Price);
            }
            Console.WriteLine("Total: " + total);
        }
    }
}
