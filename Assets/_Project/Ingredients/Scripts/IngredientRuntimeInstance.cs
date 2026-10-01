using System.Collections.Generic;
using UnityEngine;

public class IngredientRuntimeInstance
{
    public BaseIngredientData Data { get; set; }
    public Dictionary<CookingProcess, int> ProcessScores { get; private set; } = new();
    public List<IngredientRuntimeInstance> BaseComponents { get; private set; } = new();


    public void AddProcessScore(CookingProcess process, int score)
    {
        if (ProcessScores.ContainsKey(process))
        {
            ProcessScores[process] = score;
        }
        else
        {
            ProcessScores.Add(process, score);
        }
    }

    public void SetBaseComponents(IngredientRuntimeInstance[] baseComponents)
    {
        if (baseComponents == null) return;

        BaseComponents.AddRange(baseComponents);
    }

    // TO DO: Get Score for this and every base ingredient
}
