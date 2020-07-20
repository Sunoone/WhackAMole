using Grid;
using Score;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WhackAMole;

public class GameManager : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField]
    private Board _board;
    [SerializeField]
    private TimeTracker _gameTimer;
    [SerializeField]
    private MoleSpawner _moleSpawner;
    [SerializeField]
    private ScoreTracker _scoreTracker;
    [SerializeField]
    private Leaderboard _leaderboard;

    private PlayerData _playerData;

#pragma warning restore 649

    private TileTracker _tileTracker;

    public void StartGame()
    {
        _tileTracker = new TileTracker(_board.Size.x, _board.Size.y);
        _scoreTracker.Refresh();
        _gameTimer.StartTimer(GameOver);
        _moleSpawner.StartSpawner(null, null, _scoreTracker.AddScore);
    }

    // @TODO: Add losing conditions.
    public void GameOver(bool victory)
    {
        Debug.Log("Done.");
        _moleSpawner.StopSpawner();
        _leaderboard.AddScore(0, (int)_scoreTracker.Score);
    }

    

    private void OnEnable()
    {
        SavetyChecks();
    }

    private void Reset()
    {
        _board = GetComponent<Board>();
        _gameTimer = GetComponent<TimeTracker>();
    }

    private void SavetyChecks()
    {
        if (_board == null)
        {
            Debug.LogError("Board has not been assigned in the editor. Disabling " + GetType() + " on GameObject " + gameObject.name + ".");
            enabled = false;
        }
        if (_gameTimer == null)
        {
            Debug.LogError("GameTimer has not been assigned in the editor. Disabling " + GetType() + " on GameObject " + gameObject.name + ".");
            enabled = false;
        }
        if (_moleSpawner == null)
        {
            Debug.LogError("MoleSpawner has not been assigned in the editor. Disabling " + GetType() + " on GameObject " + gameObject.name + ".");
            enabled = false;
        }
        if (_scoreTracker == null)
        {
            Debug.LogError("ScoreCounter has not been assigned in the editor. Disabling " + GetType() + " on GameObject " + gameObject.name + ".");
            enabled = false;
        }
        if (_leaderboard == null)
        {
            Debug.LogError("Leaderboard has not been assigned in the editor. Disabling " + GetType() + " on GameObject " + gameObject.name + ".");
            enabled = false;
        }
    }

}
