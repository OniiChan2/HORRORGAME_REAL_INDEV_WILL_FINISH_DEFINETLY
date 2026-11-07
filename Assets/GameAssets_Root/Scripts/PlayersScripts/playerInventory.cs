using System.Collections.Generic;
using UnityEngine;

public class playerInventory : MonoBehaviour
{
   
    //Pseudo-Inventory System 
    [SerializeField] public List<string> Inventory = new List<string>();


    //Self-Explanatory, Adds said item to player Inventory 
    public void AddItem(string item)
    {
        Inventory.Add(item);
        if (Inventory.Contains(item))
        {
            Debug.Log($"Added Item {item} succesfully!");
        }
    }


    //Self-Explanatory, Removes said item from player Inventory 
    public void RemoveItem(string item)
    {
        if (Inventory.Contains(item))
        {
            Inventory.Remove(item);
        }
    }
}
