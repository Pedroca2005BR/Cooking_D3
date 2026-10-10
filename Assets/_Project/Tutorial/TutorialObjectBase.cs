using UnityEngine;
using UnityEngine.Events;

// / <summary> All TutorialObjects should inherit from this class. It provides a reference to the TutorialController and methods to start and stop the tutorial step. </summary>
public abstract class TutorialObjectBase : MonoBehaviour
{
    TutorialController controller;

    public void SetController(TutorialController controller)
    {
        this.controller = controller;
        StopStep();
    }

    public virtual void StartStep()
    {
        gameObject.SetActive(true);
    }

    public virtual void StopStep()
    {
        gameObject.SetActive(false);
    }

    public virtual bool CanProceed()
    {
        return true;
    }

    protected bool TryProceed()
    {
        if (controller != null)
        {
            return controller.TryNextStep(this);
        }
        return false;
    }
}
