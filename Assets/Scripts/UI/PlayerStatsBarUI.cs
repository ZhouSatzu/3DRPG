using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsBarUI : MonoBehaviour
{
    Text health;
    Text attack;
    Text defence;
    Text critical;

    private void Awake()
    {
        health = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        attack = transform.GetChild(1).GetChild(0).GetComponent<Text>();
        critical = transform.GetChild(2).GetChild(0).GetComponent<Text>();
        defence = transform.GetChild(3).GetChild(0).GetComponent<Text>();
    }

    private void Update()
    {
        health.text = GameManager.Instance.playerStats.characterData.currentHealth.ToString();
        attack.text = GameManager.Instance.playerStats.attackData.minDamage.ToString() + "-" + GameManager.Instance.playerStats.attackData.maxDamage.ToString();
        critical.text = GameManager.Instance.playerStats.attackData.criticalChance.ToString();
        defence.text = GameManager.Instance.playerStats.characterData.currentDefence.ToString();
    }
}
