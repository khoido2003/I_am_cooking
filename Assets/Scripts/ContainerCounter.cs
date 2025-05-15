using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField]
    private KitchenObjectSO kitchenObjectSO;

    public event EventHandler OnPlayerGrabbedObject;

    public override void Interact(Player player)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);

        kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);

        // Fire off event
        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
    }
}
