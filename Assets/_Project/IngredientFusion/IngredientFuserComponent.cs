using System.Linq;
using UnityEngine;

public class IngredientFuserComponent : MonoBehaviour
{
    [SerializeField] FusionTree fusionTree;

    [Header("Instantiate Settings")]
    [SerializeField] GameObject ingredientPrefab;
    [SerializeField] Transform position;

    public bool TryFusing(IngredientComponent[] ingredients, out GameObject fusedIngredient)
    {
        BaseIngredientData[] ingInfo = new BaseIngredientData[ingredients.Length];
        IngredientRuntimeInstance[] runtimeInstances = new IngredientRuntimeInstance[ingredients.Length];
        fusedIngredient = null;

        for(int i = 0; i < ingredients.Length; i++)
        {
            ingInfo[i] = ingredients[i].data;
            runtimeInstances[i] = ingredients[i].ingredientInstance;
        }

        BaseIngredientData newIng = fusionTree.TryFusing(ingInfo);

        if (newIng != null)
        {
            if (position == null) position = transform;

            fusedIngredient = Instantiate(ingredientPrefab, position.position, Quaternion.identity);
            fusedIngredient.GetComponent<IngredientComponent>().Setup(newIng, runtimeInstances);

            for(int i = 0;i < ingredients.Length; i++)
            {
                Destroy(ingredients[i].gameObject);
            }

            return true;
        }
        else
        {
            return false;
        }
    }
}
