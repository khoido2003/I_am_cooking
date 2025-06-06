using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    public static DeliveryManager Instance { get; private set; }

    [SerializeField]
    private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;

    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;
    private int successfulRecipeAmount;

    private void Awake()
    {
        waitingRecipeSOList = new List<RecipeSO>();
        Instance = this;
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;

        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (
                KichenGameManager.Instance.IsGamePlaying()
                && waitingRecipeSOList.Count < waitingRecipeMax
            )
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[
                    UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)
                ];

                waitingRecipeSOList.Add(waitingRecipeSO);

                // Trigger event when Recipe spawned
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (
                waitingRecipeSO.kitchenObjectSOList.Count
                == plateKitchenObject.GetKitchenObjectSOList().Count
            )
            {
                bool plateContentMatchesRecipe = true;

                // Has the same number of ingredients
                foreach (
                    KitchenObjectSO recipeKitchenObjectSo in waitingRecipeSO.kitchenObjectSOList
                )
                {
                    // Cycling through all the ingredients in the recipe
                    bool ingredientFound = false;
                    foreach (
                        KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()
                    )
                    {
                        // Cycling through all ingredients in the plate

                        if (plateKitchenObjectSO == recipeKitchenObjectSo)
                        {
                            // Ingredient match
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        // This recipe ingredient was not found on the plate
                        plateContentMatchesRecipe = false;
                    }
                }

                if (plateContentMatchesRecipe)
                {
                    successfulRecipeAmount++;

                    // player deliver the correct recipe
                    Debug.Log("Player deliver the correct recipe");
                    waitingRecipeSOList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);

                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);

                    return;
                }
            }
        }

        // No matches found!
        // Player deliver wrong recipe
        // Debug.Log("Player deliver the wrong recipe");

        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulRecipeAmount()
    {
        return successfulRecipeAmount;
    }
}
