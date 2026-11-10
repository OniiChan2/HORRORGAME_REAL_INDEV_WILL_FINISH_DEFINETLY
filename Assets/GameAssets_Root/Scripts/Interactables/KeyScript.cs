using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public ItemScriptableObject RedKey_Data;
    [SerializeField] private playerInventory inventory;

    public void pickedUp()
    {
        inventory.AddItem(RedKey_Data);
        Debug.Log($"Added Item: {RedKey_Data} To Inventory");
        if (!gameObject.CompareTag("Holdable"))  Destroy(gameObject);
    }
}
