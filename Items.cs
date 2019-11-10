using System;
using System.Numerics;

namespace Snake {

    abstract class Item {
        public Vector2 pos { get; set; }
        protected abstract char img { get; }

        public Item (Game game) {
            pos = Tools.RandomItemLocation (game);
        }

        public void draw (Game game) {
            int x = (int) pos.X;
            int y = (int) pos.Y;
            game.board[y, x] = img;
        }

        public abstract void update ();
        public abstract bool timedOut ();
        public abstract bool eattenBy (Snake s);
    }

    class Apple : Item {
        protected override char img {
            get {
                return UI.APPLE;
            }
        }
        public Apple (Game game) : base (game) { }
        public override void update () { }
        public override bool timedOut () { return false; }

        public override bool eattenBy (Snake snake) {
            if (pos == snake.head) {
                snake.eat (this);
                return true;
            }
            return false;
        }
    }

    class Life : Item {
        private TimeSpan lifeSpan = new TimeSpan ((long) 5e9);
        private static char[] imgs = new char[] { '|', '/', '-', '\\' };
        protected override char img {
            get {
                return imgs[currImg];
            }
        }
        private int currImg = 0;

        public Life (Game game) : base (game) { }

        public override void update () {
            currImg = (currImg + 1) % imgs.Length;
        }

        public override bool timedOut () {
            return Tools.RandomDouble () < 0.01;
        }

        public override bool eattenBy (Snake snake) {
            if (pos == snake.head) {
                snake.lives++;
                return true;
            }
            return false;
        }
    }
}