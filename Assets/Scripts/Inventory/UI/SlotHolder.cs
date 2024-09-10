using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//背包，武器，装备，快捷栏
public enum SlotType { Bag,Weapon,Armor,Action}
public class SlotHolder : MonoBehaviour
{
    public SlotType slotType;   
    public ItemUI itemUI;

    public void UpdateItem()
    {
        switch(slotType)
        {
            case SlotType.Bag:
                itemUI.ItemSInventoryData = InventoryManager.Instance.bagData;
                break;
            case SlotType.Weapon: 
                break;
            case SlotType.Armor:
                break;
            case SlotType.Action:
                break;
        }

        var item = itemUI.ItemSInventoryData.items[itemUI.Index];
        itemUI.SetupItemUI(item.itemData, item.itemAmount);
    }
}
