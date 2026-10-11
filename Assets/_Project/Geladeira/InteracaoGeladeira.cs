using UnityEngine;
using UnityEngine.EventSystems;
using Pedroca2005BR.Utilities;

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

            IngredientComponent comp = GetComponent<IngredientComponent>();
            if (comp != null)
            {
                EventManager.TriggerEvent(EventConstantNames.INGREDIENT_CREATED, new IngredientEventData(comp.data, comp, comp));
            }
            
            // 2. Destrói este script de clique para o item voltar a ser normal
            Destroy(this);
        }
    }
}