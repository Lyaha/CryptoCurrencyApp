using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrencyApp
{
    public class CryptoPriceData
    {
        public string priceUsd { get; set; }
        public long time { get; set; }
    }

    public class CryptoHistoryResponse
    {
        public List<CryptoPriceData> Data { get; set; }
    }
    public class CryptocurrencyInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public string priceUsd { get; set; }
        public string volumeUsd24Hr { get; set; }
        public string changePercent24Hr { get; set; }
    }
    public class CryptocurrencyInfoResponse
    {
        public CryptocurrencyInfo Data { get; set; }
    }

    public class CryptocurrencyMarket
    {
        public string exchangeId { get; set; }
        public string priceUsd { get; set; }
    }

    public class CryptocurrencyMarketResponse
    {
        public List<CryptocurrencyMarket> Data { get; set; }
    }
    public class Cryptocurrency
    {
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal PriceUsd { get; set; }
    }

    public class CryptocurrencyResponse
    {
        public List<Cryptocurrency> Data { get; set; }
    }
}
