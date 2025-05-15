using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField]
    private KitchenObjectSO kitchenObjectSO;

    private IKichentObjectParent kitchentObjectParent;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKichentObjectParent kichentObjectParent)
    {
        if (this.kitchentObjectParent != null)
        {
            this.kitchentObjectParent.ClearKitchenObject();
        }

        this.kitchentObjectParent = kichentObjectParent;

        if (kitchentObjectParent.HasKitchenObject())
        {
            Debug.LogError("Counter already has KitchenObject!");
        }

        kitchentObjectParent.SetKitchenObject(this);

        transform.parent = kitchentObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKichentObjectParent GetKitchenObjectParent()
    {
        return kitchentObjectParent;
    }
}
