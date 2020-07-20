using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Tweens;
using UnityEngine;
using UnityEngine.UIElements;
namespace WhackAMole
{
    [RequireComponent(typeof(MoleMovement))]
    public class Mole : Poolable, ITouchDownHandler
    {
        [SerializeField]
        private TweenType _tweenType = TweenType.Linear;
        [SerializeField]
        private float _value = 100;
        private float _currentValue;

        [SerializeField]
        private float _duration = 2f;
        private float _timer;

        [SerializeField][HideInInspector]
        private MoleMovement _moleMovement;
        private Action _onHit;
        private Action<float> _scoreCallback;

        public void OnDown(int touchIndex) => SolveHit();

        public void SolveHit()
        {
            _moleMovement.Cancel();
            _onHit?.Invoke();
            _scoreCallback?.Invoke(_currentValue);
            Pool();
        }

        public void Popup(Vector3 position, Action onHit, Action onMiss, Action<float> scoreCallback)
        {
            _currentValue = _value;         
            _moleMovement.PopupMovement(position, onMiss);

            _onHit = onHit;
            onMiss += Pool;
            _scoreCallback = scoreCallback;
        }

        private void Update()
        {
            if (_timer < _duration)
                _currentValue = TweenEase.GetNewValue(_tweenType, _timer, _value, _value, _duration);
            _timer = Mathf.Clamp(_timer + Time.deltaTime, 0, _duration);
        }

        private void Reset()
        {
            _moleMovement = GetComponent<MoleMovement>();
        }

#if UNITY_EDITOR
        [ContextMenu("Popup Test")]
        private void Popup() => Popup(transform.position, null, null, null);
#endif
    }
}
