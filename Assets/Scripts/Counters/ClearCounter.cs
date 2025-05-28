using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField]
    private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        /*if (HasKitchenObject())*/
        /*{*/
        /*    Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);*/
        /**/
        /*    kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);*/
        /*}*/
        /*else*/
        /*{*/
        /*    // Give the object to player*/
        /*    kitchenObject.SetKitchenObjectParent(player);*/
        /*}*/

        if (!HasKitchenObject())
        {
            // There is no Kitchen Object here
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
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
                else
                {
                    // Player is not holding plate but something else
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // Counter is holding a plate so user can add food on the plate
                        if (
                            plateKitchenObject.TryAddIngredient(
                                player.GetKitchenObject().GetKitchenObjectSO()
                            )
                        )
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
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
}
