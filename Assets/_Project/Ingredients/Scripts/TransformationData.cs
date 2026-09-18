using UnityEngine;

[System.Serializable]
public class TransformationData
{
    public bool changeName;
    public string Name;

    public CookingProcess process;
    public int minRange;
    public int maxRange;

    
    public Sprite newSprite;

    [Header("Remember to change Alpha")]
    public Color newColor;
}
