using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Process.Example
{
    public class FuseExample : MonoBehaviour
    {
        public FusionTree FusionTree;
        public GameObject IngredientPrefab;

        List<IngredientComponent> ingredients = new();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IngredientComponent>(out IngredientComponent ing))
            {
                if (!ingredients.Contains(ing))
                {
                    ingredients.Add(ing);
                }

                if (ingredients.Count > 1)
                {
                    List<BaseIngredientData> data = new();
                    List<IngredientRuntimeInstance> runtimeInstances = new();

                    foreach(var ingredient in ingredients)
                    {
                        data.Add(ingredient.data);
                        runtimeInstances.Add(ingredient.ingredientInstance);
                    }

                    BaseIngredientData newIng =  FusionTree.TryFusing(data.ToArray());

                    if (newIng != null)
                    {
                        Instantiate(IngredientPrefab, transform.position, Quaternion.identity).GetComponent<IngredientComponent>()
                        .Setup(newIng, runtimeInstances.ToArray());

                        ingredients.ForEach(ing  => Destroy(ing.gameObject));
                        ingredients.Clear();
                    }
                    
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IngredientComponent>(out IngredientComponent ing))
            {
                ingredients.Remove(ing);
            }
        }
    }
}
