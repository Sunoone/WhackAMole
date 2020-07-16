using UnityEngine;

namespace Inputs.TouchHandlers
{
    public interface ITouchDragHandler
    {
        void OnStartDrag(Vector3 position);
        void OnDrag(Vector3 position);
        void OnEndDrag();
        // For the end drag, use the ITouchUpHandler.
    }
}