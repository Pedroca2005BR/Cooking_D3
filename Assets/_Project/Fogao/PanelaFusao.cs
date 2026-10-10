using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IngredientFuserComponent))]
public class PanelaFusao : MonoBehaviour, IReceberIngrediente
{
    private IngredientFuserComponent fuser;
    private List<IngredienteFogao> ingredientes = new List<IngredienteFogao>();

    private void Awake() {
        fuser = GetComponent<IngredientFuserComponent>();
    }

    public bool AceitaIngrediente(GameObject objeto) {
        return objeto.GetComponent<IngredienteFogao>() != null;
    }

    public void ReceberIngredienteSolto(GameObject objeto) {
        IngredienteFogao novoIngrediente = objeto.GetComponent<IngredienteFogao>();
        if (novoIngrediente == null || ingredientes.Contains(novoIngrediente)) return;

        ingredientes.Add(novoIngrediente);
        
        // Todos os ingredientes ficam na mesma posição para criar a "pilha" visual
        novoIngrediente.transform.position = this.transform.position;

        if (ingredientes.Count > 1) {
            // Desliga apenas o colisor para evitar bugs físicos e sobreposição de triggers
            BoxCollider2D col = novoIngrediente.GetComponent<BoxCollider2D>();
            if (col != null) col.enabled = false;

            IngredientComponent[] paraFundir = new IngredientComponent[ingredientes.Count];
            for (int i = 0; i < ingredientes.Count; i++) {
                paraFundir[i] = ingredientes[i].ingComponent;
            }

            if (fuser.TryFusing(paraFundir, out GameObject objetoFundido)) {
                
                ingredientes.Clear(); 

                IngredienteFogao ingFundido = objetoFundido.GetComponent<IngredienteFogao>();
                if (ingFundido != null) {
                    ingredientes.Add(ingFundido);
                    ingFundido.transform.position = this.transform.position; 
                }

                DropSystem dropNovo = objetoFundido.GetComponent<DropSystem>();
                if (dropNovo != null) {
                    dropNovo.DefinirBancadaAtual(this, this.transform);
                }
            }
        }
    }

    public void RemoverIngrediente(GameObject objeto) {
        IngredienteFogao ingToRemove = objeto.GetComponent<IngredienteFogao>();
        if (ingToRemove != null && ingredientes.Contains(ingToRemove)) {
            
            ingredientes.Remove(ingToRemove);
            
            // Garante que o colisor do próximo ingrediente no topo da lista seja reativado
            // caso o jogador consiga remover a base
            if (ingredientes.Count > 0) {
                BoxCollider2D col = ingredientes[0].GetComponent<BoxCollider2D>();
                if (col != null) col.enabled = true;
            }
        }
    }
}