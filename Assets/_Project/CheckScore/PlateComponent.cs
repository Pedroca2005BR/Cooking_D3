using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateComponent : MonoBehaviour
{
    [SerializeField] RulesObject rulesObject;
    public event Action<float> OnScoreCalculated;


    // Runtime variables
    List<IngredientComponent> ingredientsOnPlate = new List<IngredientComponent>();
    IngredientFuserComponent fuser;

    private void Awake()
    {
        fuser = GetComponent<IngredientFuserComponent>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IngredientComponent>(out var ingredient))
        {
            ingredientsOnPlate.Add(ingredient);

            if (fuser.TryFusing(ingredientsOnPlate.ToArray(), out var fused))
            {
                ingredientsOnPlate.Clear();
                ingredientsOnPlate.Add(fused.GetComponent<IngredientComponent>());
            }
        }
    }

    public void Deliver()
    {
        var recipe = rulesObject.GetCurrentRecipe();
        if (recipe == null)
        {
            Debug.LogWarning("No recipe found. Cannot deliver.");
            return;
        }

        List<IngredientRuntimeInstance> ingredientInstances = new List<IngredientRuntimeInstance>();
        foreach (var ingredient in ingredientsOnPlate)
        {
            ingredientInstances.Add(ingredient.ingredientInstance);
        }

        var actualInstance = new IngredientRuntimeInstance();
        actualInstance.Data = recipe.perfectDish.ingredient; 
        actualInstance.SetBaseComponents(ingredientInstances.ToArray());

        var actual = ingredientsOnPlate.Count == 1
            ? ingredientsOnPlate[0].ingredientInstance   // sem o embrulho
            : actualInstance;

        // Calculate score based on the recipe and ingredients on the plate
        float score = ScoreCalculator.Calculate(actual, recipe.perfectDish);

        Debug.Log($"Score for the delivered dish: {score}");

        // Invoke the event to notify listeners about the score
        OnScoreCalculated?.Invoke(score);
    }
}
