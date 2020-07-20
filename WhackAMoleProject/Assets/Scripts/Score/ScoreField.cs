using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Score
{
    [SerializeField]

    public class ScoreField : MonoBehaviour
    {
        private const int textFieldCount = 2;
#pragma warning disable 649
        [SerializeField]
        protected Text _indexField;
        [SerializeField]
        protected Text _nameField;
        [SerializeField]
        protected Text _scoreField;
#pragma warning restore 649

        private void OnEnable()
        {
            SavetyChecks();
        }

        public virtual void SetScoreEntry(int index, ScoreEntry scoreEntry)
        {
            _indexField.text = index.ToString();
            _nameField.text = scoreEntry.Name;
            _scoreField.text = scoreEntry.Score.ToString();
        }

        private void SavetyChecks()
        {
            if (_indexField == null)
            {
                Debug.LogError("IndexField has not been assigned in the editor. Disabling " + GetType() + " on GameObject " + gameObject.name + ".");
                enabled = false;
            }
            if (_nameField == null)
            {
                Debug.LogError("NameField has not been assigned in the editor. Disabling " + GetType() + " on GameObject " + gameObject.name + ".");
                enabled = false;
            }
            if (_scoreField == null)
            {
                Debug.LogError("Scorefield has not been assigned in the editor. Disabling " + GetType() + " on GameObject " + gameObject.name + ".");
                enabled = false;
            }
        }

    }
}
