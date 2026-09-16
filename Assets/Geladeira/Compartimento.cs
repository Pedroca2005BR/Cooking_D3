using UnityEngine;
using UnityEngine.EventSystems;

public class Compartimento : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private GameObject ingredientePrefab;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clicou no compartimento");

        if (ingredientePrefab == null)
        {
            Debug.LogWarning("Ingrediente prefab não atribuído no compartimento.");
            return;
        }
        GameObject ingrediente = Instantiate(
                ingredientePrefab,
                transform.position,
                Quaternion.identity
            );
        DraggableComponent draggable =
        ingrediente.GetComponent<DraggableComponent>();

        draggable.StartDragging();
    }

}
