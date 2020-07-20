using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WhackAMole;
using System;

namespace Score
{
    // @TODO: Redesign
    public class Leaderboard : MonoBehaviour
    {
        private const string _scoreListKey = "scoreList";
        private const string _nameKey = "name";
        private const string _defaultName = "...";
        [SerializeField]
        private ScoreList _scoreList;

        //@TODO: Remove these variables for a different solution.
        private int _editingScore;
        private int _editingIndex;

        [SerializeField][HideInInspector]
        private CanvasGroup _canvasGroup;
        [SerializeField]
        private ScoreEditableField[] _editableFields;
        public int Size { get => _editableFields.Length; }

        private void OnEnable()
        {
            Load();
        }

        public void Show(int startIndex)
        {
            //@TODO: Remove double load (Addscore > Show)
            gameObject.SetActive(true);
            int length = startIndex + _editableFields.Length;
            for (int i = startIndex; i < length; i++)
            {
                if (i > Size || _scoreList == null || i > _scoreList.Entries.Count)
                    continue; 
                
                _editableFields[i].SetScoreEntry(i, _scoreList.Entries[i]);
            }
        }

        public void AddScore(int startIndex, int newScore)
        {
            Load();
            gameObject.SetActive(true);
            int length = startIndex + Size;
            int index = FindIndex(newScore);
            if (index >= length || index < 0)
            {
                // Out of range
                Show(startIndex);
                return;
            }
          
            for (int i = startIndex; i < length; i++)
            {            
                if (i > Size ||  i > _scoreList.Entries.Count)
                    continue;

                _editableFields[i].gameObject.SetActive(true);
                if (i == index)
                {
                    var scoreEntry = new ScoreEntry(_defaultName, newScore);
                    // @TODO: Pass the whole score entry. All the data is required. Hacky circumvention right now.
                    _editableFields[i].SetNewEditableScoreEntry(i, scoreEntry, OnScoreChanged);
                    _editingScore = newScore;
                    _editingIndex = i;
                    continue;
                }
                  
                _editableFields[i].SetScoreEntry(i, _scoreList.Entries[startIndex + i]);              
            }
            Save();
        }
        
        // Find the appropriate index for the new entry.
        private int FindIndex(int score)
        {
            if (_scoreList != null && _scoreList.Entries.Count > 0)
            {
                int length = _scoreList.Entries.Count;
                for (int i = 0; i < length; i++)
                {
                    if (_scoreList.Entries[i].Score < score)
                        return i;
                }
            }
            return -1;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
                Save();
            if (Input.GetKeyDown(KeyCode.L))
                Load();
            if (Input.GetKeyDown(KeyCode.C))
                Clear();
        }

        private void OnScoreChanged(string name)
        {
            _scoreList.Entries[_editingIndex] = new ScoreEntry(name, _editingScore);
            Save();
        }

        public void Save()
        {
            string jsonString = JsonUtility.ToJson(_scoreList);
            Debug.Log(jsonString);
            PlayerPrefs.SetString(_scoreListKey, jsonString);
        }

        private void Load()
        {
            if (PlayerPrefs.HasKey(_scoreListKey))
            {
                string loadedString = PlayerPrefs.GetString(_scoreListKey);
                _scoreList = JsonUtility.FromJson<ScoreList>(loadedString);
            }
#if UNITY_EDITOR
            else
            {
                int length = _editableFields.Length;
                _scoreList = new ScoreList();
                for (int i = 0; i < length; i++)
                {
                    var scoreEntry = new ScoreEntry("...", 0);
                    _editableFields[i].SetScoreEntry(i, scoreEntry);
                    _scoreList.Entries.Add(scoreEntry);
                }
            }
#endif
        }

        public void Save(string scoreKey) => PlayerPrefs.SetString(scoreKey, JsonUtility.ToJson(this));

        public static bool TryLoad(string scoreKey, out ScoreList scoreList)
        {
            if (PlayerPrefs.HasKey(scoreKey))
            {
                string loadedString = PlayerPrefs.GetString(scoreKey);
                Debug.Log(loadedString);
                scoreList = JsonUtility.FromJson<ScoreList>(loadedString);
                return true;
            }
            scoreList = null;
            return false;
        }

        private void Reset()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _editableFields = GetComponentsInChildren<ScoreEditableField>();
            if (_editableFields == null || _editableFields.Length == 0)
            {
                Debug.LogError("No EditableFields found. Add children with components of type " + typeof(ScoreEditableField) + ". Deleting this component.");
                DestroyImmediate(this);
            }
        }
        
        [ContextMenu("Remove PlayerPrefs")]
        private void Clear()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("PlayerPrefsCleared.");
        }
    }
}
