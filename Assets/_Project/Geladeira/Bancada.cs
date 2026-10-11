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
        Debug.Log($"[Bancada] O objeto '{other.gameObject.name}' tocou na cesta. Layer dele: {LayerMask.LayerToName(other.gameObject.layer)}");

        if (!IsItemLayer(other.gameObject.layer)) 
        {
            Debug.LogWarning($"[Bancada] '{other.gameObject.name}' foi IGNORADO! A layer dele não está marcada na 'Item Layer' da Bancada no Inspector.");
            return;
        }

        GameObject item = other.gameObject;
        if (storedItems.Contains(item)) return;

        storedItems.Add(item);
        Debug.Log($"[Bancada] '{item.name}' foi guardado na memória da cesta! Total na cesta: {storedItems.Count}");

        if (storedItems.Count > destinationSlots.Length)
        {
            botaoTransportar.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (storedItems.Remove(other.gameObject))
        {
            Debug.Log($"[Bancada] '{other.gameObject.name}' foi retirado da cesta.");
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
        Debug.Log($"[Bancada] Botão clicado! Tentando transportar {storedItems.Count} itens...");
        storedItems.RemoveAll(item => item == null);
        int slotIndex = 0;

        bool placed = false;
        foreach (GameObject item in storedItems)
        {
            bool itemPlaced = false;
            while (slotIndex < destinationSlots.Length)
            {
                Transform destination = destinationSlots[slotIndex];
                slotIndex++;
                if (destination == null) continue;
                
                Collider2D hit = Physics2D.OverlapPoint(destination.position, itemLayer.value);
                if (hit != null && hit.gameObject != item)
                {
                    Debug.LogWarning($"[Bancada] O slot '{destination.name}' já está bloqueado pelo objeto '{hit.gameObject.name}'. Tentando o próximo slot...");
                    continue;
                }
                
                item.transform.position = destination.position;
                item.transform.rotation = destination.rotation;
                item.transform.localScale = Vector3.one; 
                
                Debug.Log($"[Bancada] SUCESSO: '{item.name}' foi movido para '{destination.name}'.");
                placed = true;
                itemPlaced = true;
                break;
            }

            if (!itemPlaced)
            {
                Debug.LogError($"[Bancada] Falha: Não há nenhum destino livre para '{item.name}'!");
            }
        }
        
        if (placed)
        {
            storedItems.Clear();
            botaoTransportar.SetActive(true);
        }
    }
}