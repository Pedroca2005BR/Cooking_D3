using UnityEngine;

public class AvaliacaoController : MonoBehaviour
{
    [Header("Conexões de Sistemas")]
    [SerializeField] private PlateComponent plateComponent;
    [SerializeField] private RecipeDatabase recipeDatabase;
    [SerializeField] private AvaliacaoUI avaliacaoUI;

    [Header("Artes Globais")]
    [Tooltip("Coloque as 5 imagens completas de estrelas (da pior para a melhor)")]
    [SerializeField] private Sprite[] spritesEstrelas = new Sprite[5];
    
    [Tooltip("Coloque as 4 emoções da Velhinha (Ordem: 1, 2-3, 4, 5)")]
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
        // Se a nota for 0.0 -> CeilToInt vira 0 -> Clamp trava no mínimo de 1.
        // Se a nota for 1.0 (100%) -> 1.0 * 5 = 5.
        int quantidadeEstrelas = Mathf.CeilToInt(score * 5f);
        quantidadeEstrelas = Mathf.Clamp(quantidadeEstrelas, 1, 5);

        // 2. Define o índice que busca a arte correta (0 a 3) baseado na sua regra de variações
        int indiceVariacao = ObterIndiceDaVariacao(quantidadeEstrelas);

        // 3. Busca a receita atual para pegar o prato correto
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
        // Se tirou 1 estrela, busca o índice 0. Se tirou 5, busca o 4.
        Sprite spriteEstrela = spritesEstrelas[quantidadeEstrelas - 1]; 
        Sprite spriteVelha = spritesVelhinha[indiceVariacao];

        // 5. Envia tudo mastigado para a UI exibir
        avaliacaoUI.MostrarTela(score, spriteEstrela, spriteVelha, spritePrato);
    }

    // Sua regra de variações (4 variações para 5 estrelas)
    private int ObterIndiceDaVariacao(int estrelas)
    {
        if (estrelas == 1) return 0; // Pior estado
        if (estrelas == 2 || estrelas == 3) return 1; // Estado mediano
        if (estrelas == 4) return 2; // Estado bom
        return 3; // 5 estrelas (Estado perfeito)
    }
}