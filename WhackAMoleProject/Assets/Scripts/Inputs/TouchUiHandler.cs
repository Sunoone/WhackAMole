using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Only uses the GraphicsRaycaster to communicate Ui elements blocking scene elements to the TouchSceneHandler. Using Unity default Ui functionality for Ui interaction.
public class TouchUiHandler : MonoBehaviour
{
    [SerializeField][HideInInspector]
    private GraphicRaycaster _graphicRaycaster;
    private bool _blocking = false;
    public bool Blocking { get => _blocking; }

    private void Update()
    {
        var pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        _graphicRaycaster.Raycast(pointerEventData, results);
        _blocking = results.Count > 0;
    }

    private void Reset()
    {
        // These checks are chosen over the [RequireComponent(typeof(T))] class attribute, as you do not want to create more Canvases and GraphicsRaycasters automatically.
        if (GetComponent<Canvas>() == null)
        {
            // Doesn't actually use the canvas. It wants to have it for a clean hierarchy.
            Debug.LogError(GetType() + " requires a " + typeof(Canvas) + " component on the same GameObject. Destroying this.");
            DestroyImmediate(this);
        }
        _graphicRaycaster = GetComponent<GraphicRaycaster>();
        if (_graphicRaycaster == null)
        {
            Debug.LogError(GetType() + " requires a " + typeof(GraphicRaycaster) + " component on the same GameObject. Destroying this.");
            DestroyImmediate(this);
        }
    }
}
