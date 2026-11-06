using System.Collections.Generic;
using UnityEngine;

public class playerInventory : MonoBehaviour
{
   

    [SerializeField] public List<string> Inventory = new List<string>();


    public void AddItem(string item)
    {
        Inventory.Add(item);
        if (Inventory.Contains(item))
        {
            Debug.Log($"Added Item {item} succesfully!");
        }
    }

    public void RemoveItem(string item)
    {
        if (Inventory.Contains(item))
        {
            Inventory.Remove(item);
        }
    }

    public void checkItem()
    {

    }
}
