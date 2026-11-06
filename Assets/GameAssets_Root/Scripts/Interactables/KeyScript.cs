using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public string Key_ItemName;
    [SerializeField] private playerInventory inventory;

    private void Start()
    {
        Key_ItemName = gameObject.name;
    }

    public void pickedUp()
    {
        inventory.AddItem(Key_ItemName);
        Debug.Log($"Added Item: {Key_ItemName} To Inventory");
        Destroy(gameObject);
    }
}
