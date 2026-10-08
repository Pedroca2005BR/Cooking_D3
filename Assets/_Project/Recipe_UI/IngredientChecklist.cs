using Pedroca2005BR.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class IngredientChecklist : MonoBehaviour
{
    #region Singleton

    public static IngredientChecklist Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    #endregion

    [Header("Data")]
    [SerializeField] RecipeDatabase database;
    RecipeSO checkListData;

    [Header("References")]
    [SerializeField] Transform layoutGroup;
    [SerializeField] GameObject stepPrefab;

    // Runtime variables
    List<StepComponent> steps;

    void Start()
    {
        checkListData = database.GetCurrentRecipe();
        steps = new();
        CreateSteps();
    }

    void CreateSteps()
    {
        foreach (var step in checkListData.ingredientesNecessarios)
        {
            StepComponent s = Instantiate(stepPrefab, layoutGroup).GetComponent<StepComponent>();
            //Debug.Log($"{step.textToShow}");
            s.Setup(step);
            steps.Add(s);
        }
    }

    private int GetStepIndexByIngredient(BaseIngredientData ingredient)
    {
        for (int i = 0; i < checkListData.ingredientesNecessarios.Length; i++)
        {
            if (checkListData.ingredientesNecessarios[i].ingredient == ingredient)
            {
                return i;
            }
        }
        return -1;
    }

    private void OnEnable()
    {
        EventManager.Subscribe(EventConstantNames.INGREDIENT_CREATED, OnIngredientCreated);
        EventManager.Subscribe(EventConstantNames.INGREDIENT_CONSUMED, OnIngredientConsumed);
        EventManager.Subscribe(EventConstantNames.INGREDIENT_STOLEN, OnIngredientConsumed);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventConstantNames.INGREDIENT_CREATED, OnIngredientCreated);
        EventManager.Unsubscribe(EventConstantNames.INGREDIENT_CONSUMED, OnIngredientConsumed);
        EventManager.Unsubscribe(EventConstantNames.INGREDIENT_STOLEN, OnIngredientConsumed);

    }

    private void TryUncheck(int index)
    {
        steps[index].TryToUnmarkStep();
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
            TryUncheck(stepIndex);
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

        if (stepIndex != -1)
        {
            steps[stepIndex].UpdateQuantity(1);
        }
    }
}
