using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Score
{
    // Class created for easy JsonUtility conversions and internal saving/loading features.
    [System.Serializable]
    public class ScoreList
    {
        [SerializeField]
        private List<ScoreEntry> _entries = new List<ScoreEntry>();
        public List<ScoreEntry> Entries { get => _entries; }

        // The entire list is being saved as a json. Could be solved with a database so every entry can be accessed and modified individually instead of saving the entire scorelist.
        // The reason to not include this feature is simply to keep the complexity (and budget) at a minimum
        
    }
}
