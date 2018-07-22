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
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private bool gotOneServer = false;
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
            ProgressBar.IsIndeterminate = false;
            ProgressBar.Value = 50;
            StatusText.Text = "Downloading servers...";
            DiscordManager.Client.GuildAvailable += Client_GuildAvailable;
            await Task.Run(async () =>
            {
                await Task.Delay(6250);
                if (!gotOneServer)
                {
                    _tokenSource.Cancel();
                    await DoneDispatcher(DiscordManager.Client.Guilds.Count == 0);
                }
            });
            //var discordClient = DiscordManager.Client;
            //if (discordClient.Guilds.Count == 0)
            //{
            //    DiscordManager.Client.GuildAvailable += Client_GuildAvailable;
            //}
            //else
            //{

            //}
        }

        private async Task Client_GuildAvailable(DSharpPlus.EventArgs.GuildCreateEventArgs e)
        {
            gotOneServer = true;
            var tokenSourceToken = _tokenSource.Token;
            await Task.Factory.StartNew(async () =>
            {
                var lastCount = e.Client.Guilds.Count;
                await Task.Delay(250, tokenSourceToken);
                if (lastCount == e.Client.Guilds.Count) // download finished?
                {
                    if (tokenSourceToken.IsCancellationRequested)
                    {
                        return;
                    }
                    _tokenSource.Cancel();
                    DiscordManager.Client.GuildAvailable -= Client_GuildAvailable;
                    while (true)
                    {
                        var count = DiscordManager.Client.Guilds.Values.Count(s => s.Name != null);
                        if (count != lastCount)
                        {
                            await Dispatcher.InvokeAsync(() =>
                            {
                                ProgressBar.Value = 50 + ((double)count / lastCount) * 50;
                                StatusText.Text = $"Getting servers' names ({count}/{lastCount})";
                            });
                        }
                        else
                        {
                            break;
                        }
                        await Task.Delay(125);
                    }
                    await DoneDispatcher();
                }
            }, tokenSourceToken);
        }

        private async Task DoneDispatcher(bool warning = false)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                Done(warning);
            });
        }

        private void Done(bool warning = false)
        {
            new MainWindow(warning).Show();
            Close();
        }
    }
}
