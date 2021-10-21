using System;
using System.Collections.Generic;
using System.Text;

namespace Kck_projekty_1_2.Models
{
    class Game
    {
        public int Lifes { get; private set; }
        public bool MovingRight { get; set; }
        public bool MovingLeft { get; set; }
        private Player player = new Player(new Coords(0,0),3);
        public Game(int lifes)
        {
            Score = 0;
            Lifes = lifes;
        }

        public int Score { get; private set; }

        public event Action<int,int> PlayerMoved;
        public void NextFrame()
        {
            bool moving = MovingLeft || MovingRight;
            if (MovingRight)
            {
                player.MoveTo(player.Coords + new Coords(1,0));
                MovingRight = false;
            }
            if (MovingLeft)
            {
                player.MoveTo(player.Coords + new Coords(-1, 0));
                MovingLeft = false;
            }
            if (moving)
                PlayerMoved.Invoke(player.Coords.x, player.Coords.y);

        }

    }
}
