using System.Collections.Generic;
using UnityEngine;

// The ideal ingredient may be used to compare the actual ingredient with the ideal one, and calculate a score based on how close they are. It is used in the ScoreCalculator class.
[CreateAssetMenu(fileName = "ingredIdeal", menuName = "Scriptable Objects/Ideal Ingredient")]
public class IdealIngredientData : ScriptableObject
{
    public BaseIngredientData ingredient;
    public List<ProcessTarget> processes;

    [Tooltip("Ingredientes que foram misturados para formar este. Vazio = ingrediente simples.")]
    public List<IdealIngredientData> components;
}

[System.Serializable]
public class ProcessTarget
{
    public CookingProcess process;
    [Range(0, 100)] public int idealScore = 100;
}
