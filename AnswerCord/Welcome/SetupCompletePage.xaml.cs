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
    /// Logique d'interaction pour SetupCompletePage.xaml
    /// </summary>
    public partial class SetupCompletePage : Page
    {
        public SetupCompletePage()
        {
            InitializeComponent();
            Loaded += SetupCompletePage_Loaded;
        }

        private void SetupCompletePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is WelcomeWindow w)
            {
                w.NextButtonAction = () =>
                {
                    new ConnectingWindow().Show();
                    w.Close();
                };
            }
        }
    }
}
