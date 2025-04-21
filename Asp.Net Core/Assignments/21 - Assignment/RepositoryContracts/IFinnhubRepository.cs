using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IFinnhubRepository
    {
        /// <summary>
        /// Gets the company profile based on the given stock symbol.
        /// </summary>
        /// <param name="stockSymbol">stock symbol to filter</param>
        /// <returns>Company profile which contains the given stock symbol in format of dictionary (string:object) </returns>
        /// https://finnhub.io/api/v1/stock/profile2?symbol={symbol}&token={token}
        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);
        /// <summary>
        /// Gets the stock price quote based on the given stock symbol.
        /// </summary>
        /// <param name="stockSymbol">stock symbol to filter</param>
        /// <returns>Stock price quote which contains the given stock symbol in format of dictionary (string:object) 
        /// https://finnhub.io/api/v1/quote?symbol={symbol}&token={token}
        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
        /// <summary>
        /// Get all the stocks which using USD as currency.
        /// </summary>
        /// <returns>Stock in format of dictionary (string:string) list</returns>
        /// https://finnhub.io/api/v1/stock/symbol?exchange=US&token={token}
        Task<List<Dictionary<string, string>>?> GetStocks();
        /// <summary>
        /// Search stocks based on the given stock symbol.
        /// </summary>
        /// <param name="stockSymbolToSearch">stock symbol to filter</param>
        /// <returns>Filtered stock in format of dictionary (string:object)</returns>
        /// https://finnhub.io/api/v1/search?q={stockNameToSearch}&token={token}
        Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch);
    }
}

