using UnityEngine;
using UnityEngine.InputSystem;

public class CorteInputHandler : MonoBehaviour
{
    public void OnCorteInput(InputAction.CallbackContext context)
    {
        //dispara o evento quando o botao pressionado
        if (context.performed)
        {
            EventBusCorte.OnCortar?.Invoke();
        }
    }
}