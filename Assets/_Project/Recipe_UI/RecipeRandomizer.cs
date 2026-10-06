using UnityEngine;

public class RecipeRandomizer : MonoBehaviour
{
    [SerializeField] RecipeDatabase database;

    private void Start()
    {
        int random = Random.Range(0, database.recipes.Length);
        StepByStepChecklist.Instance.Setup(database.recipes[random]);
    }
}
