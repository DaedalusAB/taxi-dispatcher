using System;

namespace TaxiDispatcher.App
{
    public class Ride
    {
        public int RideId { get; set; }     //   i dont like this set! todo

        public int LocationFrom { get; }
        public int LocationTo { get; }
        public  RideTypeEnum RideType { get; }
        public DateTime Time { get; }
        public Taxi Taxi { get; }

        public int Price
        {
            get
            {
                var price = Taxi.Company.PricePerUnitOfDistance * Distance;

                if (RideType == RideTypeEnum.InnerCity)
                {
                    price *= 2;
                }

                if (Time.Hour < 6 || Time.Hour > 22)
                {
                    price *= 2;
                }

                return price;
            }
        }

        private int Distance =>
            Math.Abs(LocationTo - LocationFrom);

        public Ride(int locationFrom, int locationTo, RideTypeEnum rideType, DateTime time, Taxi taxi)
        {
            LocationFrom = locationFrom;
            LocationTo = locationTo;
            RideType = rideType;
            Time = time;
            Taxi = taxi;
        }
    }
}
