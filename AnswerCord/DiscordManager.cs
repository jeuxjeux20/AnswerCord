using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DSharpPlus;

namespace AnswerCord
{
    public static class DiscordManager
    {
        public static DiscordClient Client { get; private set; }
        private static string _lastUsedToken = null;
        public static bool HasConnectedSuccessfully { get; private set; } = false;
        public static async Task InitialiseWithToken(string token)
        {
            if (_lastUsedToken == token)
            {
                return;
            }
            if (Client != null && HasConnectedSuccessfully)
            {
#pragma warning disable 4014
                Task.Run(() =>
#pragma warning restore 4014
                {
                    try
                    {
                        Client.Dispose();
                    }
                    catch (Exception)
                    {
                        
                    }
                });
            }
            HasConnectedSuccessfully = false;
            Client = new DiscordClient(new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot
            });
            await Client.ConnectAsync();
            _lastUsedToken = token;
            HasConnectedSuccessfully = true;
        }
    }
}
