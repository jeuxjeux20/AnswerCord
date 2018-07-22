using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AwnserCord;
using DSharpPlus;

namespace AnswerCord.Welcome
{
    /// <summary>
    /// Logique d'interaction pour TokenSetupPage.xaml
    /// </summary>
    public partial class TokenSetupPage : Page
    {
        private WelcomeWindow _welcomeWindow = null;
        private bool _changed = false;
        private bool firstTimeRadioChecked = true;
        private bool _allOk = DiscordManager.HasConnectedSuccessfully;
        private TokenType tokenType = Properties.Settings.Default.AccountType;
        private bool shouldNotVerify = false;

        public TokenSetupPage()
        {
            InitializeComponent();
            Loaded += TokenSetupPage_Loaded;
        }

        private void TokenSetupPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (tokenType == TokenType.Bot)
            {
                BotRadioButton.IsChecked = true;
            }
            else
            {
                UserRadioButton.IsChecked = true;
            }
            var window = Window.GetWindow(this);
            switch (window)
            {
                case WelcomeWindow w:
                    _welcomeWindow = w;
                    w.NextButtonAction = () =>
                    {
                        w.MainFrame.Navigate(new SetupCompletePage());
                        UpdateSettings();
                    };
                    break;
                case TokenSetupWindow tokenWindow:
                    shouldNotVerify = true;
                    TokenTextBox.Text = Properties.Settings.Default.Token;
                    tokenWindow.Closing += TokenWindow_Closing;
                    break;
            }

            ChangeNextButtonStatus(DiscordManager.HasConnectedSuccessfully);
        }

        private void UpdateSettings()
        {
            Properties.Settings.Default.Token = TokenTextBox.Text;
            Properties.Settings.Default.AccountType = tokenType;
            Properties.Settings.Default.IsFirstRun = false;
            Properties.Settings.Default.Save();
        }

        private void TokenWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_allOk)
            {
                UpdateSettings();
            }
            else
            {
                var result = MessageBox.Show(
                    "You must provide a valid token to use AwnserCord, do you want to keep your old one ?",
                    "What is this title supposed to say?",
                    MessageBoxButton.OKCancel, MessageBoxImage.Error, MessageBoxResult.OK);
                if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }

            if (e.Cancel || !_changed) return;
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void ChangeNextButtonStatus(bool enabled)
        {
            if (_welcomeWindow != null)
            {
                _welcomeWindow.NextButton.IsEnabled = enabled;
            }
        }

        private async void TokenInputChanged(object sender, TextChangedEventArgs e)
        {

            if (shouldNotVerify)
            {
                shouldNotVerify = false;
                return;
            }
            _allOk = false;
            _changed = true;
            ChangeNextButtonStatus(false);
            var previousLength = TokenTextBox.Text.Length;
            TokenTextBox.Text = new string(TokenTextBox.Text.Where(c => c != '"').ToArray());
            //if (TokenTextBox.Text.Length != previousLength)
            //{
            //    shouldNotVerify = true;
            //}
            TokenStatus.Text = "Trying to connect...";
            TokenStatus.Style = (Style)TokenStatus.Resources["NormalStyle"];
            if (TokenTextBox.Text.Length < 59)
            {
                TokenStatus.Text = "Token too short";
                TokenStatus.Style = (Style)TokenStatus.Resources["ErrorStyle"];
                return;
            }
            try
            {
                TokenTextBox.IsEnabled = false;
                await DiscordManager.InitialiseWithToken(TokenTextBox.Text, tokenType);
                _allOk = true;
                TokenStatus.Text = "Succesfully connected!";
                TokenStatus.Style = (Style)TokenStatus.Resources["ValidStyle"];
                ChangeNextButtonStatus(true);
            }
            catch (Exception)
            {
                TokenStatus.Text = "Could not connect, is your token valid?";
                TokenStatus.Style = (Style)TokenStatus.Resources["ErrorStyle"];
            }
            finally
            {
                TokenTextBox.IsEnabled = true;
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            tokenType = TokenType.User;
            if (!firstTimeRadioChecked)
            {
                TokenInputChanged(null, null);
            }
            else
            {
                firstTimeRadioChecked = false;
            }
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            tokenType = TokenType.Bot;
            if (!firstTimeRadioChecked)
            {
                TokenInputChanged(null, null);
            }
            else
            {
                firstTimeRadioChecked = false;
            }
        }
    }
}
