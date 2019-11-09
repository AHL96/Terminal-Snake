using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Xml;

namespace Snake {

    static class UI {
        public static readonly char BOARDPIECE = ' ';
        public static readonly char SNAKEPIECE = '#';
        public static readonly char APPLE = '@';
        public static readonly ConsoleColor GAMEOVERCOLOR = ConsoleColor.Red;

    }

    static class Settings {
        public static readonly int SPEED = 100;
    }

    static class Tools {

        public static readonly Dictionary<ConsoleKey, Vector2> dirMap = new Dictionary<ConsoleKey, Vector2> (new List<KeyValuePair<ConsoleKey, Vector2>> {
            new KeyValuePair<ConsoleKey, Vector2> (ConsoleKey.UpArrow, new Vector2 (0, -1)),
            new KeyValuePair<ConsoleKey, Vector2> (ConsoleKey.RightArrow, new Vector2 (1, 0)),
            new KeyValuePair<ConsoleKey, Vector2> (ConsoleKey.DownArrow, new Vector2 (0, 1)),
            new KeyValuePair<ConsoleKey, Vector2> (ConsoleKey.LeftArrow, new Vector2 (-1, 0)),
        });
        private static readonly Random rnd = new Random ();
        public static Vector2 RandomLocation () {
            return new Vector2 {
                X = rnd.Next () % Game.WIDTH,
                    Y = rnd.Next () % Game.HEIGHT
            };
        }

        public static void WriteToFile (string txt) {
            using (StreamWriter sw = new StreamWriter (@"./Debugging/output.txt", true)) {
                sw.WriteLine (txt);
            }
        }

        public static void WriteToFile (List<Vector2> snake) {
            using (StreamWriter sw = new StreamWriter (@"./Debugging/output.txt", true)) {
                sw.WriteLine (DateTime.Now);
                snake.ForEach (part => {
                    sw.Write (part.ToString ());
                });
                sw.WriteLine ();
            }
        }

        public static void saveTop3Games (List<object> games) {
            XmlDocument xml = new XmlDocument ();

        }

    }
}