using Unity.VisualScripting;
using UnityEngine;

public class ObjetoTeste : MonoBehaviour, IInteragivel
{
    [SerializeField] int tipoObjeto = 0;
    [SerializeField] GameObject objetoInputPrefab;
    bool podeInteragir = true;
    bool estado = false; 
    SpriteRenderer spriteRenderer;
    GameObject objetoInputInst = null;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool PodeInteragir()
    {
        return podeInteragir;
    }

    public void Interagir()
    {
        if (!podeInteragir) 
            return;
        
        MudaEstado();
    }

    public void MostraInput()
    {
        if (podeInteragir && objetoInputInst == null)
        {
            objetoInputInst = Instantiate(objetoInputPrefab, transform.position + Vector3.up, Quaternion.identity);
        }
    }

    public void EscondeInput()
    {
        if (objetoInputInst != null)
        {
            Destroy(objetoInputInst);
            objetoInputInst = null;
        }
    }

    public int GetTipo()
    {
        return tipoObjeto;
    }

    void MudaEstado()
    {
        estado = !estado;

        if (!estado)
        {
            spriteRenderer.color = Color.white;
        }
        else
        {
            spriteRenderer.color = Color.black;          
        }
    }
}
