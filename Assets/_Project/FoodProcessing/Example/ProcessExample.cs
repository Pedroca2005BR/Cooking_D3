using UnityEngine;

namespace Process.Example
{
    public class ProcessExample : MonoBehaviour
    {
        public CookingProcess cookingProcess;
        public int score;
        public float cooldown = 2f;

        IngredientComponent ig;
        float _cd = -1;

        private void Start()
        {
            Debug.Log("Existo " + TryGetComponent<Collider2D>(out Collider2D col));
        }

        private void Update()
        {
            if (_cd > 0)
            {

                _cd -= Time.deltaTime;

                if (_cd < 0 && ig != null)
                {
                    ig.AddProcess(cookingProcess, score);
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
