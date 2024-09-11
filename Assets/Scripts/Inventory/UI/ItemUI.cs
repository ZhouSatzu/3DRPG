using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image icon = null;
    public Text amount = null;

    //������ʾ��item������Inventory ���� ����� װ����
    public InventoryData_SO ItemSInventoryData {  get; set; }
    //itemUI��û�м�¼itemData��Ҫ�õ�itemData��Ҫͨ��currentItemUI.ItemSInventoryData.items[currentItemUI.Index].itemData
    public int Index { get; set; } = -1;

    public void SetupItemUI(ItemData_SO item,int itemAomunt)
    {
        if(itemAomunt == 0)
        {
            ItemSInventoryData.items[Index].itemData = null;
            icon.gameObject.SetActive(false);
            return;
        }   
        if(item != null)
        {
            icon.sprite = item.itemIcon;
            amount.text = itemAomunt.ToString();
            icon.gameObject.SetActive(true);
        }
        else
            icon.gameObject.SetActive(false) ;
    }

    public ItemData_SO GetItem()
    {
        return ItemSInventoryData.items[Index].itemData;
    }
}
