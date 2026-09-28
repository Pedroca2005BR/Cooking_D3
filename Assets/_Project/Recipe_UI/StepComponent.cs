using Pedroca2005BR.Utilities;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class StepComponent : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Toggle toggle;
    [SerializeField] TextMeshProUGUI textMesh;

    [Header("Responsiveness")]
    [SerializeField] float strikethroughCooldown = 0.05f;
    [SerializeField] Color completedColor = Color.green;

    StepData data;
    public bool IsCompleted {  get; private set; }
    int quantity = 0;
    Color originalColor;

    public void Setup(StepData data)
    {
        this.data = data;
        IsCompleted = false;

        // Visuals
        toggle.isOn = false;
        textMesh.text = data.textToShow;

        // Events
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        EventManager.Subscribe(EventConstantNames.INGREDIENT_CREATED, OnIngredientCreated);
        EventManager.Subscribe(EventConstantNames.INGREDIENT_CONSUMED, OnIngredientConsumed);
    }

    private void OnIngredientCreated(object obj)
    {
        // Only receives FoodEvents
        IngredientEventData ingredient = (IngredientEventData)obj;

        if (ingredient != null)
        {
            if (ingredient.ingredientData == data.ingredient)
            {
                quantity++;
            }
        }

        if (quantity >= data.amount)
        {
            CheckMark();
        }
    }

    public void CheckMark(bool positive = true)
    {
        if (positive)
        {
            StartCoroutine(StrikeTextCoroutine());
            originalColor = textMesh.color;
            textMesh.color = completedColor;
            toggle.isOn = true;
        }
        else
        {
            // TODO: unstrike
            textMesh.color = originalColor;
            toggle.isOn = false;
        }
    }

    private IEnumerator StrikeTextCoroutine()
    {
        string originalText = textMesh.text;
        

        for (int i = 1; i <= originalText.Length; i++)
        {
            string before = originalText[..i];
            string after = originalText[i..];

            textMesh.text = $"<s>{before}</s>{after}";

            yield return new WaitForSeconds(strikethroughCooldown);
        }
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventConstantNames.INGREDIENT_CREATED, OnIngredientCreated);
        EventManager.Unsubscribe(EventConstantNames.INGREDIENT_CONSUMED, OnIngredientConsumed);
    }

    private void OnIngredientConsumed(object obj)
    {
        // Only receives FoodEvents
        IngredientEventData ingredient = (IngredientEventData)obj;

        if (ingredient != null)
        {
            if (ingredient.ingredientData == data.ingredient)
            {
                quantity--;

                if (quantity < 0)
                {
                    Debug.LogError($"More ingredients beign consumed than created! Check Event Calls for {data.ingredient.baseName}!");
                }
            }
        }
    }
}
