 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Usable Data",menuName ="Inventory/UsableItem Data")]
public class UseableItemData_SO : ScriptableObject
{
    //任何想改变的数据
    public int healthRecoverPoint;
    public int expGrowPoint;
}
