using System;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    // Event for progress bar
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;

    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }

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
                        new OnProgressChangedEventArgs
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
                new OnProgressChangedEventArgs
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
