using UnityEngine;

[CreateAssetMenu(fileName = "NovoIngrediente", menuName = "Cozinha/Ingrediente")]
public class IngredienteDataSO : ScriptableObject
{
    [Header("Informações Base")]
    public string nomeIngrediente;
    public float tempoCozinhar = 10f; 
    public float tempoQueimar = 15f; // Se ficar 15 segundos no total, queima.

    [Header("Visuais")]
    public Sprite spriteCru;
    public Sprite spriteCozido;
    public Sprite spriteQueimado;

    [Header("Dados do Corte (Minigame)")]
    public Sprite spriteCortado;       // Como ele fica depois de picado
    public int totalDeCortes = 3;      // Quantas vezes precisa acertar
    public float velocidadeCursor = 2f;// Velocidade do ponteiro
    
    [Range(0.05f, 1f)] 
    public float tamanhoZonaAcerto = 0.2f; //tamanho da area de acerto para corte
}