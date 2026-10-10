/*using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(FoodProcessorComponent))]
public class TemperoController: MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [Header("Minigame tempero")]
    [Tooltip("Quanto o jogador precisa chacoalhar")]
    [SerializeField] private float meta = 1500f; //aumentado para dar tempo de ver o efeito
    [SerializeField] private ParticleSystem particulas;

    [Header("Visual")]
    [Tooltip("Ângulo no eixo Z para o qual o saleiro vai girar ao achar um ingrediente válido")]
    [SerializeField] private float anguloInclinacao = 180f;

    [Header("Física (Arrastar)")]
    [SerializeField] private bool usarSuavizacao = true;
    [SerializeField] private float suavizacao = 0.05f;

    private FoodProcessorComponent processador;
    private Camera cam; //camera principal cacheada para nao pesar

    private float agitacao = 0f;
    private Vector3 ultimaPosicaoMouse; //variavel para calculo de quanto mouse se moveu no mundo

    private bool estaSegurando = false;
    private Vector3 velocidadeAtual = Vector3.zero;
    private Quaternion rotacaoOriginal; // Guarda a rotação inicial do objeto

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
        rotacaoOriginal = transform.rotation; // Salva como o saleiro estava no cenário
    }

    public void OnPointerDown(PointerEventData eventData) {
        Debug.Log("Clicou e segurou o tempero!");
        estaSegurando = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        Debug.Log("Soltou o tempero!");
        
        estaSegurando = false;
        
        // Volta o saleiro para a rotação normal caso seja solto
        transform.rotation = rotacaoOriginal;

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
                CalcularAgitacaoMouse();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D outro) {
        if(!estaSegurando) return;

        IngredientComponent comida = outro.GetComponent<IngredientComponent>();

        // Agora chama o AceitaProcessoEstrito, feito sob medida para ferramentas
        if(comida != null && ValidadorDeReceita.AceitaProcessoEstrito(comida, processador.ProcessoAtual)) {
            ingredienteTemperado = comida;
            agitacao = 0f;
            ultimaPosicaoMouse = transform.position;
            
            // Gira o saleiro porque encontrou um alvo válido!
            transform.rotation = Quaternion.Euler(0, 0, anguloInclinacao);
        }
    }

    private void OnTriggerExit2D(Collider2D outro) {
        //se o ingrediente existe e nao mudou
        if (ingredienteTemperado != null && outro.gameObject == ingredienteTemperado.gameObject) {
            ingredienteTemperado = null;
            if(particulas != null) emissao.enabled = false; //fecha a torneira ao sair de cima
            
            // Volta para a rotação normal ao sair de cima da comida
            transform.rotation = rotacaoOriginal;
        }
    }

    private void CalcularAgitacaoMouse() {
        //calcula distancia entre a posicao atual e a salva, depois salva a posicao atual para calcular o vai e vem
        float distanciaMovida = Vector3.Distance(transform.position, ultimaPosicaoMouse);
        ultimaPosicaoMouse = transform.position;

        //tolerancia menor para nao picotar se for devagar
        if(distanciaMovida > 0.05f) {
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
        
        // Volta para a rotação normal quando finaliza
        transform.rotation = rotacaoOriginal;

        if(alvo != null) processador.ProcessIngredient(alvo, 100);

        Debug.Log("Comida temperada!");
    }
}*/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(FoodProcessorComponent))]
public class TemperoController: MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [Header("Minigame tempero")]
    [Tooltip("Distância que o rato precisa percorrer (em unidades da Unity) para ativar o tempero")]
    [SerializeField] private float meta = 15f; 
    [Tooltip("Tempo em segundos que o tempero derrama as partículas antes de processar")]
    [SerializeField] private float tempoDerramando = 1f;
    [SerializeField] private ParticleSystem particulas;

    [Header("Visual")]
    [Tooltip("Ângulo no eixo Z para o qual o saleiro vai girar ao achar um ingrediente válido")]
    [SerializeField] private float anguloInclinacao = 180f;

    [Header("Física (Arrastar)")]
    [SerializeField] private bool usarSuavizacao = true;
    [SerializeField] private float suavizacao = 0.05f;

    private FoodProcessorComponent processador;
    private Camera cam; 

    private float agitacao = 0f;
    private Vector3 ultimaPosicaoMouseMundo; // Agora mede o rato real, ignorando o atraso da suavização

    private bool estaSegurando = false;
    private bool estaDerramando = false;
    private float timerDerramando = 0f;
    
    private Vector3 velocidadeAtual = Vector3.zero;
    private Quaternion rotacaoOriginal; 

    private IngredientComponent ingredienteTemperado; 
    private ParticleSystem.EmissionModule emissao; 

    private void Awake() {
        processador = GetComponent<FoodProcessorComponent>();
        cam = Camera.main;
        
        if(particulas != null) {
            emissao = particulas.emission;
            emissao.enabled = false;
            if(!particulas.isPlaying) particulas.Play(); 
        }
    }

    private void Start() {
        rotacaoOriginal = transform.rotation; 
    }

    public void OnPointerDown(PointerEventData eventData) {
        estaSegurando = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        estaSegurando = false;
        ResetarEstado(); // Cancela tudo se o jogador soltar a meio do processo
    }

    private void Update() {
        if (estaSegurando) {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector3 worldPoint = cam.ScreenToWorldPoint(mousePos);
            worldPoint.z = 0; // Zera a profundidade para trabalhar apenas em 2D

            // 1. Lógica de movimento visual
            if (usarSuavizacao) {
                transform.position = Vector3.SmoothDamp(transform.position, worldPoint, ref velocidadeAtual, suavizacao);
            } else {
                transform.position = worldPoint;
            }

            // 2. Lógica do minigame
            if(ingredienteTemperado != null) {
                if (!estaDerramando) {
                    CalcularAgitacao(worldPoint);
                } else {
                    ExecutarDerramamento();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D outro) {
        if(!estaSegurando || estaDerramando) return;

        IngredientComponent comida = outro.GetComponent<IngredientComponent>();

        if(comida != null && ValidadorDeReceita.AceitaProcessoEstrito(comida, processador.ProcessoAtual)) {
            ingredienteTemperado = comida;
            
            // Grava a posição exata de entrada para iniciar o cálculo limpo
            Vector2 mousePos = Mouse.current.position.ReadValue();
            ultimaPosicaoMouseMundo = cam.ScreenToWorldPoint(mousePos);
            ultimaPosicaoMouseMundo.z = 0;
            
            transform.rotation = Quaternion.Euler(0, 0, anguloInclinacao);
        }
    }

    private void OnTriggerExit2D(Collider2D outro) {
        if (ingredienteTemperado != null && outro.gameObject == ingredienteTemperado.gameObject) {
            ResetarEstado(); 
        }
    }

    private void CalcularAgitacao(Vector3 worldMousePos) {
        float distanciaMovida = Vector3.Distance(worldMousePos, ultimaPosicaoMouseMundo);
        ultimaPosicaoMouseMundo = worldMousePos;

        if(distanciaMovida > 0.05f) {
            agitacao += distanciaMovida;
        }

        if(agitacao >= meta) {
            estaDerramando = true;
            timerDerramando = 0f;
            if(particulas != null) emissao.enabled = true; // Inicia o feedback visual
        }
    }

    private void ExecutarDerramamento() {
        timerDerramando += Time.deltaTime;
        
        if (timerDerramando >= tempoDerramando) {
            FinalizarTempero();
        }
    }

    private void FinalizarTempero() {
        IngredientComponent alvo = ingredienteTemperado;
        ResetarEstado(); // Limpa estado antes de processar para evitar bugs de loop

        if(alvo != null) processador.ProcessIngredient(alvo, 100);
    }

    private void ResetarEstado() {
        ingredienteTemperado = null;
        estaDerramando = false;
        agitacao = 0f;
        timerDerramando = 0f;
        
        if(particulas != null) emissao.enabled = false;
        transform.rotation = rotacaoOriginal;
    }
}