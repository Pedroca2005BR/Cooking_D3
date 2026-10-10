using UnityEngine;

[RequireComponent(typeof(FoodProcessorComponent))]
public class TabuaDeCorte : MonoBehaviour, IReceberIngrediente {
    private IngredienteFogao ingredienteAtual;
    private FoodProcessorComponent processador; 

    [Header("Configurações do Minigame (Base)")]
    [SerializeField] private float velocidadeCursorBase = 2f;
    [SerializeField] private float tamanhoZonaBase = 0.3f;
    [SerializeField] private float tempoMiniGame = 5f;

    private int cortesAcertados = 0;

    private void Awake() {
        processador = GetComponent<FoodProcessorComponent>();
    }

    private void OnEnable() {
        EventBusCorte.OnFimMiniGame += FinalizarCorte;
    }

    private void OnDisable() {
        EventBusCorte.OnFimMiniGame -= FinalizarCorte;
    }

    public bool AceitaIngrediente(GameObject objeto) {
        IngredienteFogao ingrediente = objeto.GetComponent<IngredienteFogao>();

        // Retorna verdadeiro se existe ingrediente a ser recebido, se ele não foi cortado e se a tábua estiver vazia
        if (ingrediente != null && ingredienteAtual == null) {
            // Pergunta ao validador se este ingrediente aceita ser cortado
            return ValidadorDeReceita.AceitaProcesso(ingrediente.ingComponent, processador.ProcessoAtual);
        }

        return false;
    }

    public void ReceberIngredienteSolto(GameObject objeto) {
        ingredienteAtual = objeto.GetComponent<IngredienteFogao>();

        if(ingredienteAtual != null) {
            objeto.transform.position = this.transform.position; // Centraliza a comida na tábua

            int cortesAlvo = ingredienteAtual.ingComponent.data.GetScoreNeeded(processador.ProcessoAtual);

            if (cortesAlvo <= 0) cortesAlvo = 3; //caso o jogador n tenha processo(gororoba)

            cortesAcertados = cortesAlvo; //memoriza o numero de cortes para a pontuacao

            EventBusCorte.OnIniciarMiniGame?.Invoke(cortesAlvo, velocidadeCursorBase, tamanhoZonaBase, tempoMiniGame); // Avisa que o minigame começou

            Debug.Log($"Tabua recebeu {objeto.name}");
        }
    }

    public void RemoverIngrediente(GameObject objeto) {
        // Se o ingrediente que está saindo existe e é o mesmo que estava sendo cortado antes
        if(ingredienteAtual != null && objeto == ingredienteAtual.gameObject){
            ingredienteAtual = null;

            // Lança o evento de quando o minigame é terminado de forma brusca
            EventBusCorte.OnTerminarMiniGame?.Invoke();
            Debug.Log("Ingrediente removido da tabua!!");
        }
    }

    private void FinalizarCorte() { // Método para quando o minigame acabar
        if(ingredienteAtual != null) {
            if (processador != null) {
                processador.ProcessIngredient(ingredienteAtual.ingComponent, cortesAcertados);
                ingredienteAtual.AjustarColisor();
            }

            Debug.Log($"{ingredienteAtual.gameObject.name} foi cortado!");
        }
    }
}