namespace ServiceContracts
{
    public interface IFinnhubService
    {
        // https://finnhub.io/api/v1/stock/profile2?symbol={symbol}&token={token}
        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);
        // https://finnhub.io/api/v1/quote?symbol={symbol}&token={token}
        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
    }
}
