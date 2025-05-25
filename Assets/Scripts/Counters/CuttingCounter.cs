using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    // Event for progress bar
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    // Event for animation
    public event EventHandler OnCut;

    [SerializeField]
    private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // There is no Kitchen Object here
            if (player.HasKitchenObject())
            {
                // Player is carrying something
                if (HasRecipewithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // Player carry something that can be cut then they can place here
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    // Init cutting progress
                    cuttingProgress = 0;

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(
                        GetKitchenObject().GetKitchenObjectSO()
                    );

                    // Trigger event
                    OnProgressChanged?.Invoke(
                        this,
                        new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized =
                                (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax,
                        }
                    );
                }
            }
            else
            {
                // Player has nothing on hand
            }
        }
        else
        {
            // There is a kitchen object here
            if (player.HasKitchenObject())
            {
                // Player is carrying something

                if (
                    player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)
                )
                {
                    // User is holding a plate
                    // Find the food then remove it so later on can add the food on top of the plate
                    if (
                        plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())
                    )
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
            else
            {
                // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        // Only cut the object is valid and not cut yet
        if (HasKitchenObject() && HasRecipewithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            cuttingProgress++;

            // Trigger event for animation
            OnCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(
                GetKitchenObject().GetKitchenObjectSO()
            );

            // Trigger event for progress bar
            OnProgressChanged?.Invoke(
                this,
                new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized =
                        (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax,
                }
            );

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSO outputKitchenSO = GetOutputForInput(
                    GetKitchenObject().GetKitchenObjectSO()
                );

                // There is a KitchenObject here
                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchenSO, this);
            }
        }
    }

    private bool HasRecipewithInput(KitchenObjectSO inputKitchenSO)
    {
        return GetCuttingRecipeSOWithInput(inputKitchenSO) != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenSO);

        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
