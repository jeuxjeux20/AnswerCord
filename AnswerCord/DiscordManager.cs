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
        private static TokenType? lastUsedType = null;
        public static bool HasConnectedSuccessfully { get; private set; } = false;
        public static async Task InitialiseWithToken(string token, TokenType? type = null)
        {
            if (_lastUsedToken == token && lastUsedType == type)
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
                TokenType = type ?? GetTokenTypeForToken(token)
            });
            await Client.ConnectAsync();
            _lastUsedToken = token;
            HasConnectedSuccessfully = true;
        }
        public static TokenType GetTokenTypeForToken(string token)
        {
            if (token.Length < 59)
            {
                throw new ArgumentException("Token too short");
            }
            switch (token.Length)
            {
                case 59:
                    return TokenType.Bot;
                case 88:
                    return TokenType.User;
                default:
                    throw new ArgumentException("Invalid token");
            }
        }
    }
}
