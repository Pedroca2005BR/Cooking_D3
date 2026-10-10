using UnityEngine;
using System.Collections;

public class MinigameCorte : MonoBehaviour
{
    private bool rodando = false;
    private bool congelado = false;

    // Dados do Minigame
    private int totalDeCortes;
    private float velocidadeCursor;
    private float tamanhoZonaAcerto;
    private float tempoMaximo;

    //Estado atual
    private int cortesRestantes;
    private float tempoRestante;
    private float larguraBase = 100f;
    private float posCursor = 0f;
    private float direcao = 1f;
    private float posZonaAcerto;
    private float larguraZona;

    private void OnEnable() {
        EventBusCorte.OnIniciarMiniGame += Iniciar;
        EventBusCorte.OnCortar += AvaliarCorte;
        EventBusCorte.OnTerminarMiniGame += PararLogica;
    }

    private void OnDisable() {
        EventBusCorte.OnIniciarMiniGame -= Iniciar;
        EventBusCorte.OnCortar -= AvaliarCorte;
        EventBusCorte.OnTerminarMiniGame += PararLogica;
    }

    private void Iniciar(int cortesAlvo, float velCursor, float tamZona, float tempoMax) {
        //recebe as configuracoes do minigame
        totalDeCortes = cortesAlvo;
        velocidadeCursor = velCursor;
        tamanhoZonaAcerto = tamZona;
        tempoMaximo = tempoMax;

        //reinicia o progresso
        ReiniciarProgresso();

        //calcula o tamanho da zona de corte e sorteia em que ponto ela vai comecar
        larguraZona = larguraBase * tamanhoZonaAcerto;
        SortearZona();
        rodando = true;
    }

    private void ReiniciarProgresso()
    {
        cortesRestantes = totalDeCortes;
        tempoRestante = tempoMaximo;
        posCursor = 0f;
        direcao = 1f;

        congelado = false;

        EventBusCorte.OnCortesRestantesAlterados?.Invoke(cortesRestantes);
        EventBusCorte.OnTempoAlterado?.Invoke(tempoRestante);
    }

    private void PararLogica() {
        rodando = false;
        congelado = false;
    }

    private void SortearZona()
    {
        posZonaAcerto = Random.Range(0f, larguraBase - larguraZona);
        EventBusCorte.OnAtualizarZA?.Invoke(posZonaAcerto, larguraZona);
    }

    private void Update() {
        if(!rodando) return;

        //calculo do tempo passado
        tempoRestante -= Time.deltaTime;
        EventBusCorte.OnTempoAlterado?.Invoke(tempoRestante);

        //se o tempo acabar, o minigame reinicia
        if(tempoRestante <= 0)
        {
            ReiniciarProgresso();
            return;
        }

        if(congelado) return; //se estiver congelado o cursos não se move, ou seja, o tempo passa mas o cursor não

        posCursor += direcao * velocidadeCursor * 50f * Time.deltaTime;

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
            cortesRestantes--;
            EventBusCorte.OnCortesRestantesAlterados?.Invoke(cortesRestantes);
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
        
        if (cortesRestantes <= 0)
        {
            rodando = false;
            EventBusCorte.OnFimMiniGame?.Invoke();
            EventBusCorte.OnTerminarMiniGame?.Invoke();
        }
        else
        {
            if (cortesRestantes < totalDeCortes) SortearZona();
            congelado = false;
        }
    }
}
