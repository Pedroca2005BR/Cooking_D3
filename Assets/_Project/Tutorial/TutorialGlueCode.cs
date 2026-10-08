using UnityEngine;

public class TutorialGlueCode : MonoBehaviour
{
    [SerializeField] bool firstTimePlayingGame = true;
    [SerializeField] TutorialController tutorialController;
    [SerializeField] ConfirmationComponent confirmationComponent;
    [SerializeField] SceneController sceneController;

    private void Start()
    {
        // Can be uncommented to control background music
        //AudioManager.instance.StopSound("BackMusic");
        //AudioManager.instance.PlaySound("BackMusic");

        if (PlayerPrefs.GetInt("HasPlayedBefore", 0) == 0)
        {
            firstTimePlayingGame = true;
        }
        else
        {
            firstTimePlayingGame = false;
        }
    }

    public void TryInitiateGame()
    {
        if (firstTimePlayingGame)
        {
            firstTimePlayingGame = false;
        }
        else
        {
            tutorialController.EndTutorial();
            sceneController.PlayGame();
        }
    }
}
