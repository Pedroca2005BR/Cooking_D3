using Pedroca2005BR.Utilities;
using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(DraggableComponent))]
public class IngredientComponent : MonoBehaviour
{
    public BaseIngredientData data;
    public IngredientRuntimeInstance ingredientInstance {  get; private set; }

    SpriteRenderer spriteRenderer;
    DraggableComponent draggable;

    // Rat
    RatBehaviour rat;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ingredientInstance = new();
        draggable = GetComponent<DraggableComponent>();
    }

    private void OnGrabbedEvent(bool obj)
    {
        if (obj)
            EventManager.TriggerEvent(EventConstantNames.INGREDIENT_PICKED_UP, new IngredientEventData(data, this, this));
        else
            EventManager.TriggerEvent(EventConstantNames.INGREDIENT_DROPPED, new IngredientEventData(data, this, this));
    }

    public void Setup(BaseIngredientData ingredientData, IngredientRuntimeInstance[] baseComponents = null)
    {
        // Setting up basic info
        data = ingredientData;
        ingredientInstance.SetBaseComponents(baseComponents);

        // Visuals
        spriteRenderer.sprite = data.baseSprite;

        // Event Trigger
        EventManager.TriggerEvent(EventConstantNames.INGREDIENT_CREATED, new IngredientEventData(data, this, this));
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
                EventManager.TriggerEvent(EventConstantNames.INGREDIENT_CONSUMED, new IngredientEventData(data, this, this));
                Setup(newData);
            }
        }
        else
        {
            Debug.LogError($"No match transformation for process ({process.ToString()} => {data.baseName})");
        }
    }

    public bool TryGrabbing(RatBehaviour rat)
    {
        if (this.rat == null)
        {
            this.rat = rat;
            draggable.enabled = false;
            EventManager.TriggerEvent(EventConstantNames.INGREDIENT_PICKED_UP, new IngredientEventData(data, this, rat));
            return true;
        }

        return false;
    }

    public void RatKilled()
    {
        rat = null;
        draggable.enabled = true;
    }

    public void OnDisable()
    {
        if (rat == null)
        { 
            EventManager.TriggerEvent(EventConstantNames.INGREDIENT_CONSUMED, new IngredientEventData(data, this, this));
        }

        draggable.OnGrabbed -= OnGrabbedEvent;
    }

    public void OnEnable()
    {
        draggable.OnGrabbed += OnGrabbedEvent;
    }
}