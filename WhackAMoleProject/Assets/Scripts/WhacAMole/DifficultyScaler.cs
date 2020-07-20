using System.Collections;
using System.Collections.Generic;
using Tweens;
using UnityEngine;

public class DifficultyScaler : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField]
    TimeTracker _gameTimer;
    [SerializeField]
    private TweenType _tweenType;
#pragma warning restore 649

    public int DifficultyScale { get; private set; }
    private Vector2Int _difficultyRange;
    
    [SerializeField]
    private List<Vector2Int> Difficulties = new List<Vector2Int>() { new Vector2Int(0, 50), new Vector2Int(10, 65), new Vector2Int(20, 80) };

    public void SetDifficulty(int difficulty)
    {
        _difficultyRange = Difficulties[(int)difficulty];
        DifficultyScale = _difficultyRange.x;
    }

    private void UpdateScaler(float timeLeft)
    {
        DifficultyScale = (int)TweenEase.GetNewValue(_tweenType, _gameTimer.Duration - timeLeft, _difficultyRange.x, _difficultyRange.y, _gameTimer.Duration);
    }

    private void OnEnable()
    {
        SavetyChecks();
        _gameTimer.AddTimerListener(UpdateScaler);
    }

    private void OnDisable()
    {
        _gameTimer.RemoveTimerListener(UpdateScaler);
    }

    private void SavetyChecks()
    {
        if (_gameTimer == null)
        {
            Debug.LogError("GameTimer has not been assigned in the editor. Disabling " + GetType() + " on GameObject " + gameObject.name + ".");
            enabled = false;
        }
    }
}
