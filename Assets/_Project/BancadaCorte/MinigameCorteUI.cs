using UnityEngine;
using TMPro;

public class MinigameCorteUI : MonoBehaviour
{
    [Header("Referências da UI")]
    public GameObject painelVisual;
    public RectTransform barraFundo;
    public RectTransform zonaAcerto;
    public RectTransform cursor;

    [Header("Informações de Jogo")]
    public TextMeshProUGUI textoTempo; 
    public TextMeshProUGUI textoCortes;

    private float larguraRealUI;

    private void Awake()
    {
        Canvas.ForceUpdateCanvases();
        larguraRealUI = barraFundo.rect.width;
        zonaAcerto.pivot = new Vector2(0, 0.5f);
        cursor.pivot = new Vector2(0, 0.5f);
    }

    private void OnEnable()
    {
        // Se inscreve nos eventos do EventBus
        EventBusCorte.OnAtualizarZA += DesenharZona;
        EventBusCorte.OnCursorMove += DesenharCursor;
        
        EventBusCorte.OnIniciarMiniGame += LigarUI;
        EventBusCorte.OnTerminarMiniGame += DesligarUI;

        EventBusCorte.OnTempoAlterado += AtualizarTextoTempo;
        EventBusCorte.OnCortesRestantesAlterados += AtualizarTextoCortes;
    }

    private void OnDisable()
    {
        // Se desinscreve
        EventBusCorte.OnAtualizarZA -= DesenharZona;
        EventBusCorte.OnCursorMove -= DesenharCursor;
        
        EventBusCorte.OnIniciarMiniGame -= LigarUI;
        EventBusCorte.OnTerminarMiniGame -= DesligarUI;

        EventBusCorte.OnTempoAlterado -= AtualizarTextoTempo;
        EventBusCorte.OnCortesRestantesAlterados -= AtualizarTextoCortes;
    }

    private void LigarUI(int _, float __, float ___, float ____)
    {
        painelVisual.SetActive(true);
    }

    private void DesligarUI()
    {
        painelVisual.SetActive(false);
    }

    private void AtualizarTextoTempo(float tempo) {
        if (textoTempo != null) {
            // Formata o float para 1 casa decimal (Ex: 4.5s)
            textoTempo.text = "Tempo:" + tempo.ToString("F1") + "s"; 
        }
    }

    private void AtualizarTextoCortes(int cortesRestantes) {
        if (textoCortes != null) {
            textoCortes.text = "Restam " + cortesRestantes.ToString() + " cortes";
        }
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