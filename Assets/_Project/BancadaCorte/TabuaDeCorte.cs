using UnityEngine;

[RequireComponent(typeof(FoodProcessorComponent))]
public class TabuaDeCorte : MonoBehaviour, IReceberIngrediente {
    private IngredienteFogao ingredienteAtual;
    private FoodProcessorComponent processador; // Referência para o componente do De Paula

    [Header("Notas Provisorias")]
    [SerializeField] private int notaCorte = 100;

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

        // Retorna verdadeiro se existe ingrediente a ser recebido, se ele tem dados, se ele não foi cortado e se a tábua estiver vazia
        return (ingrediente != null && ingrediente.dadosBase != null && !ingrediente.jaFoiCortado && ingredienteAtual == null);
    }

    public void ReceberIngredienteSolto(GameObject objeto) {
        ingredienteAtual = objeto.GetComponent<IngredienteFogao>();

        if(ingredienteAtual != null) {
            objeto.transform.position = this.transform.position; // Centraliza a comida na tábua

            EventBusCorte.OnIniciarMiniGame?.Invoke(ingredienteAtual.dadosBase); // Avisa que o minigame começou

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
            ingredienteAtual.jaFoiCortado = true;

            if (processador != null) {
                int notaFinal = CalcularNotaCorte();
                processador.ProcessIngredient(ingredienteAtual.ingComponent, notaFinal);
                ingredienteAtual.AjustarColisor();
            }

            Debug.Log($"{ingredienteAtual.gameObject.name} foi cortado!");
        }
    }

    private int CalcularNotaCorte() {
        //Futuramente, substituir pelo cálculo real baseado nos acertos/erros do minigame de corte
        return notaCorte;
    }
}