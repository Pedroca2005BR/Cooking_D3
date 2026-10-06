using Pedroca2005BR.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseObject : MonoBehaviour
{
    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    public void ClickEvent(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            EventManager.TriggerEvent(EventConstantNames.MOUSE_CLICK, transform);
        }
    }
}