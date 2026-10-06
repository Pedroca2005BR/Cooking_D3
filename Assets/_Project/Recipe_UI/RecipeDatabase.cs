using UnityEngine;

// <summary>
// The recipe database will contain all the recipes in the game. An script may call it to get the recipe data.
[CreateAssetMenu(fileName = "RecipeDatabase", menuName = "Scriptable Objects/RecipeDatabase")]
public class RecipeDatabase : ScriptableObject
{
    public RecipeSO[] recipes;
}
