using UnityEngine;
using UnityEngine.UI;

public class UI_BarraPanela: MonoBehaviour
{
    [Header("Conexoes")]
    [SerializeField] private PanelaController panela;
    [SerializeField] private Image barra;

    private void OnEnable() { 
        if(panela != null) {
            panela.OnProcesso += AtualizarBarra;
        }
    }

    private void OnDisable() {
        if(panela != null) {
            panela.OnProcesso -= AtualizarBarra;
        }
    }

    private void AtualizarBarra(float porcentagem) {
        barra.fillAmount = porcentagem;
    }
}
