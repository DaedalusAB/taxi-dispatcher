namespace TaxiDispatcher.App.Models
{
    public class TaxiCompany
    {
        public string Name { get; }
        public int PricePerUnitOfDistance { get; }

        public TaxiCompany(string name, int pricePerUnitOfDistance)
        {
            Name = name;
            PricePerUnitOfDistance = pricePerUnitOfDistance;
        }
    }
}
