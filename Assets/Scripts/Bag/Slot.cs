using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public item slotItem;
    public Image slotImage;
    public Text slotNum;
    public string slotInfo;

    public GameObject itemInSlot;

    public void ItemOnClicked()
    {
        InventoryManger.UpdateItemInfo(slotInfo);
    }

    public void SetupSlot(item Item)
    {
        if (Item == null)
        {
            itemInSlot.SetActive(false);
            return;
        }

        slotImage.sprite = Item.itemImage;
        slotNum.text = Item.itemHeld.ToString();
        slotInfo = Item.itemInfo;
    }


}
