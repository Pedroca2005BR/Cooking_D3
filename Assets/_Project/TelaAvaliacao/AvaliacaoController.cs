using UnityEngine;

public class AvaliacaoController : MonoBehaviour
{
    [Header("Conexões de Sistemas")]
    [SerializeField] private PlateComponent plateComponent;
    [SerializeField] private RecipeDatabase recipeDatabase;
    [SerializeField] private AvaliacaoUI avaliacaoUI;

    [Header("Artes Globais")]
    [Tooltip("Estrelas(Da pior para a melhor)")]
    [SerializeField] private Sprite[] spritesEstrelas = new Sprite[5];

    [Tooltip("Velhinha(Da pior para a melhor)")]
    [SerializeField] private Sprite[] spritesVelhinha = new Sprite[4];

    private void OnEnable()
    {
        if (plateComponent != null)
            plateComponent.OnScoreCalculated += ProcessarAvaliacao;
    }

    private void OnDisable()
    {
        if (plateComponent != null)
            plateComponent.OnScoreCalculated -= ProcessarAvaliacao;
    }

    private void Start()
    {
        // Garante que a tela de avaliação começa escondida
        if (avaliacaoUI != null) avaliacaoUI.EsconderTela();
    }

    private void ProcessarAvaliacao(float score)
    {
        // 1. Calcula as estrelas
        float notaNormalizada = score * 100f;

        int quantidadeEstrelas = Mathf.CeilToInt(notaNormalizada / 20f);
        quantidadeEstrelas = Mathf.Clamp(quantidadeEstrelas, 1, 5);

        int indiceVariacao = ObterIndiceDaVariacao(quantidadeEstrelas);

        // 3. Busca a receita atual
        RecipeSO receitaAtual = recipeDatabase.GetCurrentRecipe();
        Sprite spritePrato = null;
        
        if (receitaAtual != null && receitaAtual.variacoesDoPrato.Length == 4)
        {
            spritePrato = receitaAtual.variacoesDoPrato[indiceVariacao];
        }
        else
        {
            Debug.LogWarning("O array de variações do prato não está configurado corretamente na Receita!");
        }

        // 4. Busca os sprites globais
        Sprite spriteEstrela = spritesEstrelas[quantidadeEstrelas - 1]; 
        Sprite spriteVelha = spritesVelhinha[indiceVariacao];

        int notaFinalInteira = Mathf.RoundToInt(notaNormalizada);
        // 5. Envia para a UI
        avaliacaoUI.MostrarTela(notaFinalInteira, spriteEstrela, spriteVelha, spritePrato);
    }

    private int ObterIndiceDaVariacao(int estrelas)
    {
        if (estrelas == 1) return 0; // Pior estado
        if (estrelas == 2 || estrelas == 3) return 1; // Estado mediano
        if (estrelas == 4) return 2; // Estado bom
        return 3; // Estado perfeito
    }
}