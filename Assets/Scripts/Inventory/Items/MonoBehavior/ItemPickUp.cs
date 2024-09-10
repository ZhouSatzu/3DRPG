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
            //TODO:��ӵ����ﱳ��
            InventoryManager.Instance.bagData.AddItem(itemData, itemData.itemAmount);
            InventoryManager.Instance.bagContainerUI.RefreshUI();
            //װ������
            //GameManager.Instance.playerStats.EquipWeapon(itemData);
            //ɾ��onworld
            Destroy(gameObject);
        }
    }
}
