using UnityEngine;

namespace Process.Example
{
    [RequireComponent(typeof(FoodProcessorComponent))]
    public class ProcessExample : MonoBehaviour
    {
        public int score;
        public float cooldown = 2f;
        FoodProcessorComponent fpc;

        IngredientComponent ig;
        float _cd = -1;

        private void Start()
        {
            fpc = GetComponent<FoodProcessorComponent>();
        }

        private void Update()
        {
            if (_cd > 0)
            {

                _cd -= Time.deltaTime;

                if (_cd < 0 && ig != null)
                {
                    fpc.ProcessIngredient(ig, score);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("EnterCol with "+ collision.name);
            if (collision.TryGetComponent<IngredientComponent>(out IngredientComponent ing))
            {
                ig = ing;
                _cd = cooldown;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IngredientComponent>(out IngredientComponent ing))
            {
                if (ing == ig) ig = null;
                _cd = -1;
            }
        }
    }
}
