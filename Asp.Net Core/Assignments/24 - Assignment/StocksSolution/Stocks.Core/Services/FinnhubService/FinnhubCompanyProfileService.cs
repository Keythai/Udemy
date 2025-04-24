using System.Text.Json;
using Repositories;
using Exceptions;
using ServiceContracts.FinnhubService;

namespace Services.FinnhubService
{
    public class FinnhubCompanyProfileService : IFinnhubCompanyProfileService
    {
        private readonly IFinnhubRepository _finnhubRepository;
        public FinnhubCompanyProfileService(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }
        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            try
            {
                return await _finnhubRepository.GetCompanyProfile(stockSymbol);
            }
            catch (Exception ex)
            {
                throw new FinnhubException("Unable to connect to Finnhub", ex);
            }
        }
    }
}
