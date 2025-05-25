using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngerdientAddedEventArgs> OnIngerdientAdded;

    public class OnIngerdientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    private List<KitchenObjectSO> kitchenObjectsSOList;

    [SerializeField]
    private List<KitchenObjectSO> validKitchenObjectSOList;

    private void Awake()
    {
        kitchenObjectsSOList = new List<KitchenObjectSO>();
    }

    // Add food to the plate - only one food per plate
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // Not a valid ingredient
            return false;
        }

        if (kitchenObjectsSOList.Contains(kitchenObjectSO))
        {
            // Already has this type
            return false;
        }
        else
        {
            kitchenObjectsSOList.Add(kitchenObjectSO);

            OnIngerdientAdded?.Invoke(
                this,
                new OnIngerdientAddedEventArgs { kitchenObjectSO = kitchenObjectSO }
            );

            return true;
        }
    }
}
