﻿using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Repositories;
using Exceptions;
using ServiceContracts.FinnhubService;

namespace Services.FinnhubService
{
    public class FinnhubPriceQuoteService : IFinnhubPriceQuoteService
    {
        private readonly IFinnhubRepository _finnhubRepository;
        public FinnhubPriceQuoteService(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }
        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            try
            {
                return await _finnhubRepository.GetStockPriceQuote(stockSymbol);
            }
            catch (Exception ex)
            {
                throw new FinnhubException("Unable to connect to Finnhub", ex);
            }
        }

    }
}
