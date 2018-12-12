namespace TaxiDispatcher.App
{
    public class Ride
    {
        public int RideId { get; set; }
        public int LocationFrom { get; set; }
        public int LocationTo { get; set; }
        public int TaxiDriverId { get; set; }       //  double check this
        public string TaxiDriverName { get; set; }
        public int Price { get; set; }
    }
}
