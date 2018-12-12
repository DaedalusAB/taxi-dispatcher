namespace TaxiDispatcher.App
{
    public class Ride
    {
        public int RideId { get; set; }
        public int LocationFrom { get; set; }
        public int LocationTo { get; set; }
        public Taxi Taxi { get; set; }
        public int Price { get; set; }
    }
}
