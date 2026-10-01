using System.Collections.Generic;
using UnityEngine;

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
