using System.Collections.Generic;
using UnityEngine;
using Pedroca2005BR.Utilities;

public class GeladeiraSpawner : MonoBehaviour
{
    [Header("Conexões Principais")]
    [SerializeField] private RecipeDatabase recipeDatabase;
    [SerializeField] private GameObject prefabGenericoIngrediente;
    
    [Header("A Bandeja Temporária (Bancada.cs)")]
    [Tooltip("Arraste o objeto que tem o script Bancada.cs e o Colisor de Trigger.")]
    [SerializeField] private Transform zonaDaBancada;

    [Header("Prateleiras da Geladeira")]
    [Tooltip("Arraste os GameObjects vazios que você posicionou nas prateleiras.")]
    [SerializeField] private Transform[] pontosDeSpawn;

    [Header("Visual")]
    [Tooltip("Escala do ingrediente enquanto estiver dentro da geladeira.")]
    [SerializeField] private float escalaNaGeladeira = 0.4f;
    [SerializeField] private float tempoDeRecarga = 15f;

    private float timer = 0f;

    private void Awake()
    {
        if (recipeDatabase != null)
        {
            recipeDatabase.SetRandomRecipeIndex();
        }
    }

    private void Start()
    {
        GerarIngredientesDaReceita();
    }

    private void Update()
    {
        // Conta o tempo e recarrega a geladeira quando atingir o limite
        timer += Time.deltaTime;
        if (timer >= tempoDeRecarga)
        {
            timer = 0f;
            RecarregarPrateleiras();
        }
    }

    private void RecarregarPrateleiras()
    {
        // 1. Vasculha todos os pontos de spawn e destrói os ingredientes antigos que o jogador não pegou
        foreach (Transform ponto in pontosDeSpawn)
        {
            foreach (Transform filho in ponto)
            {
                Destroy(filho.gameObject);
            }
        }

        // 2. Chama a função principal para gerar tudo novamente em posições aleatórias
        GerarIngredientesDaReceita();
    }

    public void GerarIngredientesDaReceita()
    {
        RecipeSO receitaAtual = recipeDatabase.GetCurrentRecipe();
        if (receitaAtual == null) return;

        List<BaseIngredientData> ingredientesParaGerar = new List<BaseIngredientData>();

        foreach (StepData step in receitaAtual.ingredientesNecessarios)
        {
            for (int i = 0; i < step.amount; i++)
            {
                ingredientesParaGerar.Add(step.ingredient);
            }
        }

        if (ingredientesParaGerar.Count == 0 || ingredientesParaGerar.Count > pontosDeSpawn.Length)
        {
            return; // Previne erros se a receita for inválida
        }

        // Embaralha os pontos de spawn (Fisher-Yates)
        List<Transform> posicoesSorteio = new List<Transform>(pontosDeSpawn);
        for (int i = 0; i < posicoesSorteio.Count; i++)
        {
            Transform temp = posicoesSorteio[i];
            int randomIndex = Random.Range(i, posicoesSorteio.Count);
            posicoesSorteio[i] = posicoesSorteio[randomIndex];
            posicoesSorteio[randomIndex] = temp;
        }

        // Instancia os prefabs nas novas posições sorteadas
        for (int i = 0; i < ingredientesParaGerar.Count; i++)
        {
            BaseIngredientData dado = ingredientesParaGerar[i];
            if (dado == null) continue;

            Transform ponto = posicoesSorteio[i];
            GameObject clone = Instantiate(prefabGenericoIngrediente, ponto.position, Quaternion.identity, ponto);

            DraggableComponent arrasto = clone.GetComponent<DraggableComponent>();
            if (arrasto != null) arrasto.enabled = false;

            clone.transform.localScale = new Vector3(escalaNaGeladeira, escalaNaGeladeira, 1f);

            IngredientComponent comp = clone.GetComponent<IngredientComponent>();
            if (comp != null) comp.Setup(dado);

            InteracaoGeladeira interacao = clone.AddComponent<InteracaoGeladeira>();
            interacao.Setup(zonaDaBancada);
        }
    }
}