using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Snake {

    public class ScoreTime {
        public string name { get; set; }
        public int score { get; set; }
        public TimeSpan duration { get; set; }

        public static List<ScoreTime> Sort (List<ScoreTime> games) {
            // scores.GroupBy (s => s.score).OrderByDescending (group => group.OrderBy (s => s.duration));
            return games.OrderByDescending (s => s.score).ThenBy (s => s.duration).ToList ();
        }

        public override string ToString () {
            return $"name: {name}\nscore: {score}\ntime: {duration}\n";
        }

        public bool OnLeaderBoard (List<ScoreTime> leaderboard) {
            foreach (var st in leaderboard) {
                if (this.Better (st)) {
                    return true;
                }
            }
            return false;
        }

        public bool Better (ScoreTime other) {
            if (score == other.score) {
                return duration > other.duration;
            }
            return score > other.score;
        }

    }

    class Game {
        #region Variables
        public static readonly int HEIGHT = 25;
        public static readonly int WIDTH = 50;
        public readonly char[, ] board = new char[HEIGHT, WIDTH];
        public readonly Snake snake = new Snake ();
        List<Apple> apples = new List<Apple> (3);
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

            for (int i = 0; i < apples.Capacity; i++) {
                apples.Add (new Apple ());
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
            int place = 0;
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
                        display.Append ($"|  BEST GAMES\n");
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
            if (!paused) {
                time = DateTime.Now - start;
                snake.update ();
                snake.eat (apples);
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