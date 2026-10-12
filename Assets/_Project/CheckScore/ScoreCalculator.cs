using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ScoreCalculator
{
    public const int MaxScore = 100;
    const int MaxDepth = 10; // proteção caso o designer crie um ciclo por engano

    public static float Calculate(IngredientRuntimeInstance actual, IdealIngredientData ideal)
        => Normalize(Compare(actual, ideal, 0, includeOwnProcesses: true), ideal);

    // Compara só os componentes do ideal, ignorando os processos do próprio nó
    // (usado quando os ingredientes base chegam soltos no prato).
    public static float CalculateComponents(IngredientRuntimeInstance actualRoot, IdealIngredientData ideal)
        => Normalize(Compare(actualRoot, ideal, 0, includeOwnProcesses: false), ideal);

    static float Normalize((float diff, float max) r, IdealIngredientData ideal)
    {
        if (r.max <= 0f)
        {
            // Nada a avaliar: erro de dados do ideal. Nunca dar nota máxima de graça.
            Debug.LogWarning($"O ideal '{ideal.name}' não tem processos nem componentes para avaliar. Nota 0.");
            return 0f;
        }

        return Mathf.Clamp01(1f - r.diff / r.max);
    }

    static (float diff, float max) Compare(IngredientRuntimeInstance actual, IdealIngredientData ideal, int depth, bool includeOwnProcesses = true)
    {
        float diff = 0, max = 0;
        if (depth > MaxDepth) return (diff, max);

        // 1) Processos
        if (includeOwnProcesses)
        {
            foreach (var target in ideal.processes)
            {
                int actualScore = 0; // processo faltante = 0
                if (actual != null) actual.ProcessScores.TryGetValue(target.process, out actualScore);

                diff += Mathf.Abs(target.idealScore - actualScore);
                max += Mathf.Max(target.idealScore, MaxScore - target.idealScore);
            }
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

            // Presença: estar no prato já vale pontos, mesmo sem processos
            max += MaxScore;

            var (cd, cm) = Compare(best, idealComp, depth + 1);
            max += cm;

            if (best == null)
                diff += MaxScore + cm;   // faltante: pior caso (presença + tudo que ele exigia)
            else
                diff += cd;
        }

        return (diff, max);
    }

    public static string Dump(IngredientRuntimeInstance i, int indent = 0)
    {
        string pad = new string(' ', indent * 2);
        if (i == null) return pad + "(null)\n";

        string name = i.Data != null ? i.Data.baseName : "<Data NULL>";
        string scores = string.Join(", ", i.ProcessScores.Select(kv => $"{kv.Key}={kv.Value}"));

        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"{pad}{name} [{scores}]");
        foreach (var c in i.BaseComponents) sb.Append(Dump(c, indent + 1));
        return sb.ToString();
    }
}
