using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class playerInventory : MonoBehaviour
{
   
    //Pseudo-Inventory System 
    [SerializeField] public List<ItemScriptableObject> Inventory = new List<ItemScriptableObject>();


    //Self-Explanatory, Adds said item to player Inventory 
    public void AddItem(ItemScriptableObject item)  
    {
        Inventory.Add(item);
        if (Inventory.Contains(item))
        {
            Debug.Log($"Added Item {item} succesfully!");
        }
    }


    //Self-Explanatory, Removes said item from player Inventory 
    public void RemoveItem(ItemScriptableObject item)
    {
        if (Inventory.Contains(item))
        {
            Inventory.Remove(item);
        }
    }
}
