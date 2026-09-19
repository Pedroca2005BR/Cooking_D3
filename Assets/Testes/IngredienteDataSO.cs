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
    public Sprite spriteQueimado; // Arte da comida arruinada
}