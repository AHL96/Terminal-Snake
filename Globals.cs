using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Snake {

    static class UI {
        public static readonly int WIDTH = 50;
        public static readonly int HEIGHT = 25;
        public static readonly char BOARDPIECE = ' ';
        public static readonly char SNAKEPIECE = '#';
        public static readonly char APPLE = '@';
        public static readonly ConsoleColor GAMEOVERCOLOR = ConsoleColor.Red;

    }

    static class Settings {
        public static readonly int SPEED = 100;
        public static readonly int MAXTOPGAMES = 3;
        public static readonly int MAXAPPLES = 3;
        public static readonly int MAXITEMS = MAXAPPLES + 1;
        public static readonly string topScoresFile = Path.Combine (Assembly.GetEntryAssembly ().Location, "../.top3games.xml");
    }

    static class Tools {

        public static readonly Dictionary<ConsoleKey, Vector2> dirMap = new Dictionary<ConsoleKey, Vector2> (new List<KeyValuePair<ConsoleKey, Vector2>> {
            new KeyValuePair<ConsoleKey, Vector2> (ConsoleKey.UpArrow, new Vector2 (0, -1)),
            new KeyValuePair<ConsoleKey, Vector2> (ConsoleKey.RightArrow, new Vector2 (1, 0)),
            new KeyValuePair<ConsoleKey, Vector2> (ConsoleKey.DownArrow, new Vector2 (0, 1)),
            new KeyValuePair<ConsoleKey, Vector2> (ConsoleKey.LeftArrow, new Vector2 (-1, 0)),
        });

        public static double RandomDouble () {
            Random rnd = new Random ();
            return rnd.NextDouble ();
        }

        public static Vector2 RandomItemLocation (Game game) {
            Random rnd = new Random ();
            Vector2 vect;
            do {
                vect = new Vector2 {
                    X = rnd.Next () % Game.WIDTH,
                    Y = rnd.Next () % Game.HEIGHT
                };
            } while (game.board[(int) vect.Y, (int) vect.X] != UI.BOARDPIECE);

            return vect;
        }

        public static void WriteToFile (string txt) {
            using (StreamWriter sw = new StreamWriter (@"./Debugging/output.txt", true)) {
                sw.WriteLine (txt);
            }
        }

        public static void WriteToFile (List<object> objs) {
            using (StreamWriter sw = new StreamWriter (@"./Debugging/output.txt", true)) {
                sw.WriteLine (DateTime.Now);
                objs.ForEach (obj => {
                    sw.Write (obj.ToString ());
                });
                sw.WriteLine ();
            }
        }

        public static void saveTop3Games (List<ScoreTime> games) {
            games = ScoreTime.Sort (games);
            if (games.Count > 3) {
                games.RemoveAt (3);
            }

            SerializeNow (games);
        }

        public static List<ScoreTime> getTopGames () {
            try {
                return ScoreTime.Sort (DeSerializeNow ());
            } catch (FileNotFoundException) {
                return new List<ScoreTime> ();
            }
        }

        private static void SerializeNow (List<ScoreTime> games) {
            XmlSerializer xs = new XmlSerializer (typeof (List<ScoreTime>));
            using (StreamWriter sw = new StreamWriter (Settings.topScoresFile)) {
                using (XmlWriter writer = XmlWriter.Create (sw)) {
                    xs.Serialize (writer, games);
                }
            }
        }

        private static List<ScoreTime> DeSerializeNow () {
            XmlSerializer xs = new XmlSerializer (typeof (List<ScoreTime>));
            using (StreamReader sr = new StreamReader (Settings.topScoresFile)) {
                sr.BaseStream.Position = 0;
                using (XmlReader reader = XmlReader.Create (sr)) {
                    return (List<ScoreTime>) xs.Deserialize (reader);
                }
            }
        }

    }
}