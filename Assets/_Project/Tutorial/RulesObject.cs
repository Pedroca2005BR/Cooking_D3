using UnityEngine;

// <summary> RulesObect holds both the game mode and the recipe database. Used to get random recipes and decide this is a tutorial run. </summary>
[CreateAssetMenu(fileName = "RulesObject", menuName = "Scriptable Objects/RulesObject")]
public class RulesObject : ScriptableObject
{
    public GameMode gameMode = GameMode.Classic;
    public RecipeDatabase recipeDatabase;

    public RecipeSO GetRandomRecipe()
    {
        if (recipeDatabase == null || recipeDatabase.recipes.Length == 0)
        {
            Debug.LogWarning("Recipe database is empty or not assigned.");
            return null;
        }
        int randomIndex = Random.Range(0, recipeDatabase.recipes.Length);
        return recipeDatabase.recipes[randomIndex];
    }
}

public enum GameMode
{
    Classic = 0,
    Tutorial = 1
}