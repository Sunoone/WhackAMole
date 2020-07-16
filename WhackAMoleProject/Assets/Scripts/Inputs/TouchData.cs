using UnityEngine;
using Inputs.TouchHandlers;

namespace Inputs
{
    [System.Serializable]
    public class TouchData
    {
        public Collider Collider;
        public ITouchDragHandler DragHandler;
        public ITouchUpHandler UpHandler;
        public int FingerId;
        public int Order;
        public Vector3 StartPosition;
        public float StartTime;
        public bool IsDragged;

        // Static allocated Empty.
        public static TouchData Empty { get; private set; }
        // Creates the allocated Empty on initialization.
        static TouchData() { Empty = CreateEmpty(); }

        public TouchData(Collider collider, ITouchDragHandler dragHandler, ITouchUpHandler upHandler, Vector3 startPosition, float startTime, int fingerId)
        {
            Collider = collider;
            DragHandler = dragHandler;
            UpHandler = upHandler;
            StartPosition = startPosition;
            StartTime = startTime;
            FingerId = fingerId;
            Order = -1;
            IsDragged = false;
        }
        private static TouchData CreateEmpty() => new TouchData(null, null, null, Vector3.zero, -1, -1);
        public bool IsEmpty() => (Collider == null);
    }
}

