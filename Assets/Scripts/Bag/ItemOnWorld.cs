using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public item thisitem;
    public Inventory playerInventory;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AddNewItem();
            Destroy(gameObject);
        }
    }

    public void AddNewItem()
    {
        if (!playerInventory.itemList.Contains(thisitem))
        {
            //playerInventory.itemList.Add(thisitem);
            //InventoryManger.CreateNewItem(thisitem);
            for(int i = 0; i < playerInventory.itemList.Count; i++)
            {
                if (playerInventory.itemList[i] == null)
                {
                    playerInventory.itemList[i] = thisitem;
                    break;
                }
            }
        }
        else
        {
            thisitem.itemHeld += 1;
        }

        InventoryManger.RefreshItem();
    }

}
