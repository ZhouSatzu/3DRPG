using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemUI))]
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //��ǰ��ק������
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
        //��¼ԭʼ����
        InventoryManager.Instance.currentDrag = new InventoryManager.DragData();
        InventoryManager.Instance.currentDrag.originalHolder = GetComponentInParent<SlotHolder>();
        //RectTransform��������������ק�����ж�����һ��ʱ�ܹ���¼��ǰ��һ����Χ
        InventoryManager.Instance.currentDrag.originalParent = (RectTransform)transform.parent;
        //����קitem�ĸ������ó�dragcanvas���ڵ�
        transform.SetParent(InventoryManager.Instance.dragCanvas.transform,true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //��������ƶ�λ��
        transform.position = eventData.position;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("111");
        //�Ƿ�ָ��UI����
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
                    //һ��û���ҵ�slotholder��ԭ������Ϊ��������item����������֮�����ҵ�slotholder
                    targetHolder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                }

                //������Ʒ ��������
                switch (targetHolder.slotType)
                {
                    case SlotType.Bag:
                        SwapItem();
                        break;
                    case SlotType.Action:
                        //ֻ�п�ʹ�õ���Ʒ�ܷ�������
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

        //����ԭ���ĸ���
        transform.SetParent(InventoryManager.Instance.currentDrag.originalParent);
        Debug.Log(InventoryManager.Instance.currentDrag.originalParent);
        //��֤itemλ�ò�ƫ��
        RectTransform t = transform as RectTransform;
        t.offsetMax = -Vector2.one * 5;
        t.offsetMin = Vector2.one * 5;
    }

    public void SwapItem()
    {
        var targetItem = targetHolder.itemUI.ItemSInventoryData.items[targetHolder.itemUI.Index];
        var tempItem = currentHolder.itemUI.ItemSInventoryData.items[currentHolder.itemUI.Index];

        bool isSameItem = tempItem.itemData == targetItem.itemData;

        //����ͬ�ҿɶѵ�
        if(isSameItem && targetItem.itemData.stackable)
        {
            targetItem.itemAmount += tempItem.itemAmount;
            tempItem.itemData = null;
            tempItem.itemAmount = 0;
        }
        else
        {
            currentHolder.itemUI.ItemSInventoryData.items[currentHolder.itemUI.Index] = targetItem;
            //ʹ��Ŀ���Ӧ����ʵ������λ�ý���ת��
            targetHolder.itemUI.ItemSInventoryData.items[targetHolder.itemUI.Index] = tempItem;
        }
    }
}
