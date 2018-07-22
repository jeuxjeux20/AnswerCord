using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace AnswerCord
{
    /// <summary>
    /// Logique d'interaction pour Connecting.xaml
    /// </summary>
    public partial class ConnectingWindow : Window
    {
        public ConnectingWindow()
        {
            InitializeComponent();
            Loaded += ConnectingWindow_Loaded;
        }

        private async void ConnectingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!DiscordManager.HasConnectedSuccessfully)
            {
                await DiscordManager.InitialiseWithToken(Properties.Settings.Default.Token, Properties.Settings.Default.AccountType);
            }
            //ProgressBar.IsIndeterminate = false;
            //ProgressBar.Value = 50;
            //StatusText.Text = "Downloading servers...";
            //var discordClient = DiscordManager.Client;
            //if (discordClient.Guilds.Count == 0)
            //{
            //    DiscordManager.Client.GuildAvailable += Client_GuildAvailable;
            //}
            //else
            //{
                Done();
            //}
        }

        // private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        //private Task Client_GuildAvailable(DSharpPlus.EventArgs.GuildCreateEventArgs e)
        //{
        //    var tokenSourceToken = _tokenSource.Token;
        //    return Task.Factory.StartNew(async () =>
        //    {
        //        await Dispatcher.InvokeAsync(() => { StatusText.Text = $"Downloaded server {e.Client.Guilds.Count}"; });           
        //        var lastCount = e.Client.Guilds.Count;
        //        await Task.Delay(250);
        //        if (lastCount == e.Client.Guilds.Count) // download finished?
        //        {
        //            if (tokenSourceToken.IsCancellationRequested)
        //            {
        //                return;
        //            }
        //            _tokenSource.Cancel();
        //            DiscordManager.Client.GuildAvailable -= Client_GuildAvailable;
        //            await Dispatcher.InvokeAsync(Done);
        //        }
        //    }, tokenSourceToken);
        //}

        private void Done()
        {
            new MainWindow().Show();
            Close();
        }
    }
}
