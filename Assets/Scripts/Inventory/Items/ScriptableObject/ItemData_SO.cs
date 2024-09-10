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
    //当前item下的一组数量：例如苹果一次拾取可能是5个
    public int itemAmount;
    //判断是否可以堆叠
    public bool stackable;

    [TextArea]
    public string description = "";

    [Header("Weapon")]
    public GameObject weaponPrefab;
    public AttackData_SO weaponData;
}
