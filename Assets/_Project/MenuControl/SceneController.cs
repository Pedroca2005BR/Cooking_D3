using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] string mainGameSceneName = "MainGameScene";
    [SerializeField] string mainMenuName = "MainMenuScene";

    public void PlayGame()
    {
       SceneManager.LoadScene(mainGameSceneName);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuName);
    }
}
