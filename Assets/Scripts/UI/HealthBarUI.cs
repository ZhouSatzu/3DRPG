using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public GameObject healthBarPrefab;

    public Transform barPoint;

    public bool alwaysVisible;

    public float visiableTime;
    private float timeLeft;

    Image healthSlider;
    Transform UIBar;
    Transform cam;

    CharacterStats currentStats;

    private void Awake()
    {
        currentStats = GetComponent<CharacterStats>();

        currentStats.UpdateHealthBar += UpdateHealthBarUI;
    }

    private void OnEnable()
    {
        cam = Camera.main.transform;

        foreach(Canvas canvas in FindObjectsOfType<Canvas>())
        {
            //所有世界坐标的canvas
            if(canvas.renderMode == RenderMode.WorldSpace)
            {
                UIBar = Instantiate(healthBarPrefab,canvas.transform).transform;
                //第一个子物体的image组件
                healthSlider = UIBar.GetChild(0).GetComponent<Image>();

                UIBar.gameObject.SetActive(alwaysVisible);
            }
        }
    }

    private void UpdateHealthBarUI(int currentHealth, int maxHealth)
    {
        if(currentHealth <= 0)
            Destroy(UIBar.gameObject);

        UIBar.gameObject.SetActive(true);
        timeLeft = visiableTime;
        float sliderPercent = (float)currentHealth / maxHealth;
        healthSlider.fillAmount = sliderPercent;
    }

    private void LateUpdate()
    {
        if(UIBar != null)
        {
            UIBar.position = barPoint.position;
            UIBar.forward = -cam.forward;

            if (timeLeft <= 0 && !alwaysVisible)
                UIBar.gameObject.SetActive(false);
            else
                timeLeft -= Time.deltaTime;
        }
    }
}
