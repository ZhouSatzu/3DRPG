using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//背包，武器，装备，快捷栏
public enum SlotType { Bag,Weapon,Armor,Action}
public class SlotHolder : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public SlotType slotType;   
    public ItemUI itemUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.clickCount %2 == 0)
        {
            UseItem();
        }
    }

    public void UseItem()
    {
        if(itemUI.GetItem() != null)
        {
            //判断item类型
            if (itemUI.ItemSInventoryData.items[itemUI.Index].itemData.itemType == ItemType.Usable && itemUI.ItemSInventoryData.items[itemUI.Index].itemAmount > 0)
            {
                GameManager.Instance.playerStats.ApplyHealth(itemUI.GetItem().useableItemData.healthRecoverPoint);

                itemUI.ItemSInventoryData.items[itemUI.Index].itemAmount -= 1;
            }
        }
        
        UpdateItem();
    }


    public void UpdateItem()
    {
        switch(slotType)
        {
            case SlotType.Bag:
                itemUI.ItemSInventoryData = InventoryManager.Instance.bagData;
                break;
            case SlotType.Weapon:
                itemUI.ItemSInventoryData = InventoryManager.Instance.equipmentData;
                if (itemUI.ItemSInventoryData.items[itemUI.Index].itemData != null)
                {
                    GameManager.Instance.playerStats.ChangeWeapon(itemUI.ItemSInventoryData.items[itemUI.Index].itemData);
                }
                else
                {
                    GameManager.Instance.playerStats.UnEquipWeapon();
                }
                break;
            case SlotType.Armor:
                itemUI.ItemSInventoryData = InventoryManager.Instance.equipmentData;
                break;
            case SlotType.Action:
                itemUI.ItemSInventoryData = InventoryManager.Instance.actionData;
                break;
        }

        var item = itemUI.ItemSInventoryData.items[itemUI.Index];
        itemUI.SetupItemUI(item.itemData, item.itemAmount);
    }

    //鼠标悬停在物品上显示信息
    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.itemTooltip.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryManager.Instance.itemTooltip.gameObject.SetActive(false);
        if (itemUI.GetItem())
        {
            Debug.Log("111");
            InventoryManager.Instance.itemTooltip.SetupTooltip(itemUI.GetItem());
            InventoryManager.Instance.itemTooltip.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        InventoryManager.Instance.itemTooltip.gameObject.SetActive(false);
    }
}

