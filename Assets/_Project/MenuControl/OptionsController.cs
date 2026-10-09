using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsController : MonoBehaviour
{
    [SerializeField] GameObject optionsMenu;
    protected bool active = false;

    public void ExitGame()
    {
        Debug.Log("Quiting game...");
        Application.Quit();
    }

    public virtual void ToggleOptionMenu()
    {
        active = optionsMenu.activeInHierarchy;
        if (active) ExitOptionsMenu();
        else OpenOptionsMenu();
        active = optionsMenu.activeInHierarchy;
    }

    void OpenOptionsMenu()
    {
        optionsMenu.SetActive(true);
        
    }

    void ExitOptionsMenu()
    {
        optionsMenu.SetActive(false);
    }
}
