using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data",menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")]
    public int maxHealth;
    public int currentHealth;
    public int baseDefence;
    public int currentDefence;

    [Header("Kill")]
    //怪物被击杀player获得的经验
    public int killExp;

    [Header("Level")]
    public int currentLevel;
    public int maxLevel;
    public int baseExp;
    public int currentExp;
    public float levelBuff;

    public float LevelMultiplier
    {
        get { return 1 + (currentLevel + 1) * levelBuff; }
    }
    public void UpdateExp(int kExp)
    {
        currentExp += kExp;

        if (currentExp >= baseExp)
            LevelUp();
    }

    private void LevelUp()
    {
        //升级提升数据
        currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);

        //所需经验提升
        baseExp += (int)(baseExp * LevelMultiplier);

        maxHealth += (int)(maxHealth * levelBuff);
        currentHealth = maxHealth;

        Debug.Log("LEVEL UP!" + currentLevel + "Max Health:" + maxHealth);
    }
}
