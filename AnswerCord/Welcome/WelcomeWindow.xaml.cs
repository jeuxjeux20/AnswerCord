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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AnswerCord.Welcome;

namespace AnswerCord
{
    /// <summary>
    /// Logique d'interaction pour WelcomeWindow.xaml
    /// </summary>
    public partial class WelcomeWindow : Window
    {
        public WelcomeWindow()
        {
            InitializeComponent();
            MainFrame.Navigated += MainFrame_Navigated;
        }
        public Action NextButtonAction { get; set; }
        private bool _shouldCloseOnPrevious = true;
        private void MainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            NextButtonAction = null;
            NextButton.IsEnabled = true;
            if (e.Content is WarningPage)
            {
                _shouldCloseOnPrevious = true;
                PreviousButton.Content = "Close";
            }
            else if (_shouldCloseOnPrevious)
            {
                _shouldCloseOnPrevious = false;
                PreviousButton.Content = "< Previous";
            }
            else
            {
                PreviousButton.IsEnabled = MainFrame.CanGoBack;
            }
            if (e.Content is SetupCompletePage)
            {
                NextButton.Content = "Finish";
            }
            else
            {
                NextButton.Content = "Next >";
            }
        }

        private void PreviousButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_shouldCloseOnPrevious)
            {
                Close();
            }

            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }
        private void NextButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoForward)
            {
                MainFrame.GoForward();
            }
            else
            {
                NextButtonAction?.Invoke();
            }
        }
    }
}
