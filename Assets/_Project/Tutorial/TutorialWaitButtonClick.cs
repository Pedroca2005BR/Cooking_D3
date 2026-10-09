using UnityEngine;
using UnityEngine.UI;

public class TutorialWaitButtonClick : TutorialObjectBase
{
    public Button button;
    bool buttonClicked = false;

    public override void StartStep()
    {
        gameObject.SetActive(true);
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClicked);
        }
    }

    public override bool CanProceed()
    {
        return buttonClicked;
    }

    void OnButtonClicked()
    {
        buttonClicked = TryProceed();

        if (buttonClicked)
        {
            button.onClick.RemoveAllListeners();
            Destroy(this);
        }
    }
}
