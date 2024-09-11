using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemUI))]
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //当前拖拽的物体
    ItemUI currentItemUI;
    SlotHolder currentHolder;
    SlotHolder targetHolder;

    private void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
        currentHolder = GetComponentInParent<SlotHolder>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //记录原始数据
        InventoryManager.Instance.currentDrag = new InventoryManager.DragData();
        InventoryManager.Instance.currentDrag.originalHolder = GetComponentInParent<SlotHolder>();
        //RectTransform的优势是在做拖拽交换判断在哪一格时能够记录当前的一个范围
        InventoryManager.Instance.currentDrag.originalParent = (RectTransform)transform.parent;
        //把拖拽item的父级设置成dragcanvas防遮挡
        transform.SetParent(InventoryManager.Instance.dragCanvas.transform,true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //跟随鼠标移动位置
        transform.position = eventData.position;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("111");
        //是否指向UI物体
        if(EventSystem.current.IsPointerOverGameObject())
        {
            if (InventoryManager.Instance.CheckInBagUI(eventData.position) || InventoryManager.Instance.CheckInActionUI(eventData.position) || InventoryManager.Instance.CheckInEquipmentUI(eventData.position))
            {
                if (eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
                {
                    targetHolder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                }
                else
                {
                    //一般没有找到slotholder的原因是因为格子上有item。往父级找之后能找到slotholder
                    targetHolder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                }

                //放下物品 交换数据
                switch (targetHolder.slotType)
                {
                    case SlotType.Bag:
                        SwapItem();
                        break;
                    case SlotType.Action:
                        //只有可使用的物品能放入快捷栏
                        if (currentItemUI.ItemSInventoryData.items[currentItemUI.Index].itemData.itemType == ItemType.Usable)
                            SwapItem();
                        break;
                    case SlotType.Weapon:
                        if (currentItemUI.ItemSInventoryData.items[currentItemUI.Index].itemData.itemType == ItemType.Weapon)
                            SwapItem();
                        break;
                    case SlotType.Armor:
                        if (currentItemUI.ItemSInventoryData.items[currentItemUI.Index].itemData.itemType == ItemType.Armor)
                            SwapItem();
                        break;
                }

                currentHolder.UpdateItem();
                targetHolder.UpdateItem();
            }
        }

        //返回原本的父级
        transform.SetParent(InventoryManager.Instance.currentDrag.originalParent);
        Debug.Log(InventoryManager.Instance.currentDrag.originalParent);
        //保证item位置不偏移
        RectTransform t = transform as RectTransform;
        t.offsetMax = -Vector2.one * 5;
        t.offsetMin = Vector2.one * 5;
    }

    public void SwapItem()
    {
        var targetItem = targetHolder.itemUI.ItemSInventoryData.items[targetHolder.itemUI.Index];
        var tempItem = currentHolder.itemUI.ItemSInventoryData.items[currentHolder.itemUI.Index];

        bool isSameItem = tempItem.itemData == targetItem.itemData;

        //若相同且可堆叠
        if(isSameItem && targetItem.itemData.stackable)
        {
            targetItem.itemAmount += tempItem.itemAmount;
            tempItem.itemData = null;
            tempItem.itemAmount = 0;
        }
        else
        {
            currentHolder.itemUI.ItemSInventoryData.items[currentHolder.itemUI.Index] = targetItem;
            //使用目标对应的真实的数据位置进行转换
            targetHolder.itemUI.ItemSInventoryData.items[targetHolder.itemUI.Index] = tempItem;
        }
    }
}
