using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteracaoTeste : MonoBehaviour
{
    List<IInteragivel> interagiveisEmRange = new List<IInteragivel>();  // Lista de todos os interagiveis em range

    public static event Action<int, Transform> OnInteracao; // Evento disparado apos um objeto ser interagido

    IInteragivel interagivelAtual = null;

    // Listener de eventos de input
    private void OnEnable()
    {
        InputTeste.OnInteractInput += Interage;
    }
    private void OnDisable()
    {
        InputTeste.OnInteractInput -= Interage;
    }

    // No FixedUpdate, vai ser procurado o objeto interagível mais próximo
    void FixedUpdate()
    {
        if (interagiveisEmRange.Count > 0)
        {
            IInteragivel interagivelBusca = null;
            float distancia = 100f;
            float distanciaAux;
            foreach (IInteragivel interagivel in interagiveisEmRange)
            {
                distanciaAux = Vector2.Distance(transform.position, interagivel.transform.position);
                if (distanciaAux <= distancia)
                {
                    distancia = distanciaAux;
                    interagivelBusca = interagivel;
                }
            }

            if (interagivelBusca != null)
            {
                if (interagivelAtual != null)
                {
                    interagivelAtual.EscondeInput();
                }
                interagivelBusca.MostraInput();

                interagivelAtual = interagivelBusca;
            }
        }
        else if (interagivelAtual != null)
        {
            interagivelAtual.EscondeInput();
            interagivelAtual = null;
        }
    }

    // Função disparada ao ser detectado input, que escolhe qual objeto no range vai ser interagido e/ou dispara evento de interação
    public void Interage()
    {
        if (interagivelAtual != null && interagivelAtual.PodeInteragir())
        {
            interagivelAtual?.Interagir();

            OnInteracao?.Invoke(interagivelAtual.GetTipo(), interagivelAtual.transform);
        }

        else    // Se não há interagiveis válidos, gera evento com valores invalidos (Objetos listeners saberão que houve input)
        {
            OnInteracao?.Invoke(-1, null);
        }
    }


    // Interagiveis que entram na área são adicionados a lista
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteragivel interagivel))
        {
            interagiveisEmRange.Add(interagivel);
        }
    }

    // Interagiveis que saem na área são removidos da lista
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteragivel interagivel))
        {
            interagiveisEmRange.Remove(interagivel);
        }
    }

    
}
