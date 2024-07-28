using PvPHelper.Console;
using PvPHelper.MVVM.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace PvPHelper
{
    public class Program
    {
        public delegate void LogHandler(string message);
        public static event LogHandler OnLogged;

        public delegate void UnhandledExceptionEventHandler(Exception args);
        public static event UnhandledExceptionEventHandler OnUnhandledException;

        [STAThread()]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            ProgramLogs.Initialize();

            (new Program()).Run();
        }


        private static void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            OnUnhandledException.Invoke(e.Exception);

            Thread.CurrentThread.IsBackground = true;
            Thread.CurrentThread.Name = "Dead thread";

            Thread.CurrentThread.Abort();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            OnUnhandledException.Invoke(e.ExceptionObject as Exception);

            var ex = e.ExceptionObject as Exception;
            InformationDialog dialog = new($"Uncaught exception: {ex.Message}");
            dialog.ShowDialog();
        }

        void Run()
        {
            try
            {
                App app = new App();
                app.Dispatcher.UnhandledException += Dispatcher_UnhandledException;


                app.InitializeComponent();

                app.Run();
            }
            catch { };
        }

        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                OnUnhandledException.Invoke(e.Exception);
                e.Handled = true;
                InformationDialog dialog = new($"Uncaught exception: {e.Exception.Message}");
                dialog.ShowDialog();
            }
            catch { }
        }

        public static void InvokeLog(string message)
        {
            OnLogged?.Invoke(message);
        }
    }
}
