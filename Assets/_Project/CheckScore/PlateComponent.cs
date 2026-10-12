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

        // Calculate score based on the recipe and ingredients on the plate
        float score;
        if (fuser.TryFusing(ingredientsOnPlate.ToArray(), out var combined))
        {
            var combinedComp = combined.GetComponent<IngredientComponent>();

            if (combinedComp.data == fuser.junkIngredient) score = 0;
            else score = ScoreCalculator.Calculate(combinedComp.ingredientInstance, recipe.perfectDish);

            Debug.Log(ScoreCalculator.Dump(combinedComp.ingredientInstance));
        }
        else
        {
            IngredientRuntimeInstance[] instances = new IngredientRuntimeInstance[ingredientsOnPlate.Count]; 
            for(int i = 0; i < instances.Length; i++)
            {
                instances[i] = ingredientsOnPlate[i].ingredientInstance;
            }

            var falseDish = new IngredientRuntimeInstance();
            falseDish.Data = null;
            falseDish.SetBaseComponents(instances);

            score = ScoreCalculator.Calculate(falseDish, recipe.perfectDish);
            Debug.Log(ScoreCalculator.Dump(falseDish));
        }


        

        // Invoke the event to notify listeners about the score
        OnScoreCalculated?.Invoke(score);
    }
}
