using UnityEngine;

public class BocaReceber : MonoBehaviour, IReceberIngrediente
{
    public bool AceitaIngrediente(GameObject objeto)
    {
        return objeto.GetComponent<PanelaController>() != null;
    }

    public void ReceberIngredienteSolto(GameObject objeto)
    {
        PanelaController panela = objeto.GetComponent<PanelaController>();
        
        if (panela != null)
        {
            objeto.transform.position = this.transform.position;
            panela.LigarFogo();
            Debug.Log($"Panela conectada na {gameObject.name}");
        }
    }

    public void RemoverIngrediente(GameObject objeto)
    {
        PanelaController panela = objeto.GetComponent<PanelaController>();
        if (panela != null)
        {
            panela.DesligarFogo();
            Debug.Log($"Panela removida da {gameObject.name}");
        }
    }
}