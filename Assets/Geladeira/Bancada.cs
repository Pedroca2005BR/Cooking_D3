
using System.Collections.Generic;
using UnityEngine;

public class Bancada : MonoBehaviour
{
    [Header("Posições de destino")]
    [SerializeField] private Transform[] destinationSlots;

    [Header("Detecção")]
    [SerializeField] private LayerMask itemLayer;

    [Header("Bloqueio")]
    [SerializeField] private GameObject botaoTransportar;

    private readonly List<GameObject> storedItems = new();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsItemLayer(other.gameObject.layer))
            return;

        GameObject item = other.gameObject;

        if (storedItems.Contains(item))
            return;

        storedItems.Add(item);

        // Se todos os slots estiverem ocupados, bloqueia o botão de transporte.
        if (storedItems.Count > destinationSlots.Length)
        {
            botaoTransportar.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (storedItems.Remove(other.gameObject))
        {
            // Se um ingrediente sair, libera o envio novamente
            if (storedItems.Count <= destinationSlots.Length)
            {
                botaoTransportar.SetActive(true);
            }
        }
    }

    private bool IsItemLayer(int layer)
    {
        return (itemLayer.value & (1 << layer)) != 0;
    }

    public void TransportItems()
    {
        storedItems.RemoveAll(item => item == null);

        int slotIndex = 0;
        bool placed = false;

        foreach (GameObject item in storedItems)
        {


            while (slotIndex < destinationSlots.Length)
            {
                Transform destination = destinationSlots[slotIndex];
                slotIndex++;

                if (destination == null)
                    continue;

                // Verifica se existe algum collider no destino.
                Collider2D hit = Physics2D.OverlapPoint(destination.position);

                // Se o destino estiver ocupado por outro objeto, pula.
                if (hit != null && hit.gameObject != item)
                    continue;

                item.transform.position = destination.position;
                item.transform.rotation = destination.rotation;

                placed = true;
                break;
            }

            if (!placed)
            {
                Debug.LogWarning(
                    "Não há destinos livres para todos os ingredientes!",
                    this
                );
                break;
            }
        }
        // Limpa a lista de itens armazenados apenas se todos os itens foram colocados com sucesso.
        if (placed)
        {
            storedItems.Clear();
            botaoTransportar.SetActive(true);
        }
    }
}
