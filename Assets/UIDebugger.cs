using UnityEngine;
using UnityEngine.EventSystems;

public class UIDebugger : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            Debug.Log("<color=yellow>L'objet qui a reçu le clic est : </color><b>" +
                      eventData.pointerCurrentRaycast.gameObject.name + "</b>");
        }
    }
}