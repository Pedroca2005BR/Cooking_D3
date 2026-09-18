using System.Collections;
using UnityEngine;

public class GerenciadorTempoFase: MonoBehaviour {
    [Header("Condiguração de Tempo")]
    [SerializeField] private int tempoReceita = 180;

    private int tempoAtual;
    private bool acabouTempo = true;

    private void Start() {
        tempoAtual = tempoReceita;
        acabouTempo = false;

        StartCoroutine(RotinaCronometro());
    }

    private IEnumerator RotinaCronometro() {
        while(tempoAtual > 0 && !acabouTempo) {
            EventBusGlobal.DispararTempoAtual(tempoAtual);

            yield return new WaitForSeconds(1f);

            tempoAtual--;
        }

        if(tempoAtual <= 0) {
            EventBusGlobal.DispararTempoAtual(0);
            EventBusGlobal.DispararTempoEsgotado();
            acabouTempo = true;
        }
    }
}