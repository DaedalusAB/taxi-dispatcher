using System;

namespace TaxiDispatcher.App.Models
{
    public class Ride
    {
        public int RideId { get; internal set; }     //   i dont like this set! todo

        public int LocationFrom { get; }
        public int LocationTo { get; }
        public  RideTypeEnum RideType { get; }
        public DateTime Time { get; }
        public Taxi Taxi { get; }
        public int Price { get; }

        public Ride(int locationFrom, int locationTo, RideTypeEnum rideType, DateTime time, Taxi taxi, int price)
        {
            LocationFrom = locationFrom;
            LocationTo = locationTo;
            RideType = rideType;
            Time = time;
            Taxi = taxi;
            Price = price;
        }
    }
}
