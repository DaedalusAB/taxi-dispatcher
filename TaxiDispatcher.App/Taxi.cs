namespace TaxiDispatcher.App
{
    public partial class Scheduler
    {
        public class Taxi
        {
            public int Taxi_driver_id { get; set; }
            public string Taxi_driver_name { get; set; }
            public string Taxi_company { get; set; }
            public int Location { get; set; }
        }
    }
}
