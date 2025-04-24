using System.Text.Json;
using Repositories;
using Exceptions;
using ServiceContracts.FinnhubService;
using ServiceContracts;

namespace Services.FinnhubService
{
    public class FinnhubStocksService : IFinnhubStocksService
    {
        private readonly IFinnhubRepository _finnhubRepository;
        public FinnhubStocksService(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }
        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            try
            {
                return await _finnhubRepository.GetStocks();
            }
            catch (Exception ex)
            {
                throw new FinnhubException("Unable to connect to Finnhub", ex);
            }
        }

    }
}
