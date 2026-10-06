using Pedroca2005BR.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RatBehaviour : MonoBehaviour
{
    [Header("Settings")]
    public Transform home;  // May be set on spawn
    [SerializeField] float grabDistance = 0.1f; // The distance at which the rat can grab the food

    NavMeshAgent agent;

    // Runtime variables
    bool hasFood = false;
    Transform desiredFood;
    private RatSpawner spawner;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        spawner = GetComponentInParent<RatSpawner>();
    }

    // This class subscribe to events to control if the food it wants is still available or not. If not, it may go after another food.
    private void OnEnable()
    {
        EventManager.Subscribe(EventConstantNames.INGREDIENT_CONSUMED, OnIngredientVanished);
        EventManager.Subscribe(EventConstantNames.INGREDIENT_STOLEN, OnIngredientVanished);
        EventManager.Subscribe(EventConstantNames.INGREDIENT_PICKED_UP, OnIngredientVanished);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(EventConstantNames.INGREDIENT_CONSUMED, OnIngredientVanished);
        EventManager.Unsubscribe(EventConstantNames.INGREDIENT_STOLEN, OnIngredientVanished);
        EventManager.Unsubscribe(EventConstantNames.INGREDIENT_PICKED_UP, OnIngredientVanished);
    }


    // Mini State Machine for the rats behaviour
    private void Update()
    {
        if (hasFood)
        {
            GoHome();
        }
        else
        {
            GoAfterFood();
        }

        // If there's no food to go after, just go home
        if (desiredFood == null)
        {
            GoHome();
        }
    }


    private void GrabFood()
    {
        //Debug.Log($"Grabbed {desiredFood.GetComponent<IngredientComponent>().data.baseName}!");

        if(desiredFood.GetComponent<IngredientComponent>().TryGrabbing(this))
        {
            hasFood = true;
            desiredFood.SetParent(transform);
            // An offset may be added here to place the food in rats mouth or something like that, for now it just stays in the center of the rat
            desiredFood.transform.position = transform.position;
        }
    }

    // This method is called by the spawner to give the rat a new objective, if it doesn't have one already
    public void TryGiveObjectives(Transform objective)
    {
        if (desiredFood == null)
        {
            agent.stoppingDistance = 0f;
            desiredFood = objective;
        }
    }

    private void GoHome()
    {
        agent.SetDestination(home.position);

        // When the rat gets home, it drops the food and goes back to the spawner to get a new objective. If it can't, it will "deactivate" itself and wait for a new objective to be given.
        if (Vector2.Distance(home.position, transform.position) < spawner.spawnRadius)
        {
            if (hasFood && desiredFood != null)
            {
                var ingComp = desiredFood.GetComponent<IngredientComponent>();

                ingComp.enabled = false;
                EventManager.TriggerEvent(EventConstantNames.INGREDIENT_STOLEN, new IngredientEventData(ingComp.data, ingComp, this));
                Destroy(desiredFood.gameObject);
                hasFood = false;
                desiredFood = null;
            }
            else
            {
                agent.stoppingDistance = 1f;
            }
        }

        
    }

    private void GoAfterFood()
    {
        if (desiredFood != null)
        {
            float distance = Vector2.Distance(transform.position, desiredFood.position);

            if (distance < grabDistance)
            {
                GrabFood();
            }
            else
            {
                agent.SetDestination(desiredFood.position);
            }
        }
        else
        {
            spawner.GiveObjectives();
        }
        
    }

    // Called to cleanly kill the rat, if it has food, it will drop it and notify the ingredient that it was killed by a rat.
    public void Die()
    {
        if (hasFood)
        {
            desiredFood.GetComponent<IngredientComponent>().RatKilled();
            desiredFood.SetParent(null);
        }

        // TODO: Play Death Animation
        Destroy(gameObject);
    }



   

    // When an ingredient vanishes for any reason, it gets removed from the list
    private void OnIngredientVanished(object obj)
    {
        IngredientEventData ingredient = (IngredientEventData)obj;

        // If the origin is not itself and the component is correct
        if (ingredient != null && desiredFood == ingredient.ingredientComponent.transform && ingredient.origin != this)
        {
            desiredFood = null;
        }
    }
}
