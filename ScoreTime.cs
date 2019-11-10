using System;
using System.Collections.Generic;
using System.Linq;

namespace Snake {

    public class ScoreTime {
        public string name { get; set; }
        public int score { get; set; }
        public TimeSpan duration { get; set; }

        public static List<ScoreTime> Sort (List<ScoreTime> games) {
            return games.OrderByDescending (s => s.score).ThenBy (s => s.duration).ToList ();
        }

        public override string ToString () {
            return $"name: {name}\nscore: {score}\ntime: {duration}\n";
        }

        public bool OnLeaderBoard (List<ScoreTime> leaderboard) {
            if (leaderboard.Count < Settings.MAXTOPGAMES) {
                return true;
            }
            foreach (var st in leaderboard) {
                if (this.Better (st)) {
                    return true;
                }
            }
            return false;
        }

        public bool Better (ScoreTime other) {
            if (score == other.score) {
                return duration < other.duration;
            }
            return score > other.score;
        }

    }
}