using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField]
    private KitchenObjectSO kitchenObjectSO;

    private IKitchentObjectParent kitchentObjectParent;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchentObjectParent kichentObjectParent)
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

    public IKitchentObjectParent GetKitchenObjectParent()
    {
        return kitchentObjectParent;
    }

    public void DestroySelf()
    {
        kitchentObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public static KitchenObject SpawnKitchenObject(
        KitchenObjectSO kitchenObjectSO,
        IKitchentObjectParent kitchentObjectParent
    )
    {
        Transform KitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);

        KitchenObject kitchenObject = KitchenObjectTransform.GetComponent<KitchenObject>();

        kitchenObject.SetKitchenObjectParent(kitchentObjectParent);

        return kitchenObject;
    }
}
