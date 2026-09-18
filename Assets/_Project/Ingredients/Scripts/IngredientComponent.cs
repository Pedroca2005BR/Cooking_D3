using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(DraggableComponent))]
public class IngredientComponent : MonoBehaviour
{
    [SerializeField] BaseIngredientData data;
    public IngredientRuntimeInstance ingredientInstance {  get; private set; }

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void Setup(BaseIngredientData ingredientData, IngredientRuntimeInstance[] baseComponents = null)
    {
        // Setting up basic info
        data = ingredientData;
        ingredientInstance = new(ingredientData);
        ingredientInstance.SetBaseComponents(baseComponents);

        // Visuals
        spriteRenderer.sprite = data.baseSprite;
    }

    // This function will be used by cooking processors (pan, knife, microwave, etc.) and redirected to RuntimeInstance
    public void AddProcess(CookingProcess process, int score)
    {
        ingredientInstance.AddProcessScore(process, score);

        // Tries to update the sprite
        if (data.TryGetNewSprite(process, score, out Sprite spr, out Color col))
        {
            spriteRenderer.sprite = spr;
            spriteRenderer.color = col;
        }
    }
}