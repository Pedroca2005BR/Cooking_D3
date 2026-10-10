using Unity.VisualScripting;
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

    //[Header("Miniaturas")]
    //[SerializeField] private Transform containerIcones;

    private void OnEnable() { 
        if(panela != null) {
            panela.OnProcesso += AtualizarBarra;
            panela.OnIniciarUI += LigarUI;
            panela.OnPausarUI += DesligarUI;
            panela.OnConfigurarUI += AjustarPosicaoUI;
            //panela.OnFusaoAlterada += AtualizarIcones;
        }
    }

    private void OnDisable() {
        if(panela != null) {
            panela.OnProcesso -= AtualizarBarra;
            panela.OnIniciarUI -= LigarUI;
            panela.OnPausarUI -= DesligarUI;
            panela.OnConfigurarUI -= AjustarPosicaoUI;
            //panela.OnFusaoAlterada -= AtualizarIcones;
        }
    }

    private void Start() {
        DesligarUI(); // Garante que começa oculto
    }

    private void AjustarPosicaoUI(bool aparecerEmCima, float distanciaY)
    {
        if (painelVisual != null)
        {
            // Força o valor de Y a ser positivo (cima) ou negativo (baixo) baseado no booleano
            float novaPosicaoY = aparecerEmCima ? Mathf.Abs(distanciaY) : -Mathf.Abs(distanciaY);
            
            // Tenta ajustar pelo RectTransform (ideal para UI), ou usa o Transform normal como fallback
            RectTransform rect = painelVisual.GetComponent<RectTransform>();
            if (rect != null) {
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, novaPosicaoY);
            } else {
                painelVisual.transform.localPosition = new Vector3(
                    painelVisual.transform.localPosition.x,
                    novaPosicaoY,
                    painelVisual.transform.localPosition.z
                );
            }
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

 /*   private void AtualizarIcones(Sprite[] spritesPanela)
    {
        if(containerIcones == null) return;

        foreach(Transform filho in containerIcones)
        {
            Destroy(filho.gameObject);
        }

        foreach (Sprite sprite in spritesPanela) {
            if (sprite == null) continue;

            // cria um objeto vazio na hierarquia
            GameObject novoIcone = new GameObject("Icone_Ingrediente");
            novoIcone.transform.SetParent(containerIcones, false);

            // adiciona o componente visual
            Image img = novoIcone.AddComponent<Image>();
            img.sprite = sprite;
            
            // Força um tamanho padrão amigável para a tela (30x30 pixels)
            RectTransform rect = novoIcone.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(30f, 30f); 
        }
    }
}*/