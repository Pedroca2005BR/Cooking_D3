using UnityEngine;

[RequireComponent(typeof(FoodProcessorComponent))]
public class RecipienteController : MonoBehaviour, IReceberIngrediente
{
    private FoodProcessorComponent processador;
    private IngredienteFogao ingredienteAtual;

    private void Awake() {
        processador = GetComponent<FoodProcessorComponent>();
    }

    public bool AceitaIngrediente(GameObject objeto) {
        IngredienteFogao novoIngrediente = objeto.GetComponent<IngredienteFogao>();
        
        // 1. Só aceita se for um ingrediente válido e se o recipiente estiver vazio
        if (novoIngrediente == null || ingredienteAtual != null) return false;

        // 2. Pergunta ao validador se o ingrediente tem uma conversão para Farinhar/Empanar
        // Isso elimina completamente a necessidade de instanciar "GameObjects de sacrifício"
        return ValidadorDeReceita.AceitaProcesso(novoIngrediente.ingComponent, processador.ProcessoAtual);
    }

    public void ReceberIngredienteSolto(GameObject objeto) {
        IngredienteFogao novoIngrediente = objeto.GetComponent<IngredienteFogao>();
        if (novoIngrediente == null) return;

        ingredienteAtual = novoIngrediente;
        objeto.transform.position = this.transform.position; // Centraliza visualmente na tigela

        // 3. Descobre qual é a "nota" exigida pela receita para esse processo
        int notaAlvo = ingredienteAtual.ingComponent.data.GetScoreNeeded(processador.ProcessoAtual);

        // 4. Como empanar/farinhar é uma ação instantânea de mergulhar o item,
        // aplicamos o processo enviando a nota exata que garante o sucesso imediato.
        if (processador != null) {
            processador.ProcessIngredient(ingredienteAtual.ingComponent, notaAlvo);
            ingredienteAtual.AjustarColisor();
        }

        Debug.Log($"Ingrediente processado instantaneamente no recipiente: {processador.ProcessoAtual}");
    }

    public void RemoverIngrediente(GameObject objeto) {
        // Libera o recipiente para ser usado novamente quando o jogador retirar o item
        if (ingredienteAtual != null && objeto == ingredienteAtual.gameObject) {
            ingredienteAtual = null;
        }
    }
}