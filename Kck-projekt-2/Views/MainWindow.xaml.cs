using Kck_projekt_1.Models;
using Kck_projekt_1.ViewModels;
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
using System.Windows.Threading;

namespace Kck_projekt_2.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            ViewModel viewModel = ViewModel.Instance;

            this.DataContext = viewModel;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000/GameConfig.Fps);
            timer.Tick += timer_Tick;
            timer.Start();

            viewModel.ManualRefreshDataCommand.Execute(null);
        }
        void timer_Tick(object sender, EventArgs e)
        {
            pauseButton.Command.Execute(null);
        }

        private void pauseButton_Checked(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void pauseButton_Unchecked(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }
    }
}
