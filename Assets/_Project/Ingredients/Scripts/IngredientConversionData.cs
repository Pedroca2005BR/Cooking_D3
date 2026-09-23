using UnityEngine;

[System.Serializable]
public struct IngredientConversionData
{
    public CookingProcess processNeeded;
    public int minimumScoreNeeded;

    public BaseIngredientData newIngredientData;
}
