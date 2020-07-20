using Inputs.TouchHandlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour, ITouchDragHandler, ITouchDownHandler
{
    private Vector3 _offset;
    public void OnStartDrag(Vector3 position)
    {

    }
    public void OnDrag(Vector3 position)
    {
        transform.position = position;
    }

    public void OnEndDrag()
    {
    }

    public void OnDown(int touchIndex)
    {

    }
}
