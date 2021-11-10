using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public GameObject healthBarUIPrefab;

    public Transform barPoint;

    public bool alwaysVisible;

    public float visibleTime;

    private float timeLeft;

    Image healthSlider;

    Transform UIbar;

    Transform cam;

    CharacterStats currentStats;

    private void Awake()
    {
        currentStats = GetComponent<CharacterStats>();

        currentStats.UpdateHealthBarOnAttack += UpdateHealthBar;
    }

    void LateUpdate()
    {
        if (UIbar != null)
        {
            UIbar.position = barPoint.position;
            UIbar.forward = -cam.forward;

            if (timeLeft <= 0 && !alwaysVisible)
            {
                UIbar.gameObject.SetActive(false);
            }
            else
            {
                timeLeft -= Time.deltaTime;
            }
        }

    }

    void OnEnable()
    {
        cam = Camera.main.transform;

        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                // ����Ѫ��ui
                UIbar = Instantiate(healthBarUIPrefab, canvas.transform).transform;

                // ��ֵѪ��ͼƬ
                healthSlider = UIbar.GetChild(0).GetComponent<Image>();

                // �ж��Ƿ���������
                UIbar.gameObject.SetActive(alwaysVisible);
            }

        }
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0)
        {
            Destroy(UIbar.gameObject);
        }

        // ����Ѫ��ʱ������Ϊ�ɼ���
        UIbar.gameObject.SetActive(true);
        timeLeft = visibleTime;

        float sliderPercent = (float)currentHealth / maxHealth;

        healthSlider.fillAmount = sliderPercent;
    }
}
