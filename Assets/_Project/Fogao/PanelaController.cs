using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(FoodProcessorComponent))]
public class PanelaController : MonoBehaviour, IReceberIngrediente
{
    public event Action<float> OnProcesso; //evento para atualizar a UI
    public event Action OnIniciarUI;
    public event Action OnPausarUI;
    public event Action<bool, float> OnConfigurarUI;

    [Header("Configurações da UI")]
    [SerializeField] private bool aparecerEmCima = true;
    [SerializeField] private float distanciaY = 1.5f;


    private FoodProcessorComponent processador; //referencia para o componente de processos
    private IngredienteFogao ingrediente;
    [SerializeField] private BaseIngredientData gororoba;

    private void Awake() {
        processador = GetComponent<FoodProcessorComponent>();
    }

    private void Start()
    {
        OnConfigurarUI?.Invoke(aparecerEmCima, distanciaY);
    }
    
    private void Update() { 
        if(ingrediente != null) { 
            ingrediente.RecebeCalor(Time.deltaTime); //logica de contagem de tempo em processo

            float progresso = ingrediente.CalculaPorcentagem(); //porcentagem conclusao do processo

            OnProcesso?.Invoke(progresso); //avisa a UI do progresso
        }
    }

    public bool AceitaIngrediente(GameObject objeto)
    {
        return objeto.GetComponent<IngredienteFogao>() != null; //panela retorna qualquer ingrediente
    }

    // Onde a física e a fusão de fato ocorrem
    public void ReceberIngredienteSolto(GameObject objeto)
    {
        IngredienteFogao novoIngrediente = objeto.GetComponent<IngredienteFogao>();
        if(novoIngrediente == null || novoIngrediente == ingrediente) return;

        if(ingrediente == null) //se a panela estiver vazia
        {
            ingrediente = novoIngrediente;
            ingrediente.transform.position = this.transform.position;
            ingrediente.DefinirProcessador(processador);

            OnIniciarUI?.Invoke();
        }else //caso a panela não esteja vazia
        {
            Debug.Log("Mais de um ingrediente na panela de processos... transformando em gororoba!!!!");

            Destroy(objeto); //destroi o novo objeto q tentou entrar

            if(gororoba != null)
            {
                ingrediente.ingComponent.Setup(gororoba);

                ingrediente.AjustarColisor();
            }

            //gororoba vira gororoba
            ingrediente.DefinirProcessador(processador);
        }
    }

    public void RemoverIngrediente(GameObject objeto) {
        if (ingrediente != null && objeto == ingrediente.gameObject) {
            ingrediente.DefinirProcessador(null);
            ingrediente = null;
            
            OnPausarUI?.Invoke();
        }
    }
}