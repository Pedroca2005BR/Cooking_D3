using System.Collections.Generic;
using UnityEngine;

public class IngredientRuntimeInstance
{
    BaseIngredientData data;
    public Dictionary<CookingProcess, int> ProcessScores { get; private set; } = new();
    public List<IngredientRuntimeInstance> BaseComponents { get; private set; }

    public IngredientRuntimeInstance(BaseIngredientData data)
    {
        this.data = data;
    }

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

    public void SetBaseComponents(List<IngredientRuntimeInstance> baseComponents)
    {
        BaseComponents = baseComponents;
    }

    public void SetBaseComponents(IngredientRuntimeInstance[] baseComponents)
    {
        BaseComponents = new(baseComponents);
    }
}
