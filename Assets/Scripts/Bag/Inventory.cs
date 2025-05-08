using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Scripts/Bag/New Inventory")]
public class Inventory : ScriptableObject
{
    public List<item> itemList = new List<item>();
}
