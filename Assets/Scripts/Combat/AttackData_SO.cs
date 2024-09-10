using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack",menuName ="Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    public float attackRange;
    //远程攻击距离
    public float skillRange;
    //冷却时间
    public float coolDown;
    public int minDamage;
    public int maxDamage;
    //暴击加成百分比
    public float criticalMultiplier;
    //暴击率
    public float criticalChance;

    public void ApplyWeaponData(AttackData_SO weapon)
    {
        attackRange = weapon.attackRange;
        skillRange = weapon.skillRange;
        coolDown = weapon.coolDown;
        
        minDamage = weapon.minDamage;
        maxDamage = weapon.maxDamage;
        
        criticalMultiplier = weapon.criticalMultiplier;
        criticalChance = weapon.criticalChance;
    }
}
