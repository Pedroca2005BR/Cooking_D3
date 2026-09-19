using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DropSystem : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [Header("Regras")]

    public string tagDoAlvo;
    public bool imantarNoCentro = true;
    public bool tornarFilhoDoAlvo = false;

    [Header("Eventos")]
    public UnityEvent AoLevantar;
    public UnityEvent<GameObject> AoSoltarNoAlvo;

    private GameObject alvoAtual;

    public void OnPointerDown(PointerEventData eventData)
    {
        AoLevantar?.Invoke(); // Avisa outros scripts que o objeto está no ar
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log($"Soltei o mouse! O alvo atual que o sistema encontrou foi: {(alvoAtual != null ? alvoAtual.name : "NENHUM")}");
        if (alvoAtual != null)
        {
            if (tornarFilhoDoAlvo)
            {
                transform.SetParent(alvoAtual.transform);
            }

            if (imantarNoCentro)
            {
                // Se virou filho, o localPosition 0,0,0 é mais seguro. Se não, usa a posição global.
                if (tornarFilhoDoAlvo) transform.localPosition = Vector3.zero;
                else transform.position = alvoAtual.transform.position;
            }

            // Avisa os outros scripts que o encaixe foi um sucesso e passa quem é o alvo
            AoSoltarNoAlvo?.Invoke(alvoAtual);
        }
    }

    private void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag(tagDoAlvo))
        {
            alvoAtual = outro.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D outro)
    {
        if (outro.CompareTag(tagDoAlvo) && alvoAtual == outro.gameObject)
        {
            alvoAtual = null;
        }
    }
}
