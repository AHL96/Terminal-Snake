using System;
using System.Collections.Generic;
using System.Numerics;

namespace Snake {

    class Apple {
        public Vector2 pos { get; set; }

        public Apple () {
            pos = Tools.RandomLocation ();
        }

        public void draw (Game game) {
            int x = (int) pos.X;
            int y = (int) pos.Y;
            game.board[y, x] = UI.APPLE;
        }
    }
}