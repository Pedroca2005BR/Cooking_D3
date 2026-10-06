using Pedroca2005BR.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

// Any object with this script will follow the mouse position and trigger an event when clicked. The event will pass the transform of the object as a parameter.
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