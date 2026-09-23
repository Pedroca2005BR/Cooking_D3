using UnityEngine;

public class IngredienteFogao : MonoBehaviour
{
    public enum EstadoCozimento { Cru, Cozido, Queimado}

    public IngredienteDataSO dadosBase;

    [Header("Estado Atual")]
    [SerializeField] private float tempoCozido = 0f;
    private EstadoCozimento estado = EstadoCozimento.Cru;
    public bool jaFoiCortado = false;

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

    private void AjustarColisor()
    {
        //ajusta o tamanho do colizor de acordo com o sprite
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if(col != null && spriteIng.sprite != null) 
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
