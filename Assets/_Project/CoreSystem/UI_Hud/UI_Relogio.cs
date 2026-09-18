using UnityEngine;
using TMPro;

public class UI_Relogio : MonoBehaviour
{
    private TextMeshProUGUI textoUI;

    //pega o componente de texto da UI
    private void Awake() {
        textoUI = GetComponent<TextMeshProUGUI>();
    }

    //assinar e desassinar os eventos
    private void OnEnable() {
        EventBusGlobal.OnTempoAtual += AtualizarTexto;
        EventBusGlobal.OnTempoEsgotado += MostrarFimJogo;
    }
    
    private void OnDisable() {
        EventBusGlobal.OnTempoAtual -= AtualizarTexto;
        EventBusGlobal.OnTempoEsgotado -= MostrarFimJogo;
    }

    //funcao para atualizar o tempo no relogio
    private void AtualizarTexto(int totalTempo) {
        int minutos = totalTempo/60;
        int segundos = totalTempo%60;

        textoUI.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    private void MostrarFimJogo() {
        //adicionar alguma coisa
    }
}
