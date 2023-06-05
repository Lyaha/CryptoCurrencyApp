using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CryptoCurrencyApp
{
    /// <summary>
    /// Логика взаимодействия для PageMain.xaml
    /// </summary>
    public partial class PageMain : Page
    {

        private readonly HttpClient httpClient = new HttpClient();
        private List<Cryptocurrency> cryptocurrencies;
        public string currencyId;
        private Frame mainFrame;
        public PageMain(Frame mF)
        {
            InitializeComponent();
            LoadCryptocurrencies();
            mainFrame = mF;
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
                currencyId = selectedCryptocurrency.Id;
                mainFrame.Navigate(new CryptoCurrencyDetailsPage(currencyId, mainFrame));
            }
        }
    }
}