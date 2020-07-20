using System;
using UnityEngine;
using UnityEngine.UI;

namespace Score {
    [RequireComponent(typeof(InputField))]
    public class ScoreEditableField : ScoreField
    {
        [SerializeField][HideInInspector]
        private InputField _inputField;
        private Action<string> _onEndEdit;

        private void OnEnable() => _inputField.onEndEdit.AddListener(SubmitScore);
        private void OnDisable() => _inputField.onEndEdit.RemoveListener(SubmitScore);

        public override void SetScoreEntry(int index, ScoreEntry scoreEntry)
        {
            base.SetScoreEntry(index + 1, scoreEntry);
            SetInteractable(false);           
        }

        public void SetNewEditableScoreEntry(int index, ScoreEntry scoreEntry, Action<string> onEndEdit)
        {
            base.SetScoreEntry(index + 1, scoreEntry);
            _onEndEdit = onEndEdit;
            SetInteractable(true);
        }

        private void SetInteractable(bool interactable)
        {
            _inputField.readOnly = !interactable;
            _inputField.interactable = interactable;
            _inputField.placeholder.gameObject.SetActive(interactable);
        }

        private void SubmitScore(string name)
        {
            if (_scoreField.text == string.Empty || name == string.Empty || _onEndEdit == null)
            {
                Debug.LogError("Cannot set a score. No callback is assigned.");
                return;
            }
            _onEndEdit.Invoke(name);
        }

        private void Reset()
        {
            _inputField = GetComponent<InputField>();
            if (_inputField.targetGraphic == null || _inputField.textComponent == null || _inputField.placeholder == null)
            {
                Debug.LogError(typeof(InputField) + " has no TargetGraphic, TextComponent and/or Placeholder. Assign those fields first. Destroying this component.");
                DestroyImmediate(this);
            }
        }

#if UNITY_EDITOR
        // Debugging purposes
        [ContextMenu("Make Interactable")]
        private void MakeInteractable() => SetInteractable(true);
        [ContextMenu("Make Uninteractable")]
        private void MakeUninteractable() => SetInteractable(false);
#endif
    }
}