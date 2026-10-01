using Pedroca2005BR.Utilities;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    string originalText;

    public void Setup(StepData data)
    {
        this.data = data;
        IsCompleted = false;

        // Visuals
        toggle.isOn = false;
        textMesh.text = data.textToShow;
        originalColor = textMesh.color;
        originalText = textMesh.text;
    }

    // When the player adds an ingredient, this method should be called to update the quantity and check if the step is completed.
    // If the step is already completed, it just adds the amount.
    public void UpdateQuantity(int qtdAdded)
    {
        quantity += qtdAdded;
        if (quantity >= data.amount && !IsCompleted)
        {
            SetStepCompleted(true);
        }
    }

    public bool TryToUnmarkStep()
    {
        if (quantity < data.amount && IsCompleted)
        {
            SetStepCompleted(false);
            return true;
        }

        return false;
    }

    public void SetStepCompleted(bool completed)
    {
        if (completed)
        {
            StartCoroutine(StrikeTextCoroutine());
            textMesh.color = completedColor;
            toggle.isOn = true;
            IsCompleted = true;
        }
        else
        {
            StopAllCoroutines();
            textMesh.text = originalText;
            textMesh.color = originalColor;
            toggle.isOn = false;
            IsCompleted = false;
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
}
