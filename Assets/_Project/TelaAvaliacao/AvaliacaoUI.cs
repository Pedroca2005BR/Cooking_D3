using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AvaliacaoUI : MonoBehaviour
{
    [Header("Referências da Tela")]
    [SerializeField] private GameObject painelPrincipal; 
    [SerializeField] private TextMeshProUGUI textoScore;
    [SerializeField] private Image imagemEstrelas;
    [SerializeField] private Image imagemVelhinha;
    [SerializeField] private Image imagemPrato;

    public void EsconderTela()
    {
        painelPrincipal.SetActive(false);
    }

    public void MostrarTela(int nota, Sprite spriteEstrela, Sprite spriteVelhinha, Sprite spritePrato)
    {
        textoScore.text = $"{nota}";
        
        imagemEstrelas.sprite = spriteEstrela;
        imagemVelhinha.sprite = spriteVelhinha;
        imagemPrato.sprite = spritePrato;
        
        painelPrincipal.SetActive(true);
    }
}