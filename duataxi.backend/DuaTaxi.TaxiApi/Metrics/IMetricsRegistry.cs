namespace DuaTaxi.Services.TaxiApi.Metrics
{
    public interface IMetricsRegistry
    {
        void IncrementFindDiscountsQuery();
    }
}