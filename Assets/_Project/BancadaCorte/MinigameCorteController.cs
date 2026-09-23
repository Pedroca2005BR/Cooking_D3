using UnityEngine;
using System.Collections;

public class MinigameCorte : MonoBehaviour
{
    private IngredienteDataSO dados;
    private bool rodando = false;
    private bool congelado = false;
    private int cortes = 0;

    private float larguraBase = 100f;
    private float posCursor = 0f;
    private float direcao = 1f;

    private float posZonaAcerto;
    private float larguraZona;

    private void OnEnable() {
        EventBusCorte.OnIniciarMiniGame += Iniciar;
        EventBusCorte.OnCortar += AvaliarCorte;
    }

    private void OnDisable() {
        EventBusCorte.OnIniciarMiniGame -= Iniciar;
        EventBusCorte.OnCortar -= AvaliarCorte;
    }

    private void Iniciar(IngredienteDataSO novosDados) {
        dados = novosDados;
        cortes = 0;
        posCursor = 0f;
        direcao = 1f;
        
        larguraZona = larguraBase * dados.tamanhoZonaAcerto;
        SortearZona();
        rodando = true;
    }

    private void SortearZona()
    {
        posZonaAcerto = Random.Range(0f, larguraBase - larguraZona);
        EventBusCorte.OnAtualizarZA?.Invoke(posZonaAcerto, larguraZona);
    }

    private void Update() {
        if(!rodando || congelado) return;

        posCursor += direcao * dados.velocidadeCursor * 50f * Time.deltaTime;

        if(posCursor >= larguraBase) { //inverte se bate na barreira
            posCursor = larguraBase;
            direcao = -1f;
        }else if (posCursor <= 0f) {
            posCursor = 0f;
            direcao = 1f;
        }

        EventBusCorte.OnCursorMove?.Invoke(posCursor);
    }

    private void AvaliarCorte()
    {
        if (!rodando || congelado) return;

        if (posCursor >= posZonaAcerto && posCursor <= (posZonaAcerto + larguraZona))
        {
            cortes++;
            EventBusCorte.OnCorteAcertado?.Invoke();
        }else {
            EventBusCorte.OnCorteErrado?.Invoke();
        }
        StartCoroutine(RotinaCongelar());
    }

    private IEnumerator RotinaCongelar()
    {
        congelado = true;
        yield return new WaitForSeconds(0.15f); // Pausa dramática
        
        if (cortes >= dados.totalDeCortes)
        {
            rodando = false;
            EventBusCorte.OnFimMiniGame?.Invoke();
            EventBusCorte.OnTerminarMiniGame?.Invoke();
        }
        else
        {
            SortearZona();
            congelado = false;
        }
    }
}
