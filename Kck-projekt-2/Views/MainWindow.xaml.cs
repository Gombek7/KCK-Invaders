using Kck_projekt_1.Models;
using Kck_projekt_1.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        readonly DispatcherTimer timer;
        ViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
            viewModel = ViewModel.Instance;

            this.DataContext = viewModel;

            timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Interval = TimeSpan.FromMilliseconds(1000/GameConfig.Fps);
            timer.Tick += timer_Tick;
            timer.Start();

            viewModel.ManualRefreshDataCommand.Execute(null);
        }
        void timer_Tick(object sender, EventArgs e)
        {
            viewModel.NextFrameCommand.Execute(null);
        }

        private void pauseButton_Checked(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void pauseButton_Unchecked(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.P)
                pauseButton.IsChecked = !pauseButton.IsChecked;
            else if (e.Key == Key.R)
            {
                restartButton.Command.Execute(null);
            }
            else if (e.Key == Key.Space)
            {
                if (viewModel.GameWon)
                    viewModel.NextRoundCommand.Execute(null);
                shootButton.Command.Execute(null);
            }
            else if (e.Key == Key.Left)
            {
                leftButton.Command.Execute(null);
            }
            else if (e.Key == Key.Right)
            {
                rightButton.Command.Execute(null);
            }
        }
    }
}
