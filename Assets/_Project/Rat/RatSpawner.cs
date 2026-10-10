using Pedroca2005BR.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class RatSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Transform spawnPoint;
    public float spawnRadius = 0.5f;
    [SerializeField] RatSpawnData[] ratPrefabs;
    [SerializeField] AnimationCurve ratSpawnChance; // This curve is used to determine the chance of spawning a rat based on the number of rats already present. The x-axis represents the ratio of current rats to max rats, and the y-axis represents the spawn chance (0 to 1).
    [SerializeField][Tooltip("Will be used to evalueate the curve.")] int maxRatAmount;

    [Header("Sprites")]
    [SerializeField] Sprite empty;
    [SerializeField] Sprite eyesInside;
    SpriteRenderer spriteRenderer;
    [SerializeField] float checkCooldown = 10f;
    [SerializeField]
    [Range(0f, 1f)] float eyesChance = 0.5f;
    [SerializeField]
    [Range(0f, 1f)] float increaseInSpawnChance = 0.2f;
    float checkTimer = 0f;



    List<RatBehaviour> rats = new();
    List<IngredientComponent> ingredients = new List<IngredientComponent>();

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = empty;
    }

    private void Update()
    {
        checkTimer += Time.deltaTime;

        if (checkTimer >= checkCooldown)
        {
            checkTimer = 0f;
            float rand = Random.value;
            if (rand < eyesChance)
            {
                spriteRenderer.sprite = eyesInside;
            }
            else
            {
                spriteRenderer.sprite = empty;
            }
        }
    }

    public void TrySpawningRat()
    {
        float rand = Random.value;
        float evaluationParameter = (float)rats.Count / maxRatAmount;
        float chance = ratSpawnChance.Evaluate(evaluationParameter);

        // Increase the chance based on sprite
        if (spriteRenderer.sprite == eyesInside)
        {
            chance += increaseInSpawnChance;
        }

        if (rand < chance)
        {
            SpawnRat();
        }
    }

    void SpawnRat()
    {
        int i;
        for (i = 0; i < ratPrefabs.Length; i++)
        {
            float rand = Random.value;
            if (rand < ratPrefabs[i].spawnChance)
            {
                break;
            }
        }

        var rat = Instantiate(ratPrefabs[i].ratPrefab, spawnPoint.position, Quaternion.identity, transform).GetComponent<RatBehaviour>();
        rat.home = spawnPoint;
        rats.Add(rat);
        GiveObjectives();
    }

    public void GiveObjectives()
    {
        rats.RemoveAll(r => r == null);

        if (ingredients.Count > 0)
        {
            foreach (var rat in rats)
            {
                int rand = Random.Range(0, ingredients.Count);
                //Debug.Log(ingredients[rand].name);
                rat.TryGiveObjectives(ingredients[rand].transform);
            }
        }
    }








    // This class subscribe to many events to keep track of the ingredients in the scene. 
    private void OnEnable()
    {
        EventManager.Subscribe(EventConstantNames.INGREDIENT_CREATED, OnIngredientCreated);
        EventManager.Subscribe(EventConstantNames.INGREDIENT_DROPPED, OnIngredientCreated);

        EventManager.Subscribe(EventConstantNames.INGREDIENT_CONSUMED, OnIngredientVanished);
        EventManager.Subscribe(EventConstantNames.INGREDIENT_STOLEN, OnIngredientVanished);
        EventManager.Subscribe(EventConstantNames.INGREDIENT_PICKED_UP, OnIngredientVanished);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventConstantNames.INGREDIENT_CREATED, OnIngredientCreated);
        EventManager.Unsubscribe(EventConstantNames.INGREDIENT_DROPPED, OnIngredientCreated);

        EventManager.Unsubscribe(EventConstantNames.INGREDIENT_CONSUMED, OnIngredientVanished);
        EventManager.Unsubscribe(EventConstantNames.INGREDIENT_STOLEN, OnIngredientVanished);
        EventManager.Unsubscribe(EventConstantNames.INGREDIENT_PICKED_UP, OnIngredientVanished);
    }


    // When an ingredient vanishes for any reason, it gets removed from the list
    private void OnIngredientVanished(object obj)
    {
        // Only receives FoodEvents
        IngredientEventData ingredient = (IngredientEventData)obj;

        if (ingredient != null)
        {
            ingredients.Remove(ingredient.ingredientComponent);
        }
    }

    // When an ingredient appears, it gets added to the list and tries to spawn a rat
    private void OnIngredientCreated(object obj)
    {
        // Only receives FoodEvents
        IngredientEventData ingredient = (IngredientEventData)obj;

        if (ingredient != null)
        {
            ingredients.Add(ingredient.ingredientComponent);
        }

        TrySpawningRat();
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}

[System.Serializable]
public struct RatSpawnData
{
    public GameObject ratPrefab;
    [Range(0f, 1f)] public float spawnChance;
}