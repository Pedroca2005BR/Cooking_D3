using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections.Generic;

public class DropSystem : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [Header("Regras")]
    public bool imantarNoCentro = true;
    public bool tornarFilhoDoAlvo = false;

    //variaveis para voltar para o lugar de origem
    private Vector3 posicaoOrigem;
    private Transform parenteOrigem;
    private IReceberIngrediente bancadaDeOrigem;

    //lista para guardar todos os objetos que o trigger esta detectando
    private List<GameObject> alvosPossiveis = new List<GameObject>();
    
    private IReceberIngrediente bancadaAtual; //guarda a referencia de onde o objeto esta

    // Permite vincular o objeto a uma bancada quando ele nasce por fusao (Instantiate)
    public void DefinirBancadaAtual(IReceberIngrediente novaBancada, Transform transformAlvo)
    {
        bancadaAtual = novaBancada;

        if (tornarFilhoDoAlvo && transformAlvo != null)
        {
            transform.SetParent(transformAlvo);
        }

        if (imantarNoCentro && transformAlvo != null)
        {
            if (tornarFilhoDoAlvo) transform.localPosition = Vector3.zero;
            else transform.position = transformAlvo.position;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //salva o estado inicial
        posicaoOrigem = transform.position;
        parenteOrigem = transform.parent;
        bancadaDeOrigem = bancadaAtual;

        //Se o objeto estiver em uma bancada e o jogador clicou para pega-lo,
        //avisa a bancada para remover o ingrediente
        if(bancadaAtual != null) {
            bancadaAtual.RemoverIngrediente(this.gameObject);
            bancadaAtual = null;
        }
        
        if (tornarFilhoDoAlvo)
        {
            transform.SetParent(null);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        bool lugarValido = false;

        Debug.Log($"Os alvos que o sistema encontrou foram:");
        
        foreach(GameObject alvo in alvosPossiveis) {
            Debug.Log($"{(alvo != null ? alvo.name : "NENHUM")}");
            if (alvo == null) continue;
            
            IReceberIngrediente recebedor = alvo.GetComponent<IReceberIngrediente>();

            //pergunta se o alvo aceita este objeto em especifico
            if(recebedor != null && recebedor.AceitaIngrediente(this.gameObject)) {
                if (tornarFilhoDoAlvo)
                {
                    transform.SetParent(alvo.transform);
                }

                if (imantarNoCentro)
                {
                    if (tornarFilhoDoAlvo) transform.localPosition = Vector3.zero;
                    else transform.position = alvo.transform.position;
                }

                //entrega o objeto para a bancada e salva a bancada
                recebedor.ReceberIngredienteSolto(this.gameObject);
                bancadaAtual = recebedor;
                lugarValido = true;
                break;
            }
        }

        if (!lugarValido) {
            transform.position = posicaoOrigem;
            transform.SetParent(parenteOrigem);

            if (bancadaDeOrigem != null) {
                // Devolve para a memória da bancada anterior
                bancadaDeOrigem.ReceberIngredienteSolto(this.gameObject);
                bancadaAtual = bancadaDeOrigem;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D outro)
    {
        //verifica se o objeto detectado pode ser um recebedor
        if (outro.GetComponent<IReceberIngrediente>() != null)
        {
            if(!alvosPossiveis.Contains(outro.gameObject))
                alvosPossiveis.Add(outro.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D outro)
    {
        //verifica se o objeto da qual esta saindo eh o mesmo q estava
        //se for, limpa a memoria, para evitar q esse objeto fique puxando novamente o item
        if (alvosPossiveis.Contains(outro.gameObject))
        {
            alvosPossiveis.Remove(outro.gameObject);
        }
    }
}