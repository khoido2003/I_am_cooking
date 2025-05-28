using UnityEngine;

public class DeliverCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                // Only accept plates
                player.GetKitchenObject().DestroySelf();
            }
        }
    }
}
