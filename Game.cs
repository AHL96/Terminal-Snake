using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;

namespace Snake {
    class ScoreTime {
        public string name { get; set; }
        public int score { get; set; }
        public TimeSpan duration { get; set; }

        public static void Sort (List<ScoreTime> scores) {
            scores.OrderBy (s => s.score).ThenByDescending (s => s.duration);
        }

    }

    class Game {
        #region Variables
        public static readonly int HEIGHT = 25;
        public static readonly int WIDTH = 50;
        public readonly char[, ] board = new char[HEIGHT, WIDTH];
        readonly Snake snake = new Snake ();
        List<Apple> apples = new List<Apple> (3);
        private readonly DateTime start = DateTime.Now;
        private List<ScoreTime> top3Games = new List<ScoreTime> ();

        #endregion Variables

        public Game () {
            for (int i = 0; i < HEIGHT; i++) {
                for (int j = 0; j < WIDTH; j++) {
                    board[i, j] = UI.BOARDPIECE;
                }
            }

            for (int i = 0; i < apples.Capacity; i++) {
                apples.Add (new Apple ());
            }

        }

        public void run () {
            try {
                while (snake.lives > 0) {
                    events ();
                    update ();
                    display ();
                    Thread.Sleep (Settings.SPEED);
                }
            } catch {
                // Hit a wall
            }

            Console.ForegroundColor = UI.GAMEOVERCOLOR;
            Console.WriteLine ("GAMEOVER");
        }

        public void clearBoard () {
            for (int i = 0; i < HEIGHT; i++) {
                for (int j = 0; j < WIDTH; j++) {
                    board[i, j] = UI.BOARDPIECE;
                }
            }
            Console.Clear ();
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

                switch (i) {
                    case 0:
                        display.Append ($"|  Your score: {snake.size}\n");
                        break;
                    case 1:
                        display.Append ($"|  Your lives: {snake.lives}\n");
                        break;
                    case 2:
                        TimeSpan duration = DateTime.Now - start;
                        display.Append ($"|  Your time: {duration.Seconds}.{duration.Milliseconds}\n");
                        break;
                        // case 5:
                        //     display.Append ($"|  BEST GAME\n ");
                        //     break;
                        // case 6:
                        //     display.Append ($"|  score: {}\n ");
                        //     break;
                    default:
                        display.Append ("|\n");
                        break;
                }

            }
            for (int i = 0; i <= WIDTH + 1; i++) {
                display.Append ('=');
            }

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