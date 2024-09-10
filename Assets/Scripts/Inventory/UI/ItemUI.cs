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
    public int Index { get; set; } = -1;

    public void SetupItemUI(ItemData_SO item,int itemAomunt)
    {
        if(item != null)
        {
            icon.sprite = item.itemIcon;
            amount.text = itemAomunt.ToString();
            icon.gameObject.SetActive(true);
        }
        else
            icon.gameObject.SetActive(false) ;
    }
}
