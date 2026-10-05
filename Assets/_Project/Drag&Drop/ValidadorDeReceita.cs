using UnityEngine;

public static class ValidadorDeReceita
{
    // Validador para bancadas que transformam itens (Frigideira, Fritadeira, Tabua de Corte)
    public static bool AceitaProcesso(IngredientComponent ingrediente, CookingProcess processoDaBancada)
    {
        if (ingrediente == null || ingrediente.data == null) return false;

        // Verifica se o ScriptableObject do ingrediente possui uma conversao para aquele processo
        foreach (var conversao in ingrediente.data.possibleConversions)
        {
            if (conversao.processNeeded == processoDaBancada) return true;
        }
        
        return false; // Se o processo nao existir na receita, bloqueia a entrada
    }

    // Validador para bancadas que misturam itens (Bacia de Ovo, Mesa de Montagem)
    public static bool AceitaFusao(IngredientFuserComponent fuser, IngredientComponent ing1, IngredientComponent ing2)
    {
        if (fuser == null || ing1 == null || ing2 == null) return false;

        IngredientComponent[] paraTestar = { ing1, ing2 };
        return fuser.CanFuse(paraTestar);
    }
}