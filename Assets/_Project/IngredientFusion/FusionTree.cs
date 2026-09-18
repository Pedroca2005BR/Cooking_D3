using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FusionTree", menuName = "Scriptable Objects/FusionTree")]
public class FusionTree : ScriptableObject
{
    public List<FusionData> possibleFusions;

    public BaseIngredientData TryFusing(BaseIngredientData[] components)
    {
        foreach (var fusion in possibleFusions)
        {
            if (fusion.TestComponents(components))
                return fusion.compositeIngredient;
        }

        return null;
    }
}

[System.Serializable]
public struct FusionData
{
    public BaseIngredientData compositeIngredient;
    public List<FusionComponent> components;

    public bool TestComponents(BaseIngredientData[] ingredients)
    {
        // Copia os ingredientes para podermos "consumi-los"
        List<BaseIngredientData> remainingIngredients =
            new List<BaseIngredientData>(ingredients);

        foreach (var component in components)
        {
            if (!component.isRequired)
                continue;

            int index = remainingIngredients.IndexOf(component.ingredient);

            if (index == -1)
                return false;

            remainingIngredients.RemoveAt(index);
        }

        return true;
    }
}

[System.Serializable]
public struct FusionComponent
{
    public BaseIngredientData ingredient;
    public bool isRequired;
}