using Kck_projekty_1_2.Models;
using Kck_projekty_1_2.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Kck_projekty_1_2.ViewModels
{
    class ConsoleViewModel
    {
        public Game game = new Game(3);

        public ICommand MoveRightCommand { get; }
        public ICommand MoveLeftCommand { get; }
        public ICommand ShootCommand { get; }

        public ConsoleViewModel()
        {
            MoveRightCommand =  new RelayCommand(MoveRight);
            MoveLeftCommand =  new RelayCommand(MoveLeft);
            ShootCommand =  new RelayCommand(Shoot);
        }

        void MoveRight()
        {
            game.MovingRight = true;
            game.NextFrame();
        }
        void MoveLeft()
        {
            game.MovingLeft = true;
            game.NextFrame();
        }
        void Shoot()
        {

        }
    }
}
