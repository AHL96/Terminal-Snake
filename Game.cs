using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;

namespace Snake {

    class Game {
        public static readonly int HEIGHT = 25;
        public static readonly int WIDTH = 50;
        public readonly char[, ] board = new char[HEIGHT, WIDTH];
        readonly Snake snake = new Snake ();
        List<Apple> apples = new List<Apple> (3);

        public Game () {
            for (int i = 0; i < HEIGHT; i++) {
                for (int j = 0; j < WIDTH; j++) {
                    board[i, j] = Constansts.BOARDPIECE;
                }
            }

            for (int i = 0; i < apples.Capacity; i++) {
                apples.Add (new Apple ());
            }

        }

        public void run () {
            while (snake.lives > 0) {
                events ();
                update ();
                display ();
                Thread.Sleep (Settings.SPEED);
            }
        }

        public void clearBoard () {
            for (int i = 0; i < HEIGHT; i++) {
                for (int j = 0; j < WIDTH; j++) {
                    board[i, j] = Constansts.BOARDPIECE;
                }
            }
        }

        public void display () {
            clearBoard ();
            snake.draw (this);
            apples.ForEach (apple => apple.draw (this));

            StringBuilder display = new StringBuilder ();
            for (int i = 0; i <= WIDTH + 1; i++) {
                display.Append ('=');
            }
            display.Append ('\n');
            for (int i = 0; i < HEIGHT; i++) {
                display.Append ('|');
                for (int j = 0; j < WIDTH; j++) {
                    display.Append (board[i, j]);
                }
                if (i == 0) {
                    display.Append ($"|  Your score: {snake.size}\n");
                } else {
                    display.Append ("|\n");
                }
            }
            for (int i = 0; i <= WIDTH + 1; i++) {
                display.Append ('=');
            }
            Console.Clear ();
            Console.WriteLine (display);
        }

        public void update () {
            snake.update ();
            snake.eat (apples);
        }

        public void events () {
            snake.events ();
        }

    }
}