using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PvPHelper.Console;
using CommandManager = PvPHelper.Console.CommandManager;

namespace PvPHelper.MVVM.Views.UserControls
{
    /// <summary>
    /// Interaction logic for Console.xaml
    /// </summary>
    public partial class Console : UserControl
    {
        public Console()
        {
            InitializeComponent();

            commandManager = CommandManager.RegisterConsole(new Action<string>(s => { Log(s); }));
            LogLoaded.Invoke();
        }
        #region Data Bindings
        public static event Action LogLoaded = new(() => { });
        private static ObservableCollection<string> defaultConsoleOutput = new();

        public ObservableCollection<string> ConsoleOutput
        {
            get { return (ObservableCollection<string>)GetValue(ConsoleOutputProperty); }
            set { SetValue(ConsoleOutputProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConsoleOutput.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConsoleOutputProperty =
            DependencyProperty.Register("ConsoleOutput", typeof(ObservableCollection<string>), typeof(Console), new PropertyMetadata(defaultConsoleOutput));




        public string InputText
        {
            get { return (string)GetValue(InputTextProperty); }
            set { SetValue(InputTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InputText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputTextProperty =
            DependencyProperty.Register("InputText", typeof(string), typeof(Console), new PropertyMetadata(string.Empty));



        private CommandManager commandManager; 

        #endregion
        public void Log(string text)
        {
            if (text.Length > 74)
            {
                var startString = text.Substring(0, 64);

                var endString = text.Substring(64);
                Log(startString);
                Log(endString);
                return;
            }
            ConsoleOutput.Add(text);
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
                        commandManager.HandleInput(InputText);
                    }
                    catch (InvalidCommandException ex)
                    {
                        Log($"Invalid Command: {ex.Message}");
                    }
                }
                else
                    Log(InputText);

                InputText = string.Empty;
            }
        }
    }
}
