using UnityEngine;

// <summary> RulesObect holds both the game mode and the recipe database. Used to get random recipes and decide this is a tutorial run. </summary>
[CreateAssetMenu(fileName = "RulesObject", menuName = "Scriptable Objects/RulesObject")]
public class RulesObject : ScriptableObject
{
    public GameMode gameMode = GameMode.Classic;
    public RecipeDatabase recipeDatabase;
    public int currentRecipeIndex = 0;

    public RecipeSO GetCurrentRecipe()
    {
        if (recipeDatabase == null || recipeDatabase.recipes.Length == 0)
        {
            Debug.LogWarning("Recipe database is empty or not assigned.");
            return null;
        }
        else if (currentRecipeIndex < 0 || currentRecipeIndex >= recipeDatabase.recipes.Length)
        {
            Debug.LogWarning("Recipe index out of bounds.");
            return null;
        }

        return recipeDatabase.recipes[currentRecipeIndex];
    }

    // Only called when the game starts, to set a random recipe for the player to cook.
    public void SetRandomRecipeIndex()
    {
        if (recipeDatabase == null || recipeDatabase.recipes.Length == 0)
        {
            Debug.LogWarning("Recipe database is empty or not assigned.");
            return;
        }
        currentRecipeIndex = Random.Range(0, recipeDatabase.recipes.Length);
    }
}

public enum GameMode
{
    Classic = 0,
    Tutorial = 1
}