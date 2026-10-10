using UnityEngine;

public static class ValidadorDeReceita
{
    public static bool AceitaProcesso(IngredientComponent ingrediente, CookingProcess processoDaBancada)
    {
        return ingrediente != null && ingrediente.data != null;
    }

    // Validador para bancadas que misturam itens (Bacia de Ovo, Mesa de Montagem)
    public static bool AceitaFusao(IngredientFuserComponent fuser, IngredientComponent[] ingredientesParaTestar)
    {
        //array permite fusões com mais de 2 ingredientes.
        if (fuser == null || ingredientesParaTestar == null) return false;

        //verifica apenas se as referências físicas são válidas.
        FusionResult resultado = fuser.Evaluate(ingredientesParaTestar, out _);
        return fuser.CanFuse(ingredientesParaTestar) || resultado == FusionResult.Incomplete;
    }
}