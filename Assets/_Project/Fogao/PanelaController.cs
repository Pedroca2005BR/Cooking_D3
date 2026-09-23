using System;
using UnityEngine;
using UnityEngine.Events;

public class PanelaController : MonoBehaviour, IReceberIngrediente
{
    public event Action<float> OnProcesso; //evento para atualizar a UI
    public event Action OnIniciarUI;
    public event Action OnPausarUI;

    [Header("Estado atual")]
    [SerializeField] private bool noFogo = false; //variavel de estado para controlar a logica
    [SerializeField] private IngredienteFogao ingrediente; //ingrediente interagindo com a panela

    private bool estaCozinhando = false;

    private void Update() { 
        if(noFogo && ingrediente != null) { 
            ingrediente.RecebeCalor(Time.deltaTime); //logica de contagem de tempo em processo

            float progresso = ingrediente.CalculaPorcentagem(); //porcentagem conclusao do processo

            OnProcesso?.Invoke(progresso); //avisa a UI do progresso
        }
    }

    public void LigarFogo() {
        noFogo = true;
        Verificar();
        Debug.Log("Panela esta no fogo!");
    }

    public void DesligarFogo() {
        noFogo = false;
        Verificar();
        Debug.Log("Panela saiu do fogo!");
    }

    private void Verificar() {
        bool podeCozinhar = (noFogo && ingrediente != null);

        if(podeCozinhar && !estaCozinhando) {
            estaCozinhando = true;
            OnIniciarUI?.Invoke();
        }else if(!podeCozinhar && estaCozinhando) {
            estaCozinhando = false;
            OnPausarUI?.Invoke();
        }
    }

    public bool AceitaIngrediente(GameObject objeto) {
        //so aceita se for um ingrediente e se a panela estiver vazia
        return objeto.GetComponent<IngredienteFogao>() != null && ingrediente == null;
    }

    public void ReceberIngredienteSolto(GameObject objeto) {
        IngredienteFogao novoIngrediente = objeto.GetComponent<IngredienteFogao>();
        if(novoIngrediente != null) {
            ingrediente = novoIngrediente;
            Verificar();
            Debug.Log("Panela recebeu ingrediente");
        }
    }

    public void RemoverIngrediente(GameObject objeto) {
        if(ingrediente != null && objeto == ingrediente.gameObject) {
            ingrediente = null;
            Verificar();
            Debug.Log("Ingrediente removido da panela");
        }
    }
}