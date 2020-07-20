using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTracker : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField]
    private Text TimerDisplay;
#pragma warning restore 649
    [SerializeField]
    private int _duration = 20;
    public int Duration { get => _duration; }
    [SerializeField]
    private float _intervalDuration = 1;
    private float _nextInterval;
    private bool _running = false;
    public float LeftoverTime { get; private set; }

    private Action _onGameStart;
    private Action<bool> _onGameOver;
    private Action<float> _onIntervalPassed;

    public void AddStartListener(Action startListener) { _onGameStart += startListener; }
    public void RemoveStartListener(Action startListener) { _onGameStart -= startListener; }
    public void AddTimerListener(Action<float> timerListener) { _onIntervalPassed += timerListener; }
    public void RemoveTimerListener(Action<float> timerListener) { _onIntervalPassed -= timerListener; }

    private void OnEnable() => SavetyChecks();

    public void StartTimer(Action<bool> onGameOver)
    {
        LeftoverTime = _duration;
        UpdateTimerDisplay();
        Pause(false);     
        _onGameOver = onGameOver;
        _onGameStart?.Invoke();
    }
    private void UpdateTimerDisplay() => TimerDisplay.text = ((int)LeftoverTime).ToString();

    public void Pause(bool pause)
    {
        _running = !pause;
        if (_running)
            _nextInterval = Time.time + _intervalDuration;
    }

    private void Update()
    {
        if (!_running)
            return;

        if (Time.time >= _nextInterval)
        {
            _nextInterval += _intervalDuration;
            LeftoverTime -= _intervalDuration;
            TimerDisplay.text = "Time: " + LeftoverTime.ToString();
            _onIntervalPassed?.Invoke(LeftoverTime);
            if (LeftoverTime <= 0)
            {
                _onGameOver?.Invoke(true);
                Pause(true);
            }
        }
    }

    private void SavetyChecks()
    {
        if (TimerDisplay == null)
        {
            Debug.LogError("TimerDisplay has not been assigned in the editor. Disabling " + GetType() + " on GameObject " + gameObject.name + ".");
            enabled = false;
        }
    }
}
