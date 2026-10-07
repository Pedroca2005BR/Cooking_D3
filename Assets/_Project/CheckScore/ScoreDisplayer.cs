using TMPro;
using UnityEngine;

public class ScoreDisplayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] string scorePrefix = "Score: ";
    [SerializeField] PlateComponent plateComponent;

    private void OnEnable()
    {
        if (plateComponent != null)
        {
            plateComponent.OnScoreCalculated += DisplayScore;
        }
    }

    private void OnDisable()
    {
        if (plateComponent != null)
        {
            plateComponent.OnScoreCalculated -= DisplayScore;
        }
    }

    public void DisplayScore(float score)
    {
        textMesh.text = $"{scorePrefix}{score * 100f:0.00}%";
    }
}
