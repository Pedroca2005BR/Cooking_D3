using UnityEngine;
using UnityEngine.EventSystems;

public class InteracaoGeladeira : MonoBehaviour, IPointerDownHandler
{
    private Transform destinoDaBancada;

    public void Setup(Transform destino)
    {
        destinoDaBancada = destino;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (destinoDaBancada != null)
        {
            // 1. Teletransporta o ingrediente direto para dentro do colisor da Bancada.cs
            transform.position = destinoDaBancada.position;

            DraggableComponent arrasto = GetComponent<DraggableComponent>();
            if (arrasto != null) arrasto.enabled = true;
            
            // 2. Destrói este script de clique para o item voltar a ser normal
            Destroy(this);
        }
    }
}