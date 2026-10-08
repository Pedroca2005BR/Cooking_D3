using Pedroca2005BR.Utilities;
using UnityEngine;

public class IngredientFuserComponent : MonoBehaviour
{
    [SerializeField] FusionTree fusionTree;

    [Header("Junk")]
    [Tooltip("Ingrediente gerado quando a combinação não leva a nenhuma receita (a 'gororoba').")]
    public BaseIngredientData junkIngredient;

    [Header("Instantiate Settings")]
    [SerializeField] GameObject ingredientPrefab;
    [SerializeField] Transform position;

    void Awake()
    {
        if (junkIngredient == null)
            Debug.LogWarning($"{name}: junkIngredient não atribuído. Combinações erradas não vão gerar gororoba.", this);
    }

    // Avalia a combinação SEM destruir nem instanciar nada
    public FusionResult Evaluate(IngredientComponent[] ingredients, out BaseIngredientData resultData)
    {
        resultData = null;

        if (fusionTree == null || ingredients == null || ingredients.Length == 0)
            return FusionResult.Nothing;

        var ingInfo = new BaseIngredientData[ingredients.Length];
        for (int i = 0; i < ingredients.Length; i++)
            ingInfo[i] = ingredients[i].data;

        resultData = fusionTree.TryFusing(ingInfo, out bool onRightPath);

        if (resultData != null) return FusionResult.Success;
        if (onRightPath) return FusionResult.Incomplete;

        // Caminho errado: gororoba só com 2+ ingredientes (um único ingrediente não "se funde" com nada)
        if (ingredients.Length < 2 || junkIngredient == null)
            return FusionResult.Nothing;

        resultData = junkIngredient;
        return FusionResult.Junk;
    }

    // Mesmo significado de antes: existe uma fusão VÁLIDA?
    public bool CanFuse(IngredientComponent[] ingredients)
        => Evaluate(ingredients, out _) == FusionResult.Success || Evaluate(ingredients, out _) == FusionResult.Junk;

    // Retorna true se algo foi criado (fusão válida OU gororoba). Veja "result" para saber qual.
    public bool TryFusing(IngredientComponent[] ingredients, out GameObject fusedIngredient, out FusionResult result)
    {
        fusedIngredient = null;
        result = Evaluate(ingredients, out BaseIngredientData newIng);

        if (result != FusionResult.Success && result != FusionResult.Junk)
            return false;

        var runtimeInstances = new IngredientRuntimeInstance[ingredients.Length];
        for (int i = 0; i < ingredients.Length; i++)
            runtimeInstances[i] = ingredients[i].ingredientInstance;

        if (position == null) position = transform;

        fusedIngredient = Instantiate(ingredientPrefab, position.position, Quaternion.identity);
        fusedIngredient.GetComponent<IngredientComponent>().Setup(newIng, runtimeInstances);

        for (int i = 0; i < ingredients.Length; i++)
            Destroy(ingredients[i].gameObject);

        return true;
    }

    public bool TryFusing(IngredientComponent[] ingredients, out GameObject fusedIngredient)
    {
        fusedIngredient = null;
        FusionResult result = Evaluate(ingredients, out BaseIngredientData newIng);

        if (result != FusionResult.Success && result != FusionResult.Junk)
            return false;

        var runtimeInstances = new IngredientRuntimeInstance[ingredients.Length];
        for (int i = 0; i < ingredients.Length; i++)
            runtimeInstances[i] = ingredients[i].ingredientInstance;

        if (position == null) position = transform;

        fusedIngredient = Instantiate(ingredientPrefab, position.position, Quaternion.identity);
        fusedIngredient.GetComponent<IngredientComponent>().Setup(newIng, runtimeInstances);

        for (int i = 0; i < ingredients.Length; i++)
            Destroy(ingredients[i].gameObject);

        return true;
    }
}

public enum FusionResult
{
    Nothing,     // nada a fazer (vazio, ingrediente solto fora de qualquer receita...)
    Incomplete,  // no caminho certo: aguardando mais ingredientes
    Success,     // fusão válida
    Junk         // caminho errado: vira gororoba
}