

using UnityEngine;

public class Outline2D : MonoBehaviour
{


    private SpriteRenderer spriteRenderer;
    private string parametroEspessura = "_Thickness";


    // O valor da espessura quando o contorno está ligado
    [SerializeField] private float espessuraContorno = 1f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void AtivarContorno(bool ativar)
    {
        if (spriteRenderer != null)
        {
            // Define o valor matemático: se ativar for true, usa a espessura; se false, usa 0
            float valor = ativar ? espessuraContorno : 0f;

            // Envia o comando para o Material atualizar o visual na tela
            spriteRenderer.material.SetFloat(parametroEspessura, valor);
        }
    }



}