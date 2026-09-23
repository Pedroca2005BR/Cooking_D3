using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Process.Example
{
    [RequireComponent(typeof(IngredientFuserComponent))]
    public class FuseExample : MonoBehaviour
    {
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
                    if (GetComponent<IngredientFuserComponent>().TryFusing(ingredients.ToArray(), out var newIng))
                    {
                        ingredients.Clear();
                        ingredients.Add(newIng.GetComponent<IngredientComponent>());
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
