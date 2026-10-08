using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialController : MonoBehaviour
{
    [Header("Rules Reference")]
    [SerializeField] RecipeDatabase rules;

    [Header("Tutorial Steps")]
    [SerializeField] TutorialObjectBase[] tutorialSteps;
    private int currentStepIndex = 0;

    private void Start()
    {
        foreach (var step in tutorialSteps)
        {
            step.SetController(this);
        }

        currentStepIndex = 0;
        
        NextStep();
        
    }

    public void NextStepByUI(InputAction.CallbackContext context)
    {
        if (context.performed && tutorialSteps[currentStepIndex - 1].CanProceed()) 
            NextStep();
    }

    public void NextStep()
    {

        if (currentStepIndex > 0)
        {
            // Stop the previous step if it exists
            tutorialSteps[currentStepIndex - 1].StopStep();
        }
        if (currentStepIndex < tutorialSteps.Length)
        {
            tutorialSteps[currentStepIndex].StartStep();
            currentStepIndex++;
        }
    }

    public bool TryNextStep(TutorialObjectBase step)
    {
        if (currentStepIndex != 0 && step == tutorialSteps[currentStepIndex - 1])
        {
            NextStep();
            return true;
        }
        return false;
    }

    public void EndTutorial()
    {
        PlayerPrefs.SetInt("HasPlayedBefore", 1);
    }
}
