using UnityEngine;

public class MinigameCorteUI : MonoBehaviour
{
    [Header("Referências da UI")]
    public GameObject painelVisual;
    public RectTransform barraFundo;
    public RectTransform zonaAcerto;
    public RectTransform cursor;

    private float larguraRealUI;

    private void Awake()
    {
        larguraRealUI = barraFundo.rect.width;
    }

    private void OnEnable()
    {
        // Se inscreve nos eventos do EventBus
        EventBusCorte.OnAtualizarZA += DesenharZona;
        EventBusCorte.OnCursorMove += DesenharCursor;
        
        EventBusCorte.OnIniciarMiniGame += LigarUI;
        EventBusCorte.OnTerminarMiniGame += DesligarUI;
    }

    private void OnDisable()
    {
        // Se desinscreve
        EventBusCorte.OnAtualizarZA -= DesenharZona;
        EventBusCorte.OnCursorMove -= DesenharCursor;
        
        EventBusCorte.OnIniciarMiniGame -= LigarUI;
        EventBusCorte.OnTerminarMiniGame -= DesligarUI;
    }

    private void LigarUI(IngredienteDataSO dados)
    {
        painelVisual.SetActive(true);
    }

    private void DesligarUI()
    {
        painelVisual.SetActive(false);
    }

    private void DesenharZona(float posMatematica, float larguraMatematica)
    {
        float posPixel = (posMatematica / 100f) * larguraRealUI;
        float largPixel = (larguraMatematica / 100f) * larguraRealUI;

        zonaAcerto.anchoredPosition = new Vector2(posPixel, zonaAcerto.anchoredPosition.y);
        zonaAcerto.sizeDelta = new Vector2(largPixel, zonaAcerto.sizeDelta.y);
    }

    private void DesenharCursor(float posMatematica)
    {
        float posPixel = (posMatematica / 100f) * larguraRealUI;
        cursor.anchoredPosition = new Vector2(posPixel, cursor.anchoredPosition.y);
    }
}