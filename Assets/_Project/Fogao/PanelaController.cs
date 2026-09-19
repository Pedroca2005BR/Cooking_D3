using System;
using UnityEngine;
using UnityEngine.Events;

public class PanelaController : MonoBehaviour
{
    [Header("Config")]
    public string tagBoca = "BocaFogao"; //tag para comparar

    public event Action<float> OnProcesso; //evento para atualizar a UI

    public UnityEvent IniciarCozimento;
    public UnityEvent PausarCozimento;

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

    public void ReceberIngrediente(IngredienteFogao novoIngrediente) {
        ingrediente = novoIngrediente.GetComponent<IngredienteFogao>();
        Verificar();
    }

    private void Verificar() {
        bool podeCozinhar = (noFogo && ingrediente != null);

        if(podeCozinhar && !estaCozinhando) {
            estaCozinhando = true;
            IniciarCozimento?.Invoke();
        }else if(!podeCozinhar && estaCozinhando) {
            estaCozinhando = false;
            PausarCozimento?.Invoke();
        }
    }
}