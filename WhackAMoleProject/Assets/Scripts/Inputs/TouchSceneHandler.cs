using Inputs;
using Inputs.TouchHandlers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Inputs
{
    public class TouchSceneHandler : MonoBehaviour
    {
        private Action<TouchData> _onTouch;
        // Not touching an ITouchHandler
        private Action _onMiss;
        // @TODO: Solve bugs when simultaniously dragging 3 or more draggables. 2 is recommended until then.
        private const int _cMultiDragCap = 2;
        
        [SerializeField][Range(0, 10f)]
        private float _zDistance = 1f;
        [SerializeField][Range(0, 100f)]
        private float _dragThreshold = 1f;

#pragma warning disable 649
        [SerializeField]
        private LayerMask _ignoreMask;
        // Current version requires both the _touchUiHandler and the _camera fields to 
        [SerializeField][HideInInspector]
        private TouchUiHandler _touchUiHandler;
        [SerializeField][HideInInspector]
        private Camera _camera;
#pragma warning restore 649

        private List<TouchData> _touchDataList = new List<TouchData>();
        private int _previousTouchCount = 0;
        private int _previousSortTouchCount = 0;

        public void AddMissListener(Action missListener) => _onMiss += missListener;
        public void RemoveMissListener(Action missListener) => _onMiss -= missListener;


        private void OnEnable()
        {
            _touchDataList.Clear();
        }

        private void Update()
        {
            SolveNewTouchInput();
            SolveTouchInput();
        }

        public void AddListener(Action<TouchData> call) => _onTouch += call;
        public void RemoveListener(Action<TouchData> call) => _onTouch -= call;

        // A new touch input has been registered.
        private void SolveNewTouchInput()
        {
            if (_touchUiHandler != null && _touchUiHandler.Blocking)
                return;

            int touchCount = Input.touchCount;
            if (touchCount > _previousTouchCount && touchCount <= _cMultiDragCap)
            {
                var touch = Input.touches[GetTouchId()];
                if (TryGetTouchData(touch.fingerId, touch.position, _camera, ~_ignoreMask, out TouchData touchData))
                {
                    _touchDataList.Insert(touch.fingerId, touchData);
                    SolveOrder();
                    _onTouch?.Invoke(touchData);
                }
            }
            _previousTouchCount = Input.touchCount;
        }

        // Gives each TouchData a ascending number based on their StartTime. Provides info in which order they were created.
        private void SolveOrder()
        {
            if (_previousSortTouchCount == Input.touchCount)
                return;

            var clonedList = new List<TouchData>(_touchDataList);
            int length = clonedList.Count;
            for (int i = 0; i < length; i++)
            {
                var item = _touchDataList[i];
                var currentIndex = i;

                while (currentIndex > 0 && clonedList[currentIndex - 1].StartTime > item.StartTime)
                    currentIndex--;
                clonedList.Insert(currentIndex, item);
                item.Order = Mathf.Clamp(currentIndex, 0, Input.touchCount - 1);
            }
            _previousSortTouchCount = Input.touchCount;
        }

        // Work around for the Unity Touch registration: The touch fingerId remains the same till it is released. Therefore when dragging an object with fingerId 1, while releasing the 
        // fingerId 0, opens up the finderId 0 for a new touch. These helper functions help finding those empty values.
        private int GetTouchId()
        {
            int lowestValue = 0;
            int length = _touchDataList.Count;
            while (true)
            {
                if (!CheckFingerIdExists(lowestValue))
                    return lowestValue;
                lowestValue++;
            }
        }

        private bool CheckFingerIdExists(int fingerId)
        {
            int length = _touchDataList.Count;
            for (int i = 0; i < length; i++)
                if (_touchDataList[i].FingerId == fingerId)
                    return true;
            return false;
        }

        private void SolveTouchInput()
        {
            for (int i = 0; i < _touchDataList.Count; i++)
            {
                var touchCount = Input.touchCount;
                var touchData = _touchDataList[i];
                var touches = Input.touches;

                int touchIndex = touchData.FingerId;
                if (touchData.FingerId >= touchCount)
                {
                    // @TODO: Solution does not work for 2+ draggables: Releasing 2 while holding 3 creates unexpected behaviour.
                    SolveOrder();
                    touchIndex = Mathf.Clamp(touchData.Order, 0, touches.Length - 1); ;
                }
                Touch touch = touches[touchIndex];

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        return;
                    case TouchPhase.Stationary:
                        break;
                    case TouchPhase.Moved:
                        HandleDraggedTouch(touchData, touch.position);
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                    default:
                        RemoveReleasedTouches(touchData);
                        break;
                }
            }
        }

        // Handles the dragged touch by solving its threshold and calling the methods on ITouchDragHandler.
        private void HandleDraggedTouch(TouchData touchData, Vector3 position)
        {
            if (touchData.DragHandler == null)
                return;

            float zDistance = _camera.nearClipPlane + touchData.Collider.transform.localScale.z + _zDistance;
            if (!touchData.IsDragged)
            {
                if ((touchData.StartPosition - position).sqrMagnitude >= (_dragThreshold * _dragThreshold))
                {
                    touchData.IsDragged = true;
                    touchData.DragHandler.OnStartDrag(touchData.StartPosition);
                }
            }
            if (touchData.IsDragged)
            {
                if (TryRayCast(touchData, position, _camera, out RaycastHit hit))
                    touchData.DragHandler.OnDrag(hit.point);
                else
                {
                    position = new Vector3(position.x, position.y, zDistance);
                    touchData.DragHandler.OnDrag(_camera.ScreenToWorldPoint(position));
                }
            }
        }

        // Requires a list with TouchData that has been released.
        private void RemoveReleasedTouches(TouchData toRemove)
        {
            _touchDataList.Remove(toRemove);
            int length = _touchDataList.Count;
            for (int i = 0; i < length; i++)
                _touchDataList[i].Order = i;
        }

        // Casts a ray and returns the touchdata which the ray collided with.
        private bool TryGetTouchData(int fingerId, Vector3 position, Camera camera, LayerMask ignoreMask, out TouchData touchData)
        {
            if (TryRayCast(position, camera, ignoreMask, out RaycastHit hit))
            {
                var collider = hit.collider;
                // @TODO: Fix this unwanted null error in stresstests
                if (collider == null)
                {
                    touchData = TouchData.Empty;
                    return false;
                }

                // Fetch all the touch handlers.
                var downHandler = collider.GetComponent<ITouchDownHandler>();
                var dragHandler = collider.GetComponent<ITouchDragHandler>();
                var upHandler = collider.GetComponent<ITouchUpHandler>();
                // If any handler is available, it will result in returning true and providing the TouchData through the "out".
                if (downHandler != null || dragHandler != null || upHandler != null)
                {
                    // @TODO: Remove double null check for performance.
                    downHandler?.OnDown(fingerId);
                    touchData = new TouchData(collider, dragHandler, upHandler, position, Time.realtimeSinceStartup, fingerId);
                    return true;
                }
                else
                    _onMiss?.Invoke();
            }
            touchData = TouchData.Empty;
            return false;
        }

        // Raycasts and returns the hit
        private bool TryRayCast(Vector3 position, Camera camera, LayerMask ignoreMask, out RaycastHit hit)
        {
            var ray = camera.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out hit, float.MaxValue, ignoreMask))
            {
                // Makes sure only unique entries are added.
                // @TODO: Probably should ignore other draggables while dragging
                int length = _touchDataList.Count;
                for (int i = 0; i < length; i++)
                    if (hit.collider == _touchDataList[i].Collider)
                        return false;
            }
            return true;
        }

        // Ignores own collider when raycasting
        private bool TryRayCast(TouchData touchData, Vector3 position, Camera camera, out RaycastHit hit)
        {
            var ray = camera.ScreenPointToRay(position);
            var hits = Physics.RaycastAll(ray);
            int length = hits.Length;
            for (int i = 0; i < length; i++)
            {
                if (hits[i].collider == touchData.Collider)
                    continue;

                hit = hits[i];
                return true;
            }
            hit = new RaycastHit();
            return false;
        }

        private void Reset()
        {
            _camera = GetComponent<Camera>();
            if (_camera == null)
            {
                Debug.LogError(GetType() + " requires a " + typeof(Camera) + " component on the same GameObject. Destroying this.");
                DestroyImmediate(this);
            }
            _touchUiHandler = FindObjectOfType<TouchUiHandler>();
            if (_touchUiHandler == null)
            {
                Debug.LogError(GetType() + " requires a " + typeof(TouchUiHandler) + " component within the scene. Destroying this.");
                DestroyImmediate(this);
            }
        }
    }
}
