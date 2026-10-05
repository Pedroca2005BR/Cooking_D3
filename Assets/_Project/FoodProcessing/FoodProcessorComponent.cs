using UnityEngine;
using UnityEngine.Events;

public class FoodProcessorComponent : MonoBehaviour
{
    [SerializeField] CookingProcess process;

    public CookingProcess ProcessoAtual => process;
    
    public void ProcessIngredient(IngredientComponent ingredient, int score)
    {
        ingredient.AddProcess(process, score);
    }
}
