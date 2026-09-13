using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DraggableComponent : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [Header("Drag Settings")]
    [SerializeField] bool useDamping = true;
    [SerializeField] float damping = 0.05f;

    Vector3 velocity = Vector3.zero;
    Vector3 dragTargetPosition;
    bool isBeingDragged;



    void Update()
    {
        // Enquanto o objeto estiver sendo arrastado, atualiza a posição do objeto para seguir o mouse
        if (isBeingDragged)
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            dragTargetPosition = new Vector3(worldPoint.x, worldPoint.y, 0);

            if (useDamping)
            {
                // Suaviza a transição da posição do objeto para a posição do mouse
                transform.position = Vector3.SmoothDamp(
                transform.position,
                dragTargetPosition,
                ref velocity,
                damping
                );
            }
            else
            {
                // Move o objeto diretamente para a posição do mouse
                transform.position = dragTargetPosition;
            }
        }
    }


    // Função chamada quando o usuário pressiona o botão do mouse sobre o objeto
    public void OnPointerDown(PointerEventData eventData)
    {
        isBeingDragged = true;

        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        dragTargetPosition = transform.position;
    }

    // Função chamada quando o usuário solta o botão do mouse após clicar sobre o objeto
    public void OnPointerUp(PointerEventData eventData)
    {
        isBeingDragged = false;
    }
}
