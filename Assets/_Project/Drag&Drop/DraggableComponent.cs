using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DraggableComponent : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    Vector3 velocity = Vector3.zero;
    Vector3 dragTargetPosition;
    bool isBeingDragged;



    void Update()
    {
        // Enquanto o objeto estiver sendo arrastado, atualiza a posi��o do objeto para seguir o mouse
        if (isBeingDragged)
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            dragTargetPosition = new Vector3(worldPoint.x, worldPoint.y, 0);

            // Suaviza a transi��o da posi��o do objeto para a posi��o do mouse
            transform.position = Vector3.SmoothDamp(
            transform.position,
            dragTargetPosition,
            ref velocity,
            0.04f
            );
        }
    }


    // Fun��o chamada quando o usu�rio pressiona o bot�o do mouse sobre o objeto
    public void OnPointerDown(PointerEventData eventData)
    {
        StartDragging();
    }

    // Fun��o chamada quando o usu�rio solta o bot�o do mouse ap�s clicar sobre o objeto
    public void OnPointerUp(PointerEventData eventData)
    {
        isBeingDragged = false;
    }

    public void StartDragging()
    {
        isBeingDragged = true;

        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(
            Mouse.current.position.ReadValue()
        );

        dragTargetPosition = transform.position;
        velocity = Vector3.zero;
    }

}
