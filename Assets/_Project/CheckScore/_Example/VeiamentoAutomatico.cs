using TMPro;
using UnityEngine;

namespace CheckScore.Example
{
    public class VeiamentoAutomatico : MonoBehaviour
    {
        bool calculou = false;

        public IdealIngredientData ideal;
        public ScoreDisplayer scoreDisplayer;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!calculou)
            {
                calculou = true;
                
                var score = ScoreCalculator.Calculate(
                    collision.GetComponent<IngredientComponent>().ingredientInstance,
                    ideal
                );

                scoreDisplayer.DisplayScore(score);
            }
        }
    }
}