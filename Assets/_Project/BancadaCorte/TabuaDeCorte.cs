using UnityEngine;

public class TabuaDeCorte : MonoBehaviour, IReceberIngrediente {
    private IngredienteFogao ingredienteAtual;

    private void OnEnable() {
        EventBusCorte.OnFimMiniGame += FinalizarCorte;        
    }

    private void OnDisable() {
        EventBusCorte.OnFimMiniGame -= FinalizarCorte;
    }

    public bool AceitaIngrediente(GameObject objeto) {
        IngredienteFogao ingrediente = objeto.GetComponent<IngredienteFogao>();

        //returna verdadeiro se existe ingrediente a ser recebido, se ele tem dados, se ele n foi cortado e se a tabua estiver vazia
        return (ingrediente != null && ingrediente.dadosBase != null && !ingrediente.jaFoiCortado && ingredienteAtual == null);
    }

    public void ReceberIngredienteSolto(GameObject objeto) {
        ingredienteAtual = objeto.GetComponent<IngredienteFogao>();

        if(ingredienteAtual != null) {
            objeto.transform.position = this.transform.position; //centraliza a comida na tabua

            EventBusCorte.OnIniciarMiniGame?.Invoke(ingredienteAtual.dadosBase); //avisa q o minigame comecou

            Debug.Log($"Tabua recebeu {objeto.name}");
        }
    }

    public void RemoverIngrediente(GameObject objeto) {
        //se o ingrediente q esta saindo existe e eh o mesmo q estava sendo cortado antes
        if(ingredienteAtual != null && objeto == ingredienteAtual.gameObject){
            ingredienteAtual = null;

            //lanca o evento de quando o mini game eh terminado de forma brusca
            EventBusCorte.OnTerminarMiniGame?.Invoke();
            Debug.Log("Ingrediente removido da tabua!!");
        }
    }

    private void FinalizarCorte() { //metodo para quando o mini game acabar
        if(ingredienteAtual != null) {
            ingredienteAtual.jaFoiCortado = true;
            Debug.Log("${ingredienteAtual.gameObject.name} foi cortado!");
        }
    }
}