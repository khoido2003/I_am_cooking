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
            }
            else
            {
                // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);

            }
        }
    }
}
