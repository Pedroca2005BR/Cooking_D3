using System;
using UnityEngine;


public class InputTeste : MonoBehaviour
{
    public static event Action OnInteractInput;

    public void OnInteract()
    {  
        OnInteractInput?.Invoke();
    }
}
