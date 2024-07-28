using PvPHelper.Console;
using PvPHelper.Core;
using PvPHelper.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Windows;
using System.Threading;
using System.ComponentModel;

namespace PvPHelper.MVVM.ViewModels
{
    public class CreditViewModel : ViewModelBase
    {
        #region Bindings
        private ObservableCollection<CreditModel> _creditsItemsSource;

        public ObservableCollection<CreditModel> Credits
        {
            get { return _creditsItemsSource; }
            set { _creditsItemsSource = value; OnPropertyChanged(); }
        }

        private ObservableCollection<CreditModel> _supportersItemsSource;

        public ObservableCollection<CreditModel> Supporters
        {
            get { return _supportersItemsSource; }
            set { _supportersItemsSource = value; OnPropertyChanged(); }
        }


        #endregion
        private const string CreditsLink = "https://raw.githubusercontent.com/ItsSenko/EldenRing-PvP-Helper/dlc/credits.json";
        private const string SupportersLink = "https://raw.githubusercontent.com/ItsSenko/EldenRing-PvP-Helper/dlc/supporters.json";

        private HttpClient client;

        public CreditViewModel()
        {
            client = new();
            Credits = new();
            Supporters = new();

            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            var credits = await LoadList(CreditsLink);
            var supporters = await LoadList(SupportersLink);

            Application.Current.Dispatcher.Invoke(() => FinalizeLoad(credits, supporters));
        }
        private async Task<List<CreditModel>> LoadList(string link)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(link);

                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<List<CreditModel>>(json);
            }
            catch (Exception ex)
            {
                CommandManager.Log("Unabled to get Credits. Reason: " + ex.Message);
                return new();
            }
        }

        private void FinalizeLoad(List<CreditModel> credits, List<CreditModel> supporters)
        {
            foreach (CreditModel credit in credits)
            {
                credit.Initialize();
                Credits.Add(credit);
            }

            foreach (CreditModel supporter in supporters)
            {
                supporter.Initialize();
                Supporters.Add(supporter);
            }
        }
    }
}
