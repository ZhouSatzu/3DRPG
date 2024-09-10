using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Inventory",menuName ="Inventory/Inventory Data")]
public class InventoryData_SO : ScriptableObject
{
    public List<InventoryItem> items = new List<InventoryItem>();

    public void AddItem(ItemData_SO newItemData ,int amount)
    {

        //现有的背包中是否有这个物品
        bool found = false;
        
        //如果可堆叠且背包中已经含有要添加的物品，则将背包中该物体的数量增加
        if(newItemData.stackable)
        {
            foreach(var item in items)
            {
                if(item.itemData = newItemData)
                {
                    item.itemAmount += amount;
                    found = true;
                    break;
                }
            }
        }

        //背包中没有该物品，则找到最靠前的格子添加该物品
        if(!found)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if(items[i].itemData == null)
                {
                    Debug.Log("111");
                    items[i].itemData = newItemData;
                    items[i].itemAmount = amount;
                    break;
                }
            }
        }
    }
}

[System.Serializable]
public class InventoryItem
{
    public ItemData_SO itemData;
    public int itemAmount;
}
