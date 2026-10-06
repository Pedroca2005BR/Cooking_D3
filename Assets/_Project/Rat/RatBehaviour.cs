using Pedroca2005BR.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RatBehaviour : MonoBehaviour
{
    [Header("Settings")]
    public Transform home;
    [SerializeField] float grabDistance = 0.1f;

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
        Debug.Log($"Grabbed {desiredFood.GetComponent<IngredientComponent>().data.baseName}!");

        //desiredFood.GetComponent<DraggableComponent>().enabled = false;

        if(desiredFood.GetComponent<IngredientComponent>().TryGrabbing(this))
        {
            hasFood = true;
            desiredFood.SetParent(transform);
            desiredFood.transform.position = transform.position;
        }
    }

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
