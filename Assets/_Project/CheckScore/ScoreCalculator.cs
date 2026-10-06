using System.Collections.Generic;
using UnityEngine;

// Advised to not change this class, as it is used to calculate the score of the recipe. If you want to change the scoring system, create a new class that inherits from this one and override the Calculate method.
public static class ScoreCalculator
{
    public const int MaxScore = 100;
    const int MaxDepth = 10; // proteção caso o designer crie um ciclo por engano

    public static float Calculate(IngredientRuntimeInstance actual, IdealIngredientData ideal)
    {
        var (diff, max) = Compare(actual, ideal, 0);
        return max <= 0 ? 1f : Mathf.Clamp01(1f - diff / max);
    }

    static (float diff, float max) Compare(IngredientRuntimeInstance actual, IdealIngredientData ideal, int depth)
    {
        float diff = 0, max = 0;
        if (depth > MaxDepth) return (diff, max);

        // 1) Processos
        foreach (var target in ideal.processes)
        {
            int actualScore = 0; // processo faltante = 0
            if (actual != null) actual.ProcessScores.TryGetValue(target.process, out actualScore);

            diff += Mathf.Abs(target.idealScore - actualScore);
            max += Mathf.Max(target.idealScore, MaxScore - target.idealScore); // pior diferença possível
        }

        // 2) Componentes (casamento guloso por tipo de ingrediente)
        var remaining = actual != null
            ? new List<IngredientRuntimeInstance>(actual.BaseComponents)
            : new List<IngredientRuntimeInstance>();

        foreach (var idealComp in ideal.components)
        {
            IngredientRuntimeInstance best = null;
            float bestDiff = float.MaxValue;

            foreach (var c in remaining)
            {
                if (c.Data != idealComp.ingredient) continue;
                float d = Compare(c, idealComp, depth + 1).diff;
                if (d < bestDiff) { bestDiff = d; best = c; }
            }

            if (best != null) remaining.Remove(best);

            // best == null => componente faltante: tudo conta como 0
            var (cd, cm) = Compare(best, idealComp, depth + 1);
            diff += cd;
            max += cm;
        }

        return (diff, max);
    }
}
