using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AnswerCord.Properties;

namespace AnswerCord
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (Settings.Default.IsFirstRun)
            {
                new WelcomeWindow().Show();
            }
            else
            {
                Settings.Default.Upgrade();
                new ConnectingWindow().Show();
            }
        }
    }
}
