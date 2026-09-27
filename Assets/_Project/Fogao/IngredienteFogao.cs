using UnityEngine;

public class IngredienteFogao : MonoBehaviour
{
    public enum EstadoCozimento { Cru, Cozido, Queimado}

    public IngredienteDataSO dadosBase;

    [Header("Estado Atual")]
    [SerializeField] private float tempoCozido = 0f;
    private EstadoCozimento estado = EstadoCozimento.Cru;
    public bool jaFoiCortado = false;

    [Header("Notas Provisorias")]
    [SerializeField] private int notaCozido = 100;
    [SerializeField] private int notaQueimado = 0;

    private SpriteRenderer spriteIng;
    private FoodProcessorComponent processoAtual;
    public IngredientComponent ingComponent { get; private set;}

    private void Awake() {
        spriteIng = GetComponent<SpriteRenderer>();
        ingComponent = GetComponent<IngredientComponent>();
    }

    private void Start() {
        if(ingComponent.data != null) {
            ingComponent.Setup(ingComponent.data);
            AjustarColisor();
        }
    }

    public void DefinirProcessador(FoodProcessorComponent processo) {
        processoAtual = processo;
    }

    public void RecebeCalor(float tempoNoFogo) {
        if(estado == EstadoCozimento.Queimado || dadosBase == null) return;

        tempoCozido += tempoNoFogo;

        if(estado == EstadoCozimento.Cru && tempoCozido >= dadosBase.tempoCozinhar) {
            FicarPronto();
        }else if(estado == EstadoCozimento.Cozido && tempoCozido >= dadosBase.tempoQueimar) {
            FicarQueimado();
        }
    }

    private void FicarPronto() {
        estado = EstadoCozimento.Cozido;
        if(processoAtual != null) {
            int nota = CalcularNota();
            processoAtual.ProcessIngredient(ingComponent, nota);
            AjustarColisor();
        }
    }

    private void FicarQueimado() {
        estado = EstadoCozimento.Queimado;
        if(processoAtual != null) {
            int nota = CalcularNota();
            processoAtual.ProcessIngredient(ingComponent, nota);
            AjustarColisor();
        }    
    }

    private int CalcularNota() {
        //Implementar calculo da nota do processo

        if(estado == EstadoCozimento.Cozido) return notaCozido;
        if(estado == EstadoCozimento.Queimado) return notaQueimado;
        return 0;
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
        if(dadosBase == null) return 0f;

        if(estado == EstadoCozimento.Cru) return Mathf.Clamp01(tempoCozido/ dadosBase.tempoCozinhar);
        if(estado == EstadoCozimento.Cozido) {
            float tempo = tempoCozido - dadosBase.tempoCozinhar;
            float duracaoQueimar = dadosBase.tempoQueimar - dadosBase.tempoCozinhar;
            return Mathf.Clamp01(tempo/duracaoQueimar);
        }

        return 1f;
    }
}
