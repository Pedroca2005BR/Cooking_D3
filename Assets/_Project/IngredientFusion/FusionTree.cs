using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FusionTree", menuName = "Scriptable Objects/FusionTree")]
public class FusionTree : ScriptableObject
{
    public List<FusionData> possibleFusions;

    public BaseIngredientData TryFusing(BaseIngredientData[] components, out bool onRightPath)
    {
        onRightPath = false;

        foreach (var fusion in possibleFusions)
        {
            switch (fusion.Compare(components))
            {
                case FusionMatch.Complete:
                    onRightPath = true;
                    return fusion.compositeIngredient;

                case FusionMatch.Partial:
                    onRightPath = true;   // continua procurando: outra receita pode estar completa
                    break;
            }
        }

        return null;
    }
}

public enum FusionMatch
{
    None,       // tem ingrediente que não pertence à receita (ou repetido demais)
    Partial,    // tudo que foi fornecido pertence à receita, mas ainda falta algo
    Complete    // exatamente os componentes da receita
}

[System.Serializable]
public struct FusionData
{
    public BaseIngredientData compositeIngredient;
    public List<BaseIngredientData> components;

    public FusionMatch Compare(BaseIngredientData[] ingredients)
    {
        if (ingredients == null || ingredients.Length == 0) return FusionMatch.None;

        // Copia os componentes necessários para podermos "marcá-los" como encontrados
        var stillNeeded = new List<BaseIngredientData>(components);

        foreach (var ingredient in ingredients)
        {
            int index = stillNeeded.IndexOf(ingredient);

            // Ingrediente que a receita não pede (ou que já foi usado)
            if (index == -1)
                return FusionMatch.None;

            stillNeeded.RemoveAt(index);
        }

        return stillNeeded.Count == 0 ? FusionMatch.Complete : FusionMatch.Partial;
    }
}
