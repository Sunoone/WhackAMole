using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Score
{
    [System.Serializable]
    public struct ScoreEntry
    {
        public string Name;
        public int Score;
        public ScoreEntry(string name, int score)
        {
            Name = name;
            Score = score;
        }
    }
}
