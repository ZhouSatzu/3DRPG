using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    bool isOpen = false;

    //����������ק��Ʒ��ԭʼ����
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

    #region �����ק��Ʒ�Ƿ���ÿһ��slot��Χ��
    public bool CheckInBagUI(Vector3 position)
    {
        for(int i = 0;i<bagContainerUI.slotHolders.Length;i++)
        {
            RectTransform t = bagContainerUI.slotHolders[i].transform as RectTransform;
            //�� RectTransform �Ƿ�����Ӹ���������۲쵽����Ļ��
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
