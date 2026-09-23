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

    public bool TryTransforming(CookingProcess process, int score, out BaseIngredientData newData)
    {
        newData = null;

        foreach (IngredientConversionData data in possibleConversions)
        {
            if (data.processNeeded == process && data.minimumScoreNeeded <= score)
            {
                
                newData = data.newIngredientData;
                return true;
               
            }
        }

        return false;
    }
}
