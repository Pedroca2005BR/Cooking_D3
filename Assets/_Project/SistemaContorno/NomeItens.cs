using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class NomeItens : MonoBehaviour 
{
    public static NomeItens Instancia { get; private set; }

    private TextMeshProUGUI texto; //componente de texto da UI
    private RectTransform rect; //rect para calcular posicao do texto
    private Camera cam; //camera para calcular os limites

    private void Awake() {
        if (Instancia == null) {
            Instancia = this;
            texto = GetComponent<TextMeshProUGUI>();
            rect = GetComponent<RectTransform>();
            cam = Camera.main;
            gameObject.SetActive(false); 
        } else {
            Destroy(gameObject);
        }
    }

    //metodo usado pelo ContornoSystem para ativar o Texto(nome dos itens)
    public void MostrarNome(string nome, SpriteRenderer spriteRender) { //recebe o nome q deve ser imprimido e o sprite
        texto.text = nome; //atualiza o componente com o nome
        texto.ForceMeshUpdate(); //forca a mudanca do conteudo
        
        Bounds limites = spriteRender.bounds; //calcula os limites do sprite
        
        //calcula as bordas do sprite em relação a cena
        Vector3 bordaDireitaMundo = limites.center + new Vector3(limites.extents.x, 0, 0);
        Vector3 bordaTopoMundo = limites.center + new Vector3(0, limites.extents.y, 0);

        // convertem as bordas para coordenadas de pixels na tela
        Vector2 bordaDireitaTela = cam.WorldToScreenPoint(bordaDireitaMundo);
        Vector2 bordaTopoTela = cam.WorldToScreenPoint(bordaTopoMundo);

        float larguraTexto = texto.preferredWidth; //calcula a largura do texto

        Vector2 posicaoFinal = new Vector2(bordaDireitaTela.x + 10f, bordaDireitaTela.y); //posiciona na direita do sprite

        // se a palavra for passar da borda do monitor
        if (posicaoFinal.x + larguraTexto > Screen.width) { 
            // vai para o meio(limites.center) do topo(bordaTopoTela) do sprite
            posicaoFinal = new Vector2(cam.WorldToScreenPoint(limites.center).x - (larguraTexto / 2f), bordaTopoTela.y + 10f);
        }

        //atualiza a posicao do rectTransform e ativa o TextMesh
        rect.position = posicaoFinal;
        gameObject.SetActive(true);
    }

    //metodo usado pelo ContornoSystem para desativar o Texto(nome dos itens)
    public void EsconderNome() {
        gameObject.SetActive(false);
    }
}