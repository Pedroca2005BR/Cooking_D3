using UnityEngine;

// <summary>
// The recipe database will contain all the recipes in the game. An script may call it to get the recipe data.
[CreateAssetMenu(fileName = "RecipeDatabase", menuName = "Scriptable Objects/RecipeDatabase")]
public class RecipeDatabase : ScriptableObject
{
    public RecipeSO[] recipes;
    public int currentRecipeIndex = 0;

    public RecipeSO GetCurrentRecipe()
    {
        if (recipes.Length == 0)
        {
            Debug.LogWarning("Recipe database is empty or not assigned.");
            return null;
        }
        else if (currentRecipeIndex < 0 || currentRecipeIndex >= recipes.Length)
        {
            Debug.LogWarning("Recipe index out of bounds.");
            return null;
        }

        return recipes[currentRecipeIndex];
    }

    // Only called when the game starts, to set a random recipe for the player to cook.
    public void SetRandomRecipeIndex()
    {
        if (recipes.Length == 0)
        {
            Debug.LogWarning("Recipe database is empty or not assigned.");
            return;
        }
        currentRecipeIndex = Random.Range(0, recipes.Length);
    }
}
