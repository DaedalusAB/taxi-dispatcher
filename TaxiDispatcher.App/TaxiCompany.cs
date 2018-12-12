namespace TaxiDispatcher.App
{
    public class TaxiCompany
    {
        public string Name { get; set; }
        public int PricePerUnitOfDistance { get; set; }

        public TaxiCompany(string name, int pricePerUnitOfDistance)
        {
            Name = name;
            PricePerUnitOfDistance = pricePerUnitOfDistance;
        }
    }
}
