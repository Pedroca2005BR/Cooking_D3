using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BaseIngredientData", menuName = "Scriptable Objects/BaseIngredientData")]
public class BaseIngredientData : ScriptableObject
{
    public string Name;
    public Sprite baseSprite;

    [Tooltip("When an ingredient goes through a process, they can change appearance.")]
    public List<TransformationData> processes;

    public bool TryGetNewSprite(CookingProcess process, int score, out Sprite sprite, out Color color)
    {
        var data = GetTransformationDataInRange(process, score);
        if (data == null)
        {
            sprite = null;
            color = Color.white;
            return false;
        }

        sprite = data.newSprite;
        color = data.newColor;

        return true;
    }

    private TransformationData GetTransformationDataInRange(CookingProcess process, int score)
    {
        foreach (TransformationData data in processes)
        {
            if (data.process == process)
            {
                if (data.minRange >= score && data.maxRange <= score)
                {
                    return data;
                }
            }
        }
        
        return null;
    }
}
