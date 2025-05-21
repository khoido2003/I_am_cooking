using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField]
    private KitchenObjectSO kitchenObjectSO;

    public event EventHandler OnPlayerGrabbedObject;

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            // Player not carrying anything then able to pick something new
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

            // Fire off event
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
