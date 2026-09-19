using UnityEngine;

public class IngredienteFogao : MonoBehaviour
{
    public enum EstadoCozimento { Cru, Cozido, Queimado}

    public IngredienteDataSO dadosBase;

    [Header("Estado Atual")]
    [SerializeField] private float tempoCozido = 0f;
    private EstadoCozimento estado = EstadoCozimento.Cru;

    private SpriteRenderer spriteIng;

    private void Awake() {
        spriteIng = GetComponent<SpriteRenderer>();

        if(dadosBase != null && dadosBase.spriteCru != null) {
            spriteIng.sprite = dadosBase.spriteCru;
        }
    }

    public void RecebeCalor(float tempoNoFogo) {
        if(estado == EstadoCozimento.Queimado) return;

        tempoCozido += tempoNoFogo;

        if(estado == EstadoCozimento.Cru && tempoCozido >= dadosBase.tempoCozinhar) {
            FicarPronto();
        }else if(estado == EstadoCozimento.Cozido && tempoCozido >= dadosBase.tempoQueimar) {
            FicarQueimado();
        }
    }

    private void FicarPronto() {
        estado = EstadoCozimento.Cozido;
        spriteIng.sprite = dadosBase.spriteCozido;
    }

    private void FicarQueimado() {
        estado = EstadoCozimento.Queimado;
        spriteIng.sprite = dadosBase.spriteQueimado;
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

    public void SoltarIngrediente(GameObject alvo)
    {
        PanelaController panela = alvo.GetComponent<PanelaController>();
        
        if (panela != null)
        {
            panela.ReceberIngrediente(this); // Entrega o bife para a panela
            GetComponent<Collider2D>().enabled = false; // Desliga o colisor como você queria
            Debug.Log("Ingrediente entregue a panela!");
        }
    }
}
