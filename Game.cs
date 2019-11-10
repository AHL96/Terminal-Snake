using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Snake {

    class Game {
        #region Variables
        public static readonly int HEIGHT = UI.HEIGHT;
        public static readonly int WIDTH = UI.WIDTH;
        public readonly char[, ] board = new char[HEIGHT, WIDTH];
        public readonly Snake snake = new Snake ();
        List<Item> items = new List<Item> ();
        private readonly DateTime start = DateTime.Now;
        private TimeSpan time;
        private List<ScoreTime> topGames;

        private bool paused = false;

        #endregion Variables

        public Game () {
            for (int i = 0; i < HEIGHT; i++) {
                for (int j = 0; j < WIDTH; j++) {
                    board[i, j] = UI.BOARDPIECE;
                }
            }

            for (int i = 0; i < Settings.MAXAPPLES; i++) {
                items.Add (new Apple (this));
            }

            topGames = new List<ScoreTime> (Tools.getTopGames ());

        }

        public void run () {
            try {
                while (snake.lives > 0) {
                    events ();
                    update ();
                    display ();
                    Thread.Sleep (Settings.SPEED);
                }
            } catch (IndexOutOfRangeException) {
                Console.WriteLine ("You hit a wall!");
            }

            Console.ForegroundColor = UI.GAMEOVERCOLOR;
            Console.WriteLine ("GAMEOVER");

            ScoreTime theirGame = new ScoreTime {
                score = snake.size,
                duration = time
            };

            if (theirGame.OnLeaderBoard (topGames)) {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine ("CONGRATS!\nYou're made it on the leaderboard.\nWhat's your name?");
                theirGame.name = Console.ReadLine ();
                topGames.Add (theirGame);
                Tools.saveTop3Games (topGames);
            }
            Console.WriteLine ("Press a button to close the terminal");
            Console.ReadLine ();
        }

        public void clearBoard () {
            for (int i = 0; i < HEIGHT; i++) {
                for (int j = 0; j < WIDTH; j++) {
                    board[i, j] = UI.BOARDPIECE;
                }
            }
            Console.Clear ();
        }

        public void drawBoard () {
            int place = 0;

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
                if (place >= Settings.MAXTOPGAMES) {
                    place = 0;
                }
                switch (i) {
                    case 0:
                        display.Append ($"|  Your score: {snake.size}\n");
                        break;
                    case 1:
                        display.Append ($"|  Your lives: {snake.lives}\n");
                        break;
                    case 2:
                        display.Append ($"|  Your time: {time.Seconds}.{time.Milliseconds}\n");
                        break;
                    case 5:
                        display.Append ($"|  LEADERBOARD\n");
                        break;
                    case 6:
                    case 10:
                    case 14:
                        if (topGames.Count > 0 && place < topGames.Count) {
                            display.Append ($"|#{place+1})\tname: {topGames[place].name}\n");
                        } else {
                            goto default;
                        }
                        break;
                    case 7:
                    case 11:
                    case 15:
                        if (topGames.Count > 0 && place < topGames.Count) {
                            display.Append ($"|\tscore: {topGames[place].score}\n");
                        } else {
                            goto default;
                        }
                        break;
                    case 8:
                    case 12:
                    case 16:
                        if (topGames.Count > 0 && place < topGames.Count) {
                            display.Append ($"|\ttime: {topGames[place].duration}\n");
                            place++;
                        } else {
                            goto default;
                        }
                        break;
                    case 20:
                        if (paused) {
                            display.Append ("|\tPAUSED\n");
                        } else {
                            goto default;
                        }
                        break;
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

        public void display () {
            clearBoard ();
            snake.draw (this);
            items.ForEach (apple => apple.draw (this));
            drawBoard ();
        }

        public void update () {
            if (!paused) {
                time = DateTime.Now - start;
                snake.update ();
                items.ForEach (item => item.update ());
                items = snake.eat (items);
                items = items.Where (item => !item.timedOut ()).ToList ();
                if (items.Count < Settings.MAXITEMS) {
                    addItem ();
                }
            }
        }

        public void addItem () {
            double n = Tools.RandomDouble ();
            int appleCount = 0;
            items.ForEach (item => {
                try {
                    item = (Apple) item;
                    appleCount++;
                } catch {
                    // not an apple
                }
            });

            if (n < 0.01) {
                items.Add (new Life (this));
            } else if (appleCount < Settings.MAXAPPLES) {
                items.Add (new Apple (this));
            }
        }

        public void events () {
            if (Console.KeyAvailable) {
                ConsoleKey key = Console.ReadKey ().Key;
                switch (key) {
                    case ConsoleKey.P:
                        paused = !paused;
                        break;
                    default:
                        snake.events (key);
                        break;
                }
            }
        }

    }
}