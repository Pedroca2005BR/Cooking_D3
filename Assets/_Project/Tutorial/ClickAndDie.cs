using UnityEngine;
using UnityEngine.EventSystems;

public class ClickAndDie : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Destroy(gameObject);
    }
}
