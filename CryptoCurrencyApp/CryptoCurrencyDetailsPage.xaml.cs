using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Net.Http;
using Newtonsoft.Json;
using System.Windows;
using System.Collections;

namespace CryptoCurrencyApp
{
    public partial class CryptoCurrencyDetailsPage : Window
    {
        private readonly HttpClient httpClient = new HttpClient();

        public string CurrencyId { get; set; }
        public CryptocurrencyInfo Currency { get; set; }

        public CryptoCurrencyDetailsPage()
        {
            InitializeComponent();
        }

        public CryptoCurrencyDetailsPage(string currencyId) : this()
        {
            InitializeComponent();
            CurrencyId = currencyId;
            LoadCurrencyDetails();
        }

        private async void LoadCurrencyDetails()
        {
            try
            {
                string currencyInfoUrl = $"https://api.coincap.io/v2/assets/{CurrencyId}";
                string marketsUrl = $"https://api.coincap.io/v2/assets/{CurrencyId}/markets";
                HttpResponseMessage currencyInfoResponse = await httpClient.GetAsync(currencyInfoUrl);
                currencyInfoResponse.EnsureSuccessStatusCode();
                string currencyInfoResponseBody = await currencyInfoResponse.Content.ReadAsStringAsync();
                CryptocurrencyInfoResponse cryptocurrencyInfo = JsonConvert.DeserializeObject<CryptocurrencyInfoResponse>(currencyInfoResponseBody);
                HttpResponseMessage marketsResponse = await httpClient.GetAsync(marketsUrl);
                marketsResponse.EnsureSuccessStatusCode();
                string marketsResponseBody = await marketsResponse.Content.ReadAsStringAsync();
                CryptocurrencyMarketResponse marketResponse = JsonConvert.DeserializeObject<CryptocurrencyMarketResponse>(marketsResponseBody);
                CryptocurrencyInfo cryptocurrencyInfos = cryptocurrencyInfo?.Data;
                if (cryptocurrencyInfos != null)
                {
                        nameTextBlock.Text = cryptocurrencyInfos.name;
                        priceTextBlock.Text = cryptocurrencyInfos.priceUsd;
                        volumeTextBlock.Text = cryptocurrencyInfos.volumeUsd24Hr;
                        priceChangeTextBlock.Text = cryptocurrencyInfos.changePercent24Hr;
                    
                }
                List<CryptocurrencyMarket> marketl = marketResponse?.Data;
                if (marketl != null)
                {
                    foreach (CryptocurrencyMarket market in marketl)
                    {
                        var listItem = new ListBoxItem
                        {
                            Content = $"{market.exchangeId}  - {market.priceUsd}$"
                        };
                        marketsListBox.Items.Add(listItem);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetMarketsText(IEnumerable<CryptocurrencyMarket> markets)
        {
            if (markets == null)
                return string.Empty;

            List<string> marketTexts = new List<string>();
            foreach (CryptocurrencyMarket market in markets)
            {
                string marketText = $"{market.exchangeId} - {market.priceUsd}";
                marketTexts.Add(marketText);
            }

            return string.Join("\n", marketTexts);
        }
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
}
