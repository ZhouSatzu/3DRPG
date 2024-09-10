using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Inventory",menuName ="Inventory/Inventory Data")]
public class InventoryData_SO : ScriptableObject
{
    public List<InventoryItem> items = new List<InventoryItem>();

    public void AddItem(ItemData_SO newItemData ,int amount)
    {

        //���еı������Ƿ��������Ʒ
        bool found = false;
        
        //����ɶѵ��ұ������Ѿ�����Ҫ��ӵ���Ʒ���򽫱����и��������������
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

        //������û�и���Ʒ�����ҵ��ǰ�ĸ�����Ӹ���Ʒ
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
