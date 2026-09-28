using Pedroca2005BR.Utilities;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StepByStepChecklist : MonoBehaviour
{
    #region Singleton

    public static StepByStepChecklist Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    #endregion

    [Header("Data")]
    [SerializeField] RecipeSO checkListData;

    [Header("References")]
    [SerializeField] Transform layoutGroup;
    [SerializeField] GameObject stepPrefab;

    public void Setup(RecipeSO recipeData)
    {
        checkListData = recipeData;
        CreateSteps();
    }

    void CreateSteps()
    {
        foreach(var step in checkListData.howToMakeChecklist)
        {
            StepComponent s = Instantiate(stepPrefab, layoutGroup).GetComponent<StepComponent>();
            s.Setup(step);
        }
    }
}
