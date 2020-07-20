using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Tweens;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UIElements;
using WhackAMole;

public class MoleMovement : MonoBehaviour
{
    [SerializeField]
    private MoveTweenData[] _popupData;
    private int _moveDataIndex = 0;
    private IEnumerator _currentTween;
    private Action _onDone;
    public void PopupMovement(Vector3 position, Action onDone) => PopupMovement(_popupData, position, onDone);

    public void PopupMovement(MoveTweenData[] popupData, Vector3 position, Action onDone)
    {
        _moveDataIndex = -1;
        _popupData = popupData;
        _onDone = onDone;
        transform.position = position;

        PlayNextTween();
    }
    private void PlayNextTween()
    {
        Stop();
        _moveDataIndex++;
        if (_moveDataIndex >= _popupData.Length)
        {
            _onDone?.Invoke();
            return;
        }
        
        _currentTween = Tween(_popupData[_moveDataIndex], PlayNextTween);
        StartCoroutine(_currentTween);
    }

    private IEnumerator Tween(MoveTweenData data, Action onComplete)
    {
        float timer = 0;
        float duration = data.Duration;
        TweenType tweenType = data.TweenType;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + data.EndPosition;
        Vector3 difference = endPosition - startPosition;
        Vector3 newPosition = startPosition;

        while (timer < duration)
        {
            // @TODO: Limit it only to necessary axis for performance.
            // For development purposes, all axis are available.

            newPosition.x = TweenEase.GetNewValue(tweenType, timer, startPosition.x, difference.x, duration);
            newPosition.y = TweenEase.GetNewValue(tweenType, timer, startPosition.y, difference.y, duration);
            newPosition.z = TweenEase.GetNewValue(tweenType, timer, startPosition.z, difference.z, duration);
            transform.position = newPosition;
            timer = Mathf.Clamp(timer + Time.deltaTime, 0, duration);
            yield return new WaitForEndOfFrame();
        }
        transform.position = endPosition;
        onComplete?.Invoke();
    }

    public void Stop()
    {
        if (_currentTween != null)
            StopCoroutine(_currentTween);
    }

    public void Cancel()
    {
        Stop();
        _onDone = null;
    }

    private void SavetyCheck()
    {
        if (_popupData == null || _popupData.Length == 0)
        {
            Debug.LogError("PopupData has not been assigned in the editor. Disabling " + GetType() + " on GameObject " + gameObject.name + ".");
            enabled = false;
        }
    }
}
