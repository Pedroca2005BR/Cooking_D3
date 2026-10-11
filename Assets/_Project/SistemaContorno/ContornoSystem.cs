using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Outline2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ContornoSystem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler 
{
    private Outline2D outliner;
    private SpriteRenderer spriteRender;
    private IngredientComponent ingrediente;
    
    private string nomeExibicao;
    private bool estaArrastando = false;

    private void Awake() {
        outliner = GetComponent<Outline2D>();
        spriteRender = GetComponent<SpriteRenderer>();
        ingrediente = GetComponent<IngredientComponent>();
        DescobrirNomeAutomaticamente();
    }

    private void OnEnable() {
        if (ingrediente != null) {
            // Subscreve ao evento para escutar as injeções de dados da geladeira
            ingrediente.OnSetup += ForcarAtualizacaoDeNome;
        }
    }

    private void OnDisable() {
        if (ingrediente != null) {
            // Remove a subscrição para evitar fugas de memória (memory leaks)
            ingrediente.OnSetup -= ForcarAtualizacaoDeNome;
        }
    }

    // Método disparado pelo evento OnSetup
    private void ForcarAtualizacaoDeNome() {
        // Limpa a variável para passar na verificação de segurança
        nomeExibicao = string.Empty; 
        DescobrirNomeAutomaticamente();
    }

    //metodo para descobrir nome do ingrediente
    private void DescobrirNomeAutomaticamente() {
        if (!string.IsNullOrEmpty(nomeExibicao)) return;

        //se for um ingrediente
        IngredientComponent ingrediente = GetComponent<IngredientComponent>();
        if (ingrediente != null && ingrediente.data != null) {
            nomeExibicao = ingrediente.data.baseName; 
            return;
        }

        //se for uma ferramenta, processador
        FoodProcessorComponent ferramenta = GetComponent<FoodProcessorComponent>();
        if (ferramenta != null) {
            nomeExibicao = ferramenta.ProcessoAtual.ToString();
            return;
        }

        nomeExibicao = gameObject.name;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.used) return; 
        
        outliner.AtivarContorno(true); //ao encostar mouse no item, ativa o contorno
        
        // Passa o SpriteRenderer inteiro para a UI calcular os limites
        if (!estaArrastando && !string.IsNullOrEmpty(nomeExibicao) && NomeItens.Instancia != null) {
            NomeItens.Instancia.MostrarNome(nomeExibicao, spriteRender);
        }

        eventData.Use();
    }

    public void OnPointerExit(PointerEventData eventData) {
        //ponteiro saiu do item, caso esteja arrastando, desativa somente o nome
        //se nao esta arrastando, desativa o contorno
        if (!estaArrastando) {
            outliner.AtivarContorno(false);
        }

        //desativa o nome
        if (NomeItens.Instancia != null) NomeItens.Instancia.EsconderNome();
    }

    public void OnPointerDown(PointerEventData eventData) {
        estaArrastando = true;
        // desativa o nome imediatamente no clique
        if (NomeItens.Instancia != null) NomeItens.Instancia.EsconderNome();
    }

    public void OnPointerUp(PointerEventData eventData) {
        estaArrastando = false;
        
        // Verifica se soltou em cima dele mesmo
        if (eventData.pointerCurrentRaycast.gameObject == gameObject) {
            if (!string.IsNullOrEmpty(nomeExibicao) && NomeItens.Instancia != null) {
                NomeItens.Instancia.MostrarNome(nomeExibicao, spriteRender);
            }
        } else {
            outliner.AtivarContorno(false);
        }
    }
}