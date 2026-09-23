using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(DraggableComponent))]
public class IngredientComponent : MonoBehaviour
{
    public BaseIngredientData data;
    public IngredientRuntimeInstance ingredientInstance {  get; private set; }

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ingredientInstance = new();
    }


    public void Setup(BaseIngredientData ingredientData, IngredientRuntimeInstance[] baseComponents = null)
    {
        // Setting up basic info
        data = ingredientData;
        ingredientInstance.SetBaseComponents(baseComponents);

        // Visuals
        spriteRenderer.sprite = data.baseSprite;
    }

    // This function will be used by cooking processors (pan, knife, microwave, etc.) and redirected to RuntimeInstance
    public void AddProcess(CookingProcess process, int score)
    {
        ingredientInstance.AddProcessScore(process, score);

        // Tries to update the sprite
        if (data.TryTransforming(process, score, out var newData))
        {
            if (newData != null)
            {
                Setup(newData);
            }
        }
        else
        {
            Debug.LogError($"No match transformation for process ({process.ToString()} => {data.baseName})");
        }
    }
}