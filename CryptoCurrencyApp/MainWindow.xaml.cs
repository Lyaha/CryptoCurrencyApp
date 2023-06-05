using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Newtonsoft.Json;

namespace CryptoCurrencyApp
{
    public partial class MainWindow : Window
    {
        private readonly HttpClient httpClient = new HttpClient();
        private List<Cryptocurrency> cryptocurrencies;

        public MainWindow()
        {
            InitializeComponent();
            LoadCryptocurrencies(); 
        }

        private async void LoadCryptocurrencies()
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync("https://api.coincap.io/v2/assets");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                CryptocurrencyResponse cryptocurrencyResponse = JsonConvert.DeserializeObject<CryptocurrencyResponse>(responseBody);

                cryptocurrencies = cryptocurrencyResponse?.Data;
                if (cryptocurrencies != null)
                {
                    foreach (Cryptocurrency cryptocurrency in cryptocurrencies)
                    {
                        var listItem = new ListBoxItem
                        {
                            Content = $"{cryptocurrency.Name} ({cryptocurrency.Symbol}) - {cryptocurrency.PriceUsd}$"
                        };
                        CryptocurrencyListBox.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CurrencyListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = CryptocurrencyListBox.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < cryptocurrencies.Count)
            {
                Cryptocurrency selectedCryptocurrency = cryptocurrencies[selectedIndex];
                string currencyId = selectedCryptocurrency.Id;
                CryptoCurrencyDetailsPage detailsPage = new CryptoCurrencyDetailsPage(currencyId);
                detailsPage.Show();
                Hide();
            }
        }
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
