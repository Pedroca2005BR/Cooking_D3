using UnityEngine;
using static UnityEngine.UI.Image;

public class IngredientEventData
{
    public BaseIngredientData ingredientData;
    public IngredientComponent ingredientComponent;
    public Component origin;

    public IngredientEventData(BaseIngredientData data, IngredientComponent ingredientComponent, Component origin)
    {
        this.ingredientComponent = ingredientComponent;
        this.ingredientData = data;
        this.origin = origin;
    }
}
