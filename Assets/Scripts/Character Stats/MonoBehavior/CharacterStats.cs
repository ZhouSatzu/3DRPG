using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public event Action<int, int> UpdateHealthBar;
    //ģ������
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
        //����һ��ģ��data�ĸ���
        if(templateData != null)
        {
            characterData = Instantiate(templateData);
        }

        //ʹ��Ϸ��ʼʱ�Ĺ��������ڳ�ʼ�Ĺ�����
        if(baseAttackData != null)
        {
            attackData = Instantiate(baseAttackData);
        }

        baseAnimator = GetComponent<Animator>().runtimeAnimatorController;
    }

    #region Read From Data_SO
    //���ԣ�Property����ʹ��.���Զ��壺MaxHealth ��һ�����ԣ�������һ���ֶΡ������������ڻ�ȡ������ֵʱ��Ӷ�����߼���
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
    //�����˺�
    public void TakeDamage(CharacterStats attacker,CharacterStats defender)
    {
        int damage = Mathf.Max(attacker.currentDamage() - defender.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        //������������ߴ������˶���
        if(attacker.isCritical)
        {
            defender.GetComponent<Animator>().SetTrigger("Hurt");
        }

        //����Ѫ��
        UpdateHealthBar?.Invoke(CurrentHealth,MaxHealth);

        //���¾���ֵ
        if (CurrentHealth <= 0)
            attacker.characterData.UpdateExp(characterData.killExp);
    }

    //�������أ�����rock����˺�
    public void TakeDamage(int damage,CharacterStats defender)
    {
        int currentdamage = Mathf.Max(damage - defender.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        //����Ѫ��
        UpdateHealthBar?.Invoke(CurrentHealth, MaxHealth);

        
        if (CurrentHealth <= 0)
            GameManager.Instance.playerStats.characterData.UpdateExp(characterData.killExp);
    }

    private int currentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(attackData.minDamage,attackData.maxDamage);
        //����
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
        //�л���������
        //attackData.ApplyWeaponData(weapon.weaponData);
        attackData = weapon.weaponData; //�������

        //�л�����
        GetComponent<Animator>().runtimeAnimatorController = weapon.weaponAnimator;
    }

    public void UnEquipWeapon()
    {
        if (weaponSlot.transform.childCount != 0)
        {
            for (int i = 0; i < weaponSlot.transform.childCount; i++)
            {
                //�ݻ��ֲ�����������
                Destroy(weaponSlot.transform.GetChild(i).gameObject);
            }
        }

        //�л�����
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
