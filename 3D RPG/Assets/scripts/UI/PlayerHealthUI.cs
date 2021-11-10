using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    Text levelText;

    Image healthSlider;

    Image expSlider;

    private void Awake()
    {
        levelText = transform.GetChild(2).GetComponent<Text>();
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        levelText.text = "Level  " + GameManager.Instance.playerStats.characterData.currentLevel.ToString("00");
        UpdateHealth();
        UpdateExp();
    }

    // �������Ѫ��
    private void UpdateHealth()
    {
        CharacterStats playerStats = GameManager.Instance.playerStats;

        float sliderPercent = (float)playerStats.CurrentHealth / playerStats.MaxHealth;
        healthSlider.fillAmount = sliderPercent;
    }

    // ������Ҿ�����
    private void UpdateExp()
    {
        CharacterStats playerStats = GameManager.Instance.playerStats;

        float sliderPercent = (float)playerStats.characterData.currentExp / playerStats.characterData.baseExp;
        expSlider.fillAmount = sliderPercent;
    }
}
