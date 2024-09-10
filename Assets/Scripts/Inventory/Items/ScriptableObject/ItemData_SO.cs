using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Usable,Weapon,Armor}

[CreateAssetMenu(fileName ="New Item",menuName ="Inventory/Item Data")]
public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;
    //��ǰitem�µ�һ������������ƻ��һ��ʰȡ������5��
    public int itemAmount;
    //�ж��Ƿ���Զѵ�
    public bool stackable;

    [TextArea]
    public string description = "";

    [Header("Weapon")]
    public GameObject weaponPrefab;
    public AttackData_SO weaponData;
}
