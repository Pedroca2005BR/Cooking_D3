using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(FoodProcessorComponent))]
public class PanelaController : MonoBehaviour, IReceberIngrediente
{
    public event Action<float> OnProcesso; //evento para atualizar a UI
    public event Action OnIniciarUI;
    public event Action OnPausarUI;

    [Header("Estado atual")]
    [SerializeField] private bool noFogo = false; //variavel de estado para controlar a logica
    [SerializeField] private IngredienteFogao ingrediente; //ingrediente interagindo com a panela

    private bool estaCozinhando = false;
    private FoodProcessorComponent processador; //referencia para o componente de processos
    private IngredientFuserComponent fuser; //referencia para o componente de fusao

    private void Awake() {
        processador = GetComponent<FoodProcessorComponent>();
        fuser = GetComponent<IngredientFuserComponent>();
    }
    
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
        IngredienteFogao novoIngrediente = objeto.GetComponent<IngredienteFogao>();
        if (novoIngrediente == null) return false;

        // 1. Se a panela estiver vazia, aceita o ingrediente
        if (ingrediente == null) {
            return ValidadorDeReceita.AceitaProcesso(novoIngrediente.ingComponent, processador.ProcessoAtual);
        }

        // 2. Se já tiver um ingrediente, pergunta ao Fuser se os dois podem ser fundidos
        if (fuser != null && novoIngrediente != ingrediente) {
            return ValidadorDeReceita.AceitaFusao(fuser, ingrediente.ingComponent, novoIngrediente.ingComponent);
        }

        return false;
    }

    public void ReceberIngredienteSolto(GameObject objeto) {
        IngredienteFogao novoIngrediente = objeto.GetComponent<IngredienteFogao>();
        if (novoIngrediente == null) return;

        // Caso 1: Panela vazia -> Recebe o 1º ingrediente
        if (ingrediente == null) {
            ingrediente = novoIngrediente;
            ingrediente.DefinirProcessador(processador); //passa o processador
            Verificar();
            Debug.Log("Panela recebeu ingrediente");
        }
        // Caso 2: Panela já tem ingrediente -> Funde os dois
        else if (fuser != null && novoIngrediente != ingrediente) {
            IngredientComponent[] paraFundir = new IngredientComponent[] {
                ingrediente.ingComponent,
                novoIngrediente.ingComponent
            };

            if (fuser.TryFusing(paraFundir, out GameObject objetoFundido)) {
                ingrediente = objetoFundido.GetComponent<IngredienteFogao>();
                if (ingrediente != null) {
                    ingrediente.DefinirProcessador(processador);
                }

                // Avisa o DropSystem do novo objeto fundido que ele está dentro desta panela!
                DropSystem dropNovo = objetoFundido.GetComponent<DropSystem>();
                if (dropNovo != null) {
                    dropNovo.DefinirBancadaAtual(this, this.transform);
                }

                Verificar();
                Debug.Log("Ingredientes fundidos na panela!");
            }
        }
    }

    public void RemoverIngrediente(GameObject objeto) {
        if(ingrediente != null && objeto == ingrediente.gameObject) {
            ingrediente.DefinirProcessador(null); // Limpa o processador
            ingrediente = null;
            Verificar();
            Debug.Log("Ingrediente removido da panela");
        }
    }
}