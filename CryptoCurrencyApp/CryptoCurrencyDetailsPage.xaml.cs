using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Net.Http;
using Newtonsoft.Json;
using System.Windows;
using System.Collections;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using System.Globalization;

namespace CryptoCurrencyApp
{
    public partial class CryptoCurrencyDetailsPage : Page
    {
        private readonly HttpClient httpClient = new HttpClient();

        public string CurrencyId { get; set; }
        private Frame mainFrame;
        public CryptocurrencyInfo Currency { get; set; }

        public CryptoCurrencyDetailsPage()
        {
            InitializeComponent();
        }

        public CryptoCurrencyDetailsPage(string currencyId, Frame mF) : this()
        {
            CurrencyId = currencyId;
            LoadCurrencyDetails();
            LoadCryptoPrices();
            this.mainFrame = mF;
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
                Currency = cryptocurrencyInfos;
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

        private void Button_MainPage_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new PageMain(mainFrame));
        }
        public async void LoadCryptoPrices()
        {
            try
            {
                string cryptoHistoryUrl = $"https://api.coincap.io/v2/assets/{CurrencyId}/history?interval=d1";
                HttpResponseMessage cryptoHistoryResponse = await httpClient.GetAsync(cryptoHistoryUrl);
                cryptoHistoryResponse.EnsureSuccessStatusCode();
                string cryptoHistoryResponseBody = await cryptoHistoryResponse.Content.ReadAsStringAsync();
                CryptoHistoryResponse historyResponse = JsonConvert.DeserializeObject<CryptoHistoryResponse>(cryptoHistoryResponseBody);
                SeriesCollection seriesCollection = new SeriesCollection();



                string currencyInfoUrl = $"https://api.coincap.io/v2/assets/{CurrencyId}";
                HttpResponseMessage currencyInfoResponse = await httpClient.GetAsync(currencyInfoUrl);
                currencyInfoResponse.EnsureSuccessStatusCode();
                string currencyInfoResponseBody = await currencyInfoResponse.Content.ReadAsStringAsync();
                CryptocurrencyInfoResponse cryptocurrencyInfo = JsonConvert.DeserializeObject<CryptocurrencyInfoResponse>(currencyInfoResponseBody);
                CryptocurrencyInfo cryptocurrencyInfos = cryptocurrencyInfo?.Data;
                


                LineSeries lineSeries = new LineSeries
                {
                    Title = cryptocurrencyInfos.name,
                    Values = new ChartValues<double>()
                };

                // Добавление данных в серию графика
                foreach (CryptoPriceData priceData in historyResponse.Data)
                {
                    
                    DateTime timestamp = DateTimeOffset.FromUnixTimeMilliseconds(priceData.time).DateTime;
                    lineSeries.Values.Add(Convert.ToDouble(priceData.priceUsd, CultureInfo.InvariantCulture));
                }

                seriesCollection.Add(lineSeries);
                chart.Series = seriesCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    

     
}
