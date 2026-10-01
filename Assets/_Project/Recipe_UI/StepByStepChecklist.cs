using Pedroca2005BR.Utilities;
using System;
using System.Collections.Generic;
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

    // Runtime variables
    List<StepComponent> steps;
    int currentStepIndex;

    public void Setup(RecipeSO recipeData)
    {
        checkListData = recipeData;
        steps = new();
        CreateSteps();
        currentStepIndex = 0;
    }

    void CreateSteps()
    {
        foreach(var step in checkListData.howToMakeChecklist)
        {
            StepComponent s = Instantiate(stepPrefab, layoutGroup).GetComponent<StepComponent>();
            s.Setup(step);
            steps.Add(s);
        }
    }

    private int GetStepIndexByIngredient(BaseIngredientData ingredient)
    {
        for(int i = 0; i < checkListData.howToMakeChecklist.Length; i++)
        {
            if(checkListData.howToMakeChecklist[i].ingredient == ingredient)
            {
                return i;
            }
        }
        return -1;
    }

    private void UpdateCurrentStep(int index)
    {
        // TODO: Add logic to update the visuals of the current step
    }


    private void OnEnable()
    {
        EventManager.Subscribe(EventConstantNames.INGREDIENT_CREATED, OnIngredientCreated);
        EventManager.Subscribe(EventConstantNames.INGREDIENT_CONSUMED, OnIngredientConsumed);
        EventManager.Subscribe(EventConstantNames.INGREDIENT_STOLEN, OnIngredientStolen);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventConstantNames.INGREDIENT_CREATED, OnIngredientCreated);
        EventManager.Unsubscribe(EventConstantNames.INGREDIENT_CONSUMED, OnIngredientConsumed);
        EventManager.Subscribe(EventConstantNames.INGREDIENT_STOLEN, OnIngredientStolen);

    }

    // Recursive function to uncheck previous steps if the ingredient is stolen
    private int TryRecursionUncheck(int index)
    {
        if (index < 0)
        {
            return -1;
        }

        bool unmarked = steps[index].TryToUnmarkStep();

        if (unmarked)
        {
            return TryRecursionUncheck(index - 1);
        }
        else
        {
            return index;
        }
    }

    
    private void OnIngredientStolen(object obj)
    {
        IngredientEventData newObj = obj as IngredientEventData;
        if (newObj == null)
        {
            Debug.LogError($"{EventConstantNames.INGREDIENT_CONSUMED} has different event data type");
            return;
        }

        int stepIndex = GetStepIndexByIngredient(newObj.ingredientData);

        if (stepIndex < currentStepIndex-1)
        {
            Debug.Log($"Ingredient {newObj.ingredientData.name} was stolen, but it was already used in a previous step. Current step index: {currentStepIndex}, Stolen step index: {stepIndex}");
            return;
        }

        if (stepIndex != -1)
        {
            steps[stepIndex].UpdateQuantity(-1);
            int stopIndex = TryRecursionUncheck(stepIndex);

            // stopIndex + 1 = last step that was unmarked succesfully, which means that currentStepIndex becomes the new current step index
            if (stopIndex+1 < currentStepIndex)
            {
                currentStepIndex = stopIndex + 1;
                UpdateCurrentStep(currentStepIndex);
            }
        }
    }

    private void OnIngredientConsumed(object obj)
    {
        IngredientEventData newObj = obj as IngredientEventData;
        if (newObj == null)
        {
            Debug.LogError($"{EventConstantNames.INGREDIENT_CONSUMED} has different event data type");
            return;
        }

        int stepIndex = GetStepIndexByIngredient(newObj.ingredientData);

        if (stepIndex != -1)
        {
            steps[stepIndex].UpdateQuantity(-1);
        }
    }

    private void OnIngredientCreated(object obj)
    {
        IngredientEventData newObj = obj as IngredientEventData;
        if (newObj == null)
        {
            Debug.LogError($"{EventConstantNames.INGREDIENT_CREATED} has different event data type");
            return;
        }

        int stepIndex = GetStepIndexByIngredient(newObj.ingredientData);

        if(stepIndex != -1)
        {
            steps[stepIndex].UpdateQuantity(1);

            if (steps[currentStepIndex].IsCompleted)
            {
                currentStepIndex++;
                UpdateCurrentStep(currentStepIndex);
            }
        }
    }
}
