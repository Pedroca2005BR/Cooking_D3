using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BaseIngredientData", menuName = "Scriptable Objects/BaseIngredientData")]
public class BaseIngredientData : ScriptableObject
{
    public string baseName;
    public Sprite baseSprite;

    [Tooltip("When an ingredient goes through a process, they can convert into another one.")]
    public List<IngredientConversionData> possibleConversions;

    [Header("Junk")]
    [Tooltip("Ingrediente gerado quando a combinação não leva a nenhuma receita (a 'gororoba').")]
    [SerializeField] BaseIngredientData junkIngredient;

    public bool TryTransforming(CookingProcess process, int score, out BaseIngredientData newData)
    {
        newData = junkIngredient;

        foreach (IngredientConversionData data in possibleConversions)
        {
            if (data.processNeeded == process)
            {
                if (data.minimumScoreNeeded <= score)
                {
                    newData = data.newIngredientData;
                    return true;
                }

                // If score is not enough, but the process is correct, returns false and nothing happens
                return false;
            }
        }

        // If the process is wrong, returns true with the junkIngredient
        return true;
    }

    public int GetScoreNeeded(CookingProcess process)
    {
        foreach (var data in possibleConversions)
        {
            if (process == data.processNeeded)
            {
                return data.minimumScoreNeeded;
            }
        }

        Debug.Log("No such process for this ingredient.");
        return -1;
    }
}
