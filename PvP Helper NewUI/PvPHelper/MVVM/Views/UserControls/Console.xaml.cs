using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using PvPHelper.Console;
using PvPHelper.Console.Commands;
using CommandManager = PvPHelper.Console.CommandManager;

namespace PvPHelper.MVVM.Views.UserControls
{
    /// <summary>
    /// Interaction logic for Console.xaml
    /// </summary>
    public partial class Console : UserControl, INotifyPropertyChanged
    {
        public Console()
        {
            DataContext = this;
            InitializeComponent();
        }
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
        #region Data Bindings
        private ObservableCollection<string> _itemsSource = new();

        public ObservableCollection<string> ConsoleOutput
        {
            get { return _itemsSource; }
            set { _itemsSource = value; OnPropertyChanged(); }
        }
        private string _inputText;

        public string InputText
        {
            get { return _inputText; }
            set { _inputText = value; OnPropertyChanged(); }
        }

        private CommandManager _commandManager; 

        public CommandManager CommandManager
        {
            get { return _commandManager; }
            set { _commandManager = value; }
        }

        #endregion

        public void Log(string text)
        {
            string SystemTime = DateTime.Now.ToString("[" + "hh:mm:ss" + "]");

            ConsoleOutput.Add($"{SystemTime} {text}");
            Scroller.ScrollToBottom();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (string.IsNullOrEmpty(InputText))
                    return;

                if (InputText.StartsWith("/"))
                {
                    try
                    {
                        CommandManager.HandleInput(InputText);
                    }
                    catch (InvalidCommandException ex)
                    {
                        Log($"Inavalid Command: {ex.Message}");
                    }
                }
                else
                    Log(InputText);

                InputText = string.Empty;
            }
        }
    }
}
