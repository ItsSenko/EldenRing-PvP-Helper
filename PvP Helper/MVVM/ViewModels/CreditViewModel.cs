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

namespace PvPHelper.MVVM.ViewModels
{
    public class CreditViewModel : ViewModelBase
    {
        #region Bindings
        private object creditLock = new();
        private ObservableCollection<CreditModel> _creditsItemsSource;

        public ObservableCollection<CreditModel> Credits
        {
            get { return _creditsItemsSource; }
            set 
            {
                _creditsItemsSource = value;
                BindingOperations.EnableCollectionSynchronization(Credits, creditLock);
                OnPropertyChanged(); 
            }
        }

        #endregion
        private string CreditsLink = "https://raw.githubusercontent.com/ItsSenko/EldenRing-PvP-Helper/dlc/credits.json";
        private string SupportersLink = "https://raw.githubusercontent.com/ItsSenko/EldenRing-PvP-Helper/dlc/supporters.txt";

        private HttpClient client;

        public CreditViewModel()
        {
            client = new();
            Credits.Clear();
        }

        private async Task LoadCredits()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(CreditsLink);

                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();

                List<CreditModel> credits = JsonConvert.DeserializeObject<List<CreditModel>>(json);

                foreach (CreditModel credit in credits)
                {
                    credit.Initialize();
                    Credits.Add(credit);
                }
            }
            catch (Exception ex)
            {
                CommandManager.Log("Unabled to get Credits. Reason: " + ex.Message);
            }
        }
    }
}
