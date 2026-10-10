using UnityEngine;

public class GC_BackgroundPlay : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.StopSound("Background");
        AudioManager.instance.PlaySound("Background");
    }
}
