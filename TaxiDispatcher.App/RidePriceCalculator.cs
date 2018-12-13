using System;

namespace TaxiDispatcher.App
{
    public class RidePriceCalculator
    {
        private const int ExpensiveHoursStart = 22;
        private const int ExpensiveHoursEnd = 6;

        public int CalculatePrice(int locationFrom, int locationTo, RideTypeEnum rideType, DateTime time, TaxiCompany company)
        {
            var price = company.PricePerUnitOfDistance * Math.Abs(locationFrom - locationTo);

            if (rideType == RideTypeEnum.InnerCity)
            {
                price *= 2;
            }

            if (time.Hour < ExpensiveHoursEnd || time.Hour > ExpensiveHoursStart)
            {
                price *= 2;
            }

            return price;
        }
    }
}
