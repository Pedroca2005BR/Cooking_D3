using UnityEngine;
using UnityEngine.EventSystems;

public class BotaoEntrega : MonoBehaviour, IPointerDownHandler
{
    [Header("Conexão")]
    [Tooltip("Arraste o objeto que contém o PlateComponent aqui.")]
    [SerializeField] private PlateComponent plateComponent;

    private void OnEnable()
    {
        EventBusGlobal.OnTempoEsgotado += Acabou;
    }
    private void OnDisable()
    {
        EventBusGlobal.OnTempoEsgotado -= Acabou;
    }

    void Acabou()
    {
        OnPointerDown(null);
    }

    // Este método é disparado automaticamente pela Unity no exato frame em que o rato clica no BoxCollider2D deste objeto.
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clique detectado no botão físico!");
        if (plateComponent != null)
        {
            plateComponent.Deliver();
            Debug.Log("Deliver executado!!!");
        }
        else
        {
            Debug.LogWarning("PlateComponent não referenciado no Botão de Entrega!");
        }
    }
}