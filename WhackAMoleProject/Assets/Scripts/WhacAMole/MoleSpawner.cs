using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid;
using GarbageCollection;
using System.Threading;
using System;

namespace WhackAMole
{
    public class MoleSpawner : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField]
        private Pool _pool;
        [SerializeField]
        private Board _board;
        [SerializeField]
        private DifficultyScaler _difficultyScaler;
        [SerializeField]
        private TileTracker _tileTracker;
        [SerializeField]
        private Vector3 _spawnOffset;
#pragma warning restore 649

        private Vector2 _spawnTimeRange = new Vector2(3f, 0.1f);
        private float _range;
        
        private float _timer;
        private int _intervalDuration;
        private float _nextInterval;
        private bool _running = false;

        private Action _onHit;
        private Action _onMiss;
        private Action<float> _scoreCallback;

        private void OnEnable()
        {
            SavetyChecks();
            _range = _spawnTimeRange.x - _spawnTimeRange.y;
        }

        public void StartSpawner(Action onHit, Action onMiss, Action<float> scoreCallback)
        {
            _tileTracker = new TileTracker(_board.Size.x, _board.Size.y);
            _running = true;
            // @TODO: less dynamic calculations
            UpdateSpawnRate();
            _timer = Time.time;
            _onHit = onHit;
            _onMiss = onMiss;
            _scoreCallback = scoreCallback;
        }

        public void StopSpawner()
        {
            _running = false;
        }

        private void SpawnMole()
        {
            // Check if there are unlocked tiles available or if it can lock the tile it received.
            if (!_tileTracker.TryGetRandomUnlocked(out int index))
                return;

            _tileTracker.LockTile(index);
            var mole = _pool.GetItem() as Mole;
            var coordinates = _tileTracker.Convert1dTo2d(index);
            Action onHit = _onHit;
            onHit += () => _tileTracker.UnlockTile(index);
            Action onMiss = _onMiss;
            onMiss = () => _tileTracker.UnlockTile(index);
            mole.Popup(_board.Grid[coordinates.x, coordinates.y] + _spawnOffset, onHit, onMiss, _scoreCallback);
        }

        private void Update()
        {
            if (!_running)
                return;

           if ( _timer > _nextInterval)
            {
                UpdateSpawnRate();
                SpawnMole();              
            }
            _timer += Time.deltaTime;
        }

        private void UpdateSpawnRate()
        {
            float percentage = (100f - _difficultyScaler.DifficultyScale) / 100f;
            _nextInterval = Time.time + Mathf.Clamp(percentage * _range, 0.05f, float.MaxValue);
        }

        private void SavetyChecks()
        {
            if (_pool == null)
            {
                Debug.LogError("Pool has not been assigned in the editor. Disabling " + GetType() + " on GameObject " + gameObject.name + ".");
                enabled = false;
            }
            if (_board == null)
            {
                Debug.LogError("Board tabs are visible than assigned in the editor. Disabling " + GetType() + " on GameObject " + gameObject.name + ".");
                enabled = false;
            }
            if (_difficultyScaler == null)
            {
                Debug.LogError("DifficultyScaler tabs are visible than assigned in the editor. Disabling " + GetType() + " on GameObject " + gameObject.name + ".");
                enabled = false;
            }
        }

    }
}