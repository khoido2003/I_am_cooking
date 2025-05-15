using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField]
    private KitchenObjectSO kitchenObjectSO;

    private void Update() { }

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
    }
}
