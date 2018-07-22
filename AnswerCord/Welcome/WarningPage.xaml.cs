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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnswerCord.Welcome
{
    /// <summary>
    /// Logique d'interaction pour WarningPage.xaml
    /// </summary>
    public partial class WarningPage : Page
    {
        public WarningPage()
        {
            InitializeComponent();
            Loaded += WarningPage_Loaded;
        }

        private void WarningPage_Loaded(object sender, RoutedEventArgs e)
        {
            var w = Window.GetWindow(this);
            if (w is WelcomeWindow window)
            {
                window.NextButtonAction = () =>
                {
                    NavigationService?.Navigate(new TokenSetupPage());
                };
            }
        }
    }
}
