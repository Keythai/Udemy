namespace ServiceContracts.FinnhubService
{
    public interface IFinnhubPriceQuoteService
    {
        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
    }
}
