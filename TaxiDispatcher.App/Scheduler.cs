using System;
using System.Collections.Generic;

namespace TaxiDispatcher.App
{
    public class Scheduler
    {
        protected Taxi taxi1 = new Taxi { TaxiDriverId = 1, TaxiDriverName = "Predrag", TaxiCompany = "Naxi", Location = 1};
        protected Taxi taxi2 = new Taxi { TaxiDriverId = 2, TaxiDriverName = "Nenad", TaxiCompany = "Naxi", Location = 4 };
        protected Taxi taxi3 = new Taxi { TaxiDriverId = 3, TaxiDriverName = "Dragan", TaxiCompany = "Alfa", Location = 6 };
        protected Taxi taxi4 = new Taxi { TaxiDriverId = 4, TaxiDriverName = "Goran", TaxiCompany = "Gold", Location = 7 };

        public Ride OrderRide(int locationFrom, int locationTo, int rideType, DateTime time)
        {
            var bestTaxi = FindBestTaxi(locationFrom);
            var ride = CreateRide(locationFrom, locationTo, bestTaxi);
            CalculatePrice(locationFrom, locationTo, rideType, time, bestTaxi, ride);

            Console.WriteLine("Ride ordered, price: " + ride.Price);

            return ride;
        }

        private Taxi FindBestTaxi(int locationFrom)
        {
            var minTaxi = taxi1;
            var minDistance = Math.Abs(taxi1.Location - locationFrom);

            if (Math.Abs(taxi2.Location - locationFrom) < minDistance)
            {
                minTaxi = taxi2;
                minDistance = Math.Abs(taxi2.Location - locationFrom);
            }

            if (Math.Abs(taxi3.Location - locationFrom) < minDistance)
            {
                minTaxi = taxi3;
                minDistance = Math.Abs(taxi3.Location - locationFrom);
            }

            if (Math.Abs(taxi4.Location - locationFrom) < minDistance)
            {
                minTaxi = taxi4;
                minDistance = Math.Abs(taxi4.Location - locationFrom);
            }

            if (minDistance > 15)
                throw new Exception("There are no available taxi vehicles!");
            return minTaxi;
        }

        private static Ride CreateRide(int locationFrom, int locationTo, Taxi bestTaxi)
        {
            Ride ride = new Ride();
            ride.TaxiDriverId = bestTaxi.TaxiDriverId;
            ride.LocationFrom = locationFrom;
            ride.LocationTo = locationTo;
            ride.TaxiDriverName = bestTaxi.TaxiDriverName;
            return ride;
        }

        private static void CalculatePrice(int locationFrom, int locationTo, int rideType, DateTime time, Taxi bestTaxi, Ride ride)
        {
            switch (bestTaxi.TaxiCompany)
            {
                case "Naxi":
                    {
                        ride.Price = 10 * Math.Abs(locationFrom - locationTo);
                        break;
                    }
                case "Alfa":
                    {
                        ride.Price = 15 * Math.Abs(locationFrom - locationTo);
                        break;
                    }
                case "Gold":
                    {
                        ride.Price = 13 * Math.Abs(locationFrom - locationTo);
                        break;
                    }
                default:
                    {
                        throw new Exception("Ilegal company");
                    }
            }

            if (rideType == Constants.InterCity)
            {
                ride.Price *= 2;
            }

            if (time.Hour < 6 || time.Hour > 22)
            {
                ride.Price *= 2;
            }
        }

        public void AcceptRide(Ride ride)
        {
            InMemoryRideDataBase.SaveRide(ride);

            if (taxi1.TaxiDriverId == ride.TaxiDriverId)
            {
                taxi1.Location = ride.LocationTo;
            }

            if (taxi2.TaxiDriverId == ride.TaxiDriverId)
            {
                taxi2.Location = ride.LocationTo;
            }

            if (taxi3.TaxiDriverId == ride.TaxiDriverId)
            {
                taxi3.Location = ride.LocationTo;
            }

            if (taxi4.TaxiDriverId == ride.TaxiDriverId)
            {
                taxi4.Location = ride.LocationTo;
            }

            Console.WriteLine("Ride accepted, waiting for driver: " + ride.TaxiDriverName);
        }

        public List<Ride> GetRideList(int driverId)
        {
            List<Ride> rides = new List<Ride>();
            List<int> ids = InMemoryRideDataBase.GetRideIds();
            foreach (int id in ids)
            {
                Ride ride = InMemoryRideDataBase.GetRide(id);
                if (ride.TaxiDriverId == driverId)
                    rides.Add(ride);
            }

            return rides;
        }
    }
}
