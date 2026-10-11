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

    private void OnEnable()
    {
        EventManager.Subscribe(EventConstantNames.INGREDIENT_STOLEN, ReporIngredienteRoubado);
    }

    // 2. Remove a assinatura quando a geladeira desliga (previne memory leaks)
    private void OnDisable()
    {
        EventManager.Unsubscribe(EventConstantNames.INGREDIENT_STOLEN, ReporIngredienteRoubado);
    }
    private void Awake()
    {
        // Sorteia a receita oficial da rodada na Base de Dados antes de qualquer script executar o Start()
        if (recipeDatabase != null)
        {
            recipeDatabase.SetRandomRecipeIndex();
        }
    }

    private void Start()
    {
        GerarIngredientesDaReceita();
    }

    public void GerarIngredientesDaReceita()
    {
        RecipeSO receitaAtual = recipeDatabase.GetCurrentRecipe();
        if (receitaAtual == null) return;

        List<BaseIngredientData> ingredientesParaGerar = new List<BaseIngredientData>();

        // Lê a lista de StepData e extrai os ingredientes com base na quantidade
        foreach (StepData step in receitaAtual.ingredientesNecessarios)
        {
            for (int i = 0; i < step.amount; i++)
            {
                ingredientesParaGerar.Add(step.ingredient);
            }
        }

        if (ingredientesParaGerar.Count == 0)
        {
            Debug.LogWarning($"A receita {receitaAtual.name} não tem nenhum item na lista 'Ingredientes Necessarios'!");
            return;
        }

        if (ingredientesParaGerar.Count > pontosDeSpawn.Length)
        {
            Debug.LogError("A receita precisa de mais ingredientes do que o número de pontos de spawn!");
            return;
        }

        // Embaralha os pontos de spawn (Fisher-Yates) para nascerem em lugares aleatórios
        List<Transform> posicoesSorteio = new List<Transform>(pontosDeSpawn);
        for (int i = 0; i < posicoesSorteio.Count; i++)
        {
            Transform temp = posicoesSorteio[i];
            int randomIndex = Random.Range(i, posicoesSorteio.Count);
            posicoesSorteio[i] = posicoesSorteio[randomIndex];
            posicoesSorteio[randomIndex] = temp;
        }

        // Instancia os prefabs e configura os dados
        for (int i = 0; i < ingredientesParaGerar.Count; i++)
        {
            BaseIngredientData dado = ingredientesParaGerar[i];
            
            if (dado == null)
            {
                Debug.LogError($"O ingrediente na posição {i} está nulo no ScriptableObject! O loop vai pular este item para não quebrar o jogo.");
                continue;
            }

            Transform ponto = posicoesSorteio[i];
            GameObject clone = Instantiate(prefabGenericoIngrediente, ponto.position, Quaternion.identity, ponto);

            DraggableComponent arrasto = clone.GetComponent<DraggableComponent>();
            if (arrasto != null) arrasto.enabled = false;

            clone.transform.localScale = new Vector3(escalaNaGeladeira, escalaNaGeladeira, 1f);

            IngredientComponent comp = clone.GetComponent<IngredientComponent>();
            if (comp != null)
            {
                comp.Setup(dado);
            }

            InteracaoGeladeira interacao = clone.AddComponent<InteracaoGeladeira>();
            interacao.Setup(zonaDaBancada);
        }
    }

    private void ReporIngredienteRoubado(object obj)
    {
        // Converte a mensagem recebida para o formato de dados do seu jogo
        IngredientEventData eventData = obj as IngredientEventData;
        if (eventData == null || eventData.ingredientData == null) return;

        BaseIngredientData ingredienteRoubado = eventData.ingredientData;

        // Procura uma prateleira (ponto de spawn) que esteja VAZIA
        Transform pontoVazio = null;
        foreach (Transform ponto in pontosDeSpawn)
        {
            // Como instanciamos os itens como "filhos" dos pontos, 
            // se o childCount for 0, sabemos que aquele berço está livre!
            if (ponto.childCount == 0)
            {
                pontoVazio = ponto;
                break;
            }
        }

        // Se a geladeira estiver lotada, cancela a reposição
        if (pontoVazio == null)
        {
            Debug.LogWarning($"A geladeira tentou repor {ingredienteRoubado.name}, mas não há espaço nas prateleiras!");
            return;
        }

        // Instancia a reposição e injeta as dependências de forma segura
        GameObject clone = Instantiate(prefabGenericoIngrediente, pontoVazio.position, Quaternion.identity, pontoVazio);

        DraggableComponent arrasto = clone.GetComponent<DraggableComponent>();
        if (arrasto != null) arrasto.enabled = false;
        clone.transform.localScale = new Vector3(escalaNaGeladeira, escalaNaGeladeira, 1f);

        IngredientComponent comp = clone.GetComponent<IngredientComponent>();
        if (comp != null)
        {
            comp.Setup(ingredienteRoubado);
        }

        InteracaoGeladeira interacao = clone.AddComponent<InteracaoGeladeira>();
        interacao.Setup(zonaDaBancada);
        
        Debug.Log($"A Geladeira repôs o ingrediente {ingredienteRoubado.name} que foi roubado!");
    }
}