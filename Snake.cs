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

        public void move () {
            Vector2 oldHead = body[0];
            body.Insert (0, oldHead + dir);
            body.RemoveAt (body.Count - 1);
            head = body[0];
        }

        public void collisionCheck () {
            for (int i = 1; i < body.Count; i++) {
                if (head == body[i]) {
                    lives--;
                }
            }
        }

        public void events (ConsoleKey key) {
            dir = Tools.dirMap[key];
        }

        public void update () {
            if (lives > 0) {
                move ();
                collisionCheck ();
            }
        }

        public void draw (Game game) {
            foreach (Vector2 bodyPart in body) {
                int x = (int) bodyPart.X;
                int y = (int) bodyPart.Y;
                game.board[y, x] = UI.SNAKEPIECE;
            }
        }

    }
}