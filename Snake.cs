using System;
using System.Collections.Generic;
using System.Numerics;

namespace Snake {

    class Snake {
        public Vector2 head { get; set; }
        public int size {
            get {
                return body.Count;
            }
        }
        private Vector2 dir { get; set; }

        private List<Vector2> body = new List<Vector2> ();

        public int lives;

        public Snake () {
            head = new Vector2 (5, 10);
            dir = new Vector2 (1, 0);
            body.Add (head);
            body.Add (new Vector2 {
                X = head.X + 1, Y = head.Y
            });
            body.Add (new Vector2 {
                X = head.X + 2, Y = head.Y
            });
            lives = 1;
        }

        public void eat (List<Apple> apples) {
            List<Apple> tempApples = new List<Apple> (apples);
            tempApples.ForEach (apple => {
                if (apple.pos == head) {
                    body.Add (apple.pos);
                    apples.Remove (apple);
                    apples.Add (new Apple ());
                }
            });
        }

        public void events () {
            if (Console.KeyAvailable) {
                dir = Tools.dirMap[Console.ReadKey ().Key];
            }
        }

        public void update () {
            Vector2 oldHead = body[0];
            body.Insert (0, oldHead + dir);
            body.RemoveAt (body.Count - 1);
            head = body[0];
        }

        public void draw (Game game) {
            foreach (Vector2 bodyPart in body) {
                int x = (int) bodyPart.X;
                int y = (int) bodyPart.Y;
                game.board[y, x] = Constansts.SNAKEPIECE;
            }
        }

    }
}