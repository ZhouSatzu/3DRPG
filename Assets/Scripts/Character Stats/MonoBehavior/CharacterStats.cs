using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public event Action<int, int> UpdateHealthBar;
    //模版数据
    public CharacterData_SO templateData;

    public CharacterData_SO characterData;

    public AttackData_SO attackData;

    public AttackData_SO baseAttackData;

    [HideInInspector]public bool isCritical;

    private RuntimeAnimatorController baseAnimator;

    [Header("Weapon")]
    public Transform weaponSlot;

    private void Awake()
    {
        //生成一个模版data的副本
        if(templateData != null)
        {
            characterData = Instantiate(templateData);
        }

        //使游戏开始时的攻击力等于初始的攻击力
        if(baseAttackData != null)
        {
            attackData = Instantiate(baseAttackData);
        }

        baseAnimator = GetComponent<Animator>().runtimeAnimatorController;
    }

    #region Read From Data_SO
    //属性（Property）的使用.属性定义：MaxHealth 是一个属性，而不是一个字段。属性允许你在获取或设置值时添加额外的逻辑。
    public int MaxHealth
    {
        get
        {
            if (characterData != null)
                return characterData.maxHealth;
            else return 0;
        }
        set
        {
            characterData.maxHealth = value;
        }
    }

    public int CurrentHealth
    {
        get
        {
            if (characterData != null)
                return characterData.currentHealth;
            else return 0;
        }
        set
        {
            characterData.currentHealth = value;
        }
    }

    public int BaseDefence
    {
        get
        {
            if (characterData != null)
                return characterData.baseDefence;
            else return 0;
        }
        set
        {
            characterData.baseDefence = value;
        }
    }

    public int CurrentDefence
    {
        get
        {
            if (characterData != null)
                return characterData.currentDefence;
            else return 0;
        }
        set
        {
            characterData.currentDefence = value;
        }
    }
    #endregion

    #region Character Combat
    //承受伤害
    public void TakeDamage(CharacterStats attacker,CharacterStats defender)
    {
        int damage = Mathf.Max(attacker.currentDamage() - defender.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        //如果暴击受伤者触发受伤动画
        if(attacker.isCritical)
        {
            defender.GetComponent<Animator>().SetTrigger("Hurt");
        }

        //更新血条
        UpdateHealthBar?.Invoke(CurrentHealth,MaxHealth);

        //更新经验值
        if (CurrentHealth <= 0)
            attacker.characterData.UpdateExp(characterData.killExp);
    }

    //函数重载，用于rock造成伤害
    public void TakeDamage(int damage,CharacterStats defender)
    {
        int currentdamage = Mathf.Max(damage - defender.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        //更新血条
        UpdateHealthBar?.Invoke(CurrentHealth, MaxHealth);

        
        if (CurrentHealth <= 0)
            GameManager.Instance.playerStats.characterData.UpdateExp(characterData.killExp);
    }

    private int currentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(attackData.minDamage,attackData.maxDamage);
        //暴击
        if(isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
        }

        return (int)coreDamage;
    }
    #endregion

    #region Equipment
    
    public void EquipWeapon(ItemData_SO weapon)
    {
        Debug.Log("nooooooo");
        if(weapon.weaponPrefab != null)
        {
            Instantiate(weapon.weaponPrefab, weaponSlot);
        }
        //切换攻击属性
        //attackData.ApplyWeaponData(weapon.weaponData);
        attackData = weapon.weaponData; //自助添加

        //切换动画
        GetComponent<Animator>().runtimeAnimatorController = weapon.weaponAnimator;
    }

    public void UnEquipWeapon()
    {
        if (weaponSlot.transform.childCount != 0)
        {
            for (int i = 0; i < weaponSlot.transform.childCount; i++)
            {
                //摧毁手部的武器物体
                Destroy(weaponSlot.transform.GetChild(i).gameObject);
            }
        }

        //切换动画
        GetComponent<Animator>().runtimeAnimatorController = baseAnimator;

        attackData = baseAttackData;
    }

    public void ChangeWeapon(ItemData_SO weapon)
    {
        UnEquipWeapon();
        EquipWeapon(weapon);
    }

    #endregion

    #region Apply Data Change
    public void ApplyHealth(int recoverAmount)
    {
        if (CurrentHealth + recoverAmount <= MaxHealth)
            CurrentHealth += recoverAmount;
        else
            CurrentHealth = MaxHealth;
    }

    #endregion
}
