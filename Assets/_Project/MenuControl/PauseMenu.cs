using UnityEngine;

public class PauseMenu : OptionsController
{
    public override void ToggleOptionMenu()
    {
        if (active) Time.timeScale = 1f;
        else Time.timeScale = 0f;



        base.ToggleOptionMenu();
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
}
