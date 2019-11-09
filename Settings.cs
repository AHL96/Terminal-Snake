using System;
using System.Collections.Generic;
using System.Numerics;

namespace Snake {

    static class Constansts {
        public static readonly char BOARDPIECE = ' ';
        public static readonly char SNAKEPIECE = '#';
        public static readonly char APPLE = '@';

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
        public static Random rnd = new Random ();
        public static Vector2 RandomLocation () {
            return new Vector2 {
                X = rnd.Next () % Game.WIDTH,
                    Y = rnd.Next () % Game.HEIGHT
            };
        }
    }
}