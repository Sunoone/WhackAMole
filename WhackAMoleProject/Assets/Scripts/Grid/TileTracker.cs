using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [System.Serializable]
    public class TileTracker
    {
        public List<int> _unlockedList;
        private int _length;
        private int _width;
        private int _height;

        public TileTracker(int width, int height)
        {
            _unlockedList = new List<int>();
            _width = width;
            _height = height;

            _length = width * height;
            int length = width * height;
            for (int i = 0; i < length; i++)
                UnlockTile(i);
        }       

        public void LockTile(int x, int y)
        {
            int index = Convert2dTo1d(x, y);
            if (index < 0 || index > _length)
                throw new System.Exception("Out of grid space.");
            LockTile(index);
        }
        public void LockTile(int index) => _unlockedList.Remove(index);

        public void UnlockTile(int x, int y)
        {
            int index = Convert2dTo1d(x, y);
            if (index < 0 || index > _length)
                throw new System.Exception("Out of grid space.");
            UnlockTile(index);
        }
        public void UnlockTile(int index)
        {
            if (!_unlockedList.Contains(index))
                _unlockedList.Add(index);
        }            

        public int Convert2dTo1d(int x, int y) => (x + (y * _width));
        public Vector2Int Convert1dTo2d(int index)
        {
            int y = index / _width;
            int x = index % _width;
            return new Vector2Int(x, y);
        }

        public bool IsLocked(int x, int y) => _unlockedList.Contains(Convert2dTo1d(x, y));
        public bool IsLock(int index) => _unlockedList.Contains(index);
        public bool TryGetRandomUnlocked(out int index)
        {
            if (_unlockedList.Count <= 0)
            {
                index = 0;
                return false;
            }
            index = _unlockedList[Random.Range(0, _unlockedList.Count)];
            return true;
        }


    }
}