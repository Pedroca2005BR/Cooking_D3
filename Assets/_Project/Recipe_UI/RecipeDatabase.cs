using UnityEngine;

[CreateAssetMenu(fileName = "RecipeDatabase", menuName = "Scriptable Objects/RecipeDatabase")]
public class RecipeDatabase : ScriptableObject
{
    public RecipeSO[] recipes;
}
