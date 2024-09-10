using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData_SO itemData;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //TODO:添加到人物背包
            InventoryManager.Instance.bagData.AddItem(itemData, itemData.itemAmount);
            InventoryManager.Instance.bagContainerUI.RefreshUI();
            //装备武器
            //GameManager.Instance.playerStats.EquipWeapon(itemData);
            //删除onworld
            Destroy(gameObject);
        }
    }
}
