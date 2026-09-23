using UnityEngine;
using UnityEngine.UI;

public class UI_BarraPanela: MonoBehaviour
{
    [Header("Conexoes")]
    [SerializeField] private PanelaController panela;
    [SerializeField] private Image barra;

    [Header("UI Painel")]
    [SerializeField] private GameObject painelVisual;
    //painel contendo os elementos visuais da UI

    private void OnEnable() { 
        if(panela != null) {
            panela.OnProcesso += AtualizarBarra;
            panela.OnIniciarUI += LigarUI;
            panela.OnPausarUI += DesligarUI;
        }
    }

    private void OnDisable() {
        if(panela != null) {
            panela.OnProcesso -= AtualizarBarra;
            panela.OnIniciarUI -= LigarUI;
            panela.OnPausarUI -= DesligarUI;
        }
    }

    private void AtualizarBarra(float porcentagem) { //atualiza a barra de cozimento
        barra.fillAmount = porcentagem;
    }

    private void LigarUI() //ativa UI
    {
        if(painelVisual != null) painelVisual.SetActive(true);
    }

    private void DesligarUI() //desativa UI
    {
        if(painelVisual != null) painelVisual.SetActive(false);
    }
}
