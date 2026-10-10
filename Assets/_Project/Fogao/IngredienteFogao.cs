using Unity.Multiplayer.Center.Common;
using Unity.VisualScripting;
using UnityEngine;

public class IngredienteFogao : MonoBehaviour
{
    [Header("Estado Atual")]
    [SerializeField] private float tempoCozido = 0f;
    [Header("Configuracao")]
    [SerializeField] private float tempoGororoba = 10f;

    private float tempoNecessario;
    private bool processado = false;
    public bool jaFoiCortado = false;

    private SpriteRenderer spriteIng;
    private FoodProcessorComponent processoAtual;
    public IngredientComponent ingComponent { get; private set;}

    private void Awake() {
        spriteIng = GetComponent<SpriteRenderer>();
        ingComponent = GetComponent<IngredientComponent>();
        ingComponent.OnSetup += AjustarColisor;
    }

    void OnDisable()
    {
        ingComponent.OnSetup -= AjustarColisor;
    }

    public void DefinirProcessador(FoodProcessorComponent processo) {
        //seta neste metodo caso o novo ingrediente continue no mesmo processador
        processoAtual = processo;
        processado = false;
        tempoCozido = 0f;

        if(processoAtual != null && ingComponent != null && ingComponent.data != null) {
            int score = ingComponent.data.GetScoreNeeded(processoAtual.ProcessoAtual); //score necessario para o processo

            //Se o processo for valido, utiliza o tempo da receita. Se for invalido utliza o tempo para gororoba
            tempoNecessario = (score != -1) ? score : tempoGororoba; 
        }
    }

    public void RecebeCalor(float tempoNoFogo)
    {
        if(processoAtual == null && ingComponent == null && ingComponent.data == null) return;

        tempoCozido += tempoNoFogo;

        if(tempoCozido >= tempoNecessario) //se o tempo alcancou o limite da receita ou para virar gororoba
        {
            processado = true; 
            processoAtual.ProcessIngredient(ingComponent, (int)tempoCozido); //realiza o processo

            AjustarColisor(); //ajusta o tamanho do colisor do novo ingrediente

            DefinirProcessador(processoAtual);
        }
    }
    public void AjustarColisor()
    {
        //ajusta o tamanho do colizor de acordo com o sprite
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if(col != null && spriteIng != null && spriteIng.sprite != null) 
        {
            col.size = spriteIng.sprite.bounds.size;
        }
    }

    public float CalculaPorcentagem() {

        if(tempoNecessario <= 0) return 0f;
        return Mathf.Clamp01(tempoCozido / tempoNecessario);
    }
}