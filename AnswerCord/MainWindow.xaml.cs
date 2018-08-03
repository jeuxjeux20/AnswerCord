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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AwnserCord;
using DSharpPlus.Entities;
using VeryHotKeys.WPF;

namespace AnswerCord
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(bool displayNoServer = false)
        {
            InitializeComponent();
            DataContext = DiscordManager.Client;
            Loaded += MainWindow_Loaded;
            shouldShowNoServWarning = displayNoServer;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (shouldShowNoServWarning)
            {
                MessageBox.Show("No servers were found :( Has your account got any servers?", "Errrrrrrrrrrrr.....or!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            for (int i = 1; i < 5; i++) // 1 to 4 u see?
            {
                var list = new List<HotKeyRegisterer>();
                _registerers.Add(i, list);
                // ctrl + numpad = [number]
                var i1 = i;
                list.Add(new HotKeyRegisterer(this, () => SendMessageForAwnser(i1), HotKeyMods.Control, BaseFunctionKey + (uint)i - 1));
                // ctrl + shift + numpad = [number] apg
                var i2 = i;
                list.Add(new HotKeyRegisterer(this, () => SendMessageForAwnser(i2, true), HotKeyMods.Control | HotKeyMods.Shift, BaseFunctionKey + (uint)i - 1));
                // ctrl + alt + numpad = [number]?
                var i3 = i;
                list.Add(new HotKeyRegisterer(this, () => SendMessageForAwnser(i3, hasQuestionMark: true), HotKeyMods.Control | HotKeyMods.Alt, BaseFunctionKey + (uint)i - 1));
                // ctrl + shift + alt + numpad = [number] apg?
                var i4 = i;
                list.Add(new HotKeyRegisterer(this, () => SendMessageForAwnser(i4, true, true), HotKeyMods.Control | HotKeyMods.Shift | HotKeyMods.Alt, BaseFunctionKey + (uint)i - 1));
            }
        }

        

        // It's basically NumPad0 but you can increment it to get 1 or more.
        public const uint BaseFunctionKey = (uint)ConsoleKey.F1;
        private Dictionary<int, List<HotKeyRegisterer>> _registerers = new Dictionary<int, List<HotKeyRegisterer>>();
        private bool shouldShowNoServWarning = false;

        private void SendMessageForAwnser(int number, bool hasApg = false, bool hasQuestionMark = false)
        {
            if (!DiscordManager.HasConnectedSuccessfully || ChannelBox.SelectedItem == null)
            {
                return;
            }

            try
            {
                var awnser = GetStringForAwnser(number, hasApg, hasQuestionMark);
                DiscordManager.Client.SendMessageAsync((DiscordChannel) ChannelBox.SelectedItem, awnser);
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot send message :(", "Damn...", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private string GetStringForAwnser(int number, bool hasApg = false, bool hasQuestionMark = false)
        {
            var result = number.ToString();
            if (hasApg)
            {
                result += " apg";
            }
            if (hasQuestionMark)
            {
                result += "?";
            }

            return result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new TokenSetupWindow().ShowDialog();
        }

        private void ServerBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChannelBox.SelectedIndex = 0;
        }
    }
}
