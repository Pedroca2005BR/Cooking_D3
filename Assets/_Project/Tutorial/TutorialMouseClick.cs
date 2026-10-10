using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialMouseClick : TutorialObjectBase
{
    bool wasPressed = false;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !wasPressed)
        {
            wasPressed = true;
            TryProceed();
        }
    }
}
