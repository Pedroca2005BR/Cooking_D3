using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewChecklist", menuName = "Scriptable Objects/RecipeSO")]
public class RecipeSO : ScriptableObject
{
    public StepData[] ingredientesNecessarios;

    public StepData[] howToMakeChecklist;

    public IdealIngredientData perfectDish;

    public Sprite[] variacoesDoPratoPronto = new Sprite[4];
}

[System.Serializable]
public struct StepData
{
    [TextArea] public string textToShow;
    public BaseIngredientData ingredient;
    public int amount;
}
