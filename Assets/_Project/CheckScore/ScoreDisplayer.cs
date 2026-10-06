using TMPro;
using UnityEngine;

public class ScoreDisplayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;
    
    public void DisplayScore(float score)
    {
        textMesh.text = $"Score: {score * 100f:0.00}%";
    }
}
