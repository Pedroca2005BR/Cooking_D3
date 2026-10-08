using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateComponent : MonoBehaviour
{
    [SerializeField] RecipeDatabase recipeDatabase;
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
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IngredientComponent>(out var ingredient))
        {
            ingredientsOnPlate.Remove(ingredient);
        }
    }

    public void Deliver()
    {
        var recipe = recipeDatabase.GetCurrentRecipe();
        if (recipe == null)
        {
            Debug.LogWarning("No recipe found. Cannot deliver.");
            return;
        }

        // Remove ingredientes que foram destruídos enquanto estavam no prato
        ingredientsOnPlate.RemoveAll(i => i == null);

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
        float score;
        if (actual.Data == fuser.junkIngredient) score = 0;
        else score = ScoreCalculator.Calculate(actual, recipe.perfectDish);

        Debug.Log(ScoreCalculator.Dump(actual));

        // Invoke the event to notify listeners about the score
        OnScoreCalculated?.Invoke(score);
    }
}
