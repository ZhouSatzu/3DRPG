using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    bool isOpen = false;

    //用来保存拖拽物品的原始数据
    public class DragData
    {
        public SlotHolder originalHolder;
        public RectTransform originalParent;
    }
        
    [Header("Inventory Data")]
    public InventoryData_SO bagData;
    public InventoryData_SO actionData;
    public InventoryData_SO equipmentData;

    [Header("Containers")]
    public ContainerUI bagContainerUI;
    public ContainerUI actionContainerUI;
    public ContainerUI equipmentContainerUI;

    [Header("Drag Canvas")]
    public Canvas dragCanvas;
    public DragData currentDrag;

    [Header("UI Panel")]
    public GameObject bagPanel;
    public GameObject characterPanel;

    [Header("Tooltip")]
    public ItemTooltip itemTooltip;

    private void Start()
    {
        bagContainerUI.RefreshUI();
        actionContainerUI.RefreshUI();
        equipmentContainerUI.RefreshUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isOpen = !isOpen;
            bagPanel.SetActive(isOpen);
            characterPanel.SetActive(isOpen); 
        }
    }

    #region 检查拖拽物品是否在每一个slot范围内
    public bool CheckInBagUI(Vector3 position)
    {
        for(int i = 0;i<bagContainerUI.slotHolders.Length;i++)
        {
            RectTransform t = bagContainerUI.slotHolders[i].transform as RectTransform;
            //此 RectTransform 是否包含从给定摄像机观察到的屏幕点
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
                return true;
        }
        return false;
    }

    public bool CheckInActionUI(Vector3 position)
    {
        for (int i = 0; i < actionContainerUI.slotHolders.Length; i++)
        {
            RectTransform t = actionContainerUI.slotHolders[i].transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
                return true;
        }
        return false;
    }

    public bool CheckInEquipmentUI(Vector3 position)
    {
        for (int i = 0; i < equipmentContainerUI.slotHolders.Length; i++)
        {
            RectTransform t = equipmentContainerUI.slotHolders[i].transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
                return true;
        }
        return false;
    }
    #endregion
}
