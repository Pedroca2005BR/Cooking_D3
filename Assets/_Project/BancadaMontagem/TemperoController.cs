using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(FoodProcessorComponent))]
public class TemperoController: MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [Header("Minigame tempero")]
    [Tooltip("Quanto o jogador precisa chacoalhar")]
    [SerializeField] private float meta = 1500f; //aumentado para dar tempo de ver o efeito
    [SerializeField] private ParticleSystem particulas;

    [Header("Física (Arrastar)")]
    [SerializeField] private bool usarSuavizacao = true;
    [SerializeField] private float suavizacao = 0.05f;

    private FoodProcessorComponent processador;
    private Camera cam; //camera principal cacheada para nao pesar

    private float agitacao = 0f;
    private Vector2 ultimaPosicaoMouse; //variavel para calculo de quanto mouse se moveu

    private Vector3 posicaoOrigem; //posicao que estava
    private bool estaSegurando = false;
    private Vector3 velocidadeAtual = Vector3.zero;

    private IngredientComponent ingredienteTemperado; //ingrediente que esta sendo temperado
    
    private ParticleSystem.EmissionModule emissao; //modulo para efeito suave de torneira

    private void Awake() {
        processador = GetComponent<FoodProcessorComponent>();
        cam = Camera.main;
        
        //prepara a torneira de particulas
        if(particulas != null) {
            emissao = particulas.emission;
            emissao.enabled = false;
            if(!particulas.isPlaying) particulas.Play(); //deixa rodando no fundo sem emitir
        }
    }

    private void Start() {
        posicaoOrigem = transform.position;
    }

    public void OnPointerDown(PointerEventData eventData) {
        Debug.Log("Clicou e segurou o tempero!");
        
        estaSegurando = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        Debug.Log("Soltou o tempero!");
        
        // Volta para a posição original
        transform.position = posicaoOrigem;
        estaSegurando = false;

        // Limpa o processo
        ingredienteTemperado = null;
        if(particulas != null) emissao.enabled = false;
    }

    private void Update() {
        if (estaSegurando) {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector3 worldPoint = cam.ScreenToWorldPoint(mousePos);
            Vector3 dragTargetPosition = new Vector3(worldPoint.x, worldPoint.y, 0);

            if (usarSuavizacao) {
                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    dragTargetPosition,
                    ref velocidadeAtual,
                    suavizacao
                );
            } else {
                transform.position = dragTargetPosition;
            }

            //chacoalha direto nos frames da tela e nao da fisica
            if(ingredienteTemperado != null) {
                CalcularAgitacaoMouse(mousePos);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D outro) {
        if(!estaSegurando) return;

        IngredientComponent comida = outro.GetComponent<IngredientComponent>();

        //verifica se a comida aceita o processo e atualiza o estado
        if(comida != null && ValidadorDeReceita.AceitaProcesso(comida, processador.ProcessoAtual)) {
            ingredienteTemperado = comida;
            agitacao = 0f;
            ultimaPosicaoMouse = Mouse.current.position.ReadValue();
        }
    }

    private void OnTriggerExit2D(Collider2D outro) {
        //se o ingrediente existe e nao mudou
        if (ingredienteTemperado != null && outro.gameObject == ingredienteTemperado.gameObject) {
            ingredienteTemperado = null;
            if(particulas != null) emissao.enabled = false; //fecha a torneira ao sair de cima
        }
    }

    private void CalcularAgitacaoMouse(Vector2 posicaoAtualMouse) {
        //calcula distancia entre a posicao atual e a salva, depois salva a posicao atual para calcular o vai e vem
        float distanciaMovida = Vector2.Distance(posicaoAtualMouse, ultimaPosicaoMouse);
        ultimaPosicaoMouse = posicaoAtualMouse;

        //tolerancia menor para nao picotar se for devagar
        if(distanciaMovida > 0.5f) {
            agitacao += distanciaMovida;
            if(particulas != null) emissao.enabled = true; //abre a torneira
        }else {
            if(particulas != null) emissao.enabled = false; //fecha a torneira
        }

        if(agitacao >= meta) {
            FinalizarTempero();
        }
    }

    private void FinalizarTempero() {
        if(particulas != null) emissao.enabled = false; //fecha a torneira ao terminar

        IngredientComponent alvo = ingredienteTemperado;
        ingredienteTemperado = null; 
        agitacao = 0f;

        if(alvo != null) processador.ProcessIngredient(alvo, 100);


        Debug.Log("Comida temperada!");
    }

    //Para cada tipo de tempero precisara de um processo diferente
}