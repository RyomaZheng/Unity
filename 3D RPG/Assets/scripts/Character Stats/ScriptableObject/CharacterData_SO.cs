using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Character stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")]
    public int maxHealth;
    public int currentHealth;
    public int baseDefence;
    public int currentDefence;

    [Header("Kill")]
    public int killPoint;

    [Header("Level")]
    public int currentLevel;
    public int maxLevel;
    public float baseExp;
    public float currentExp;
    public float levelBuff;

    public float LevelMultiplier
    {
        get { return 1 + (currentLevel - 1) * levelBuff; }
    }

    // ���¾���ֵ
    public void UpdateExp(float point)
    {
        // ��ǰ����ֵ + ��ȡ�ľ���ֵ
        currentExp += point;
        // �����ǰ����ֵ���ڵ�ǰ�ȼ��Ļ�������ֵ���ͽ�������
        if (currentExp >= baseExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        // ������Ҫ���������ݵķ���
        // �����ȼ�
        currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);

        // ���ӻ�������ֵ
        baseExp += (baseExp * LevelMultiplier);
        currentExp = 0f;

        // ����Ѫ��
        maxHealth += (int)(maxHealth * LevelMultiplier);

        // �ָ���Ѫ
        currentHealth = maxHealth;

        Debug.Log("LEVEL UP!" + currentLevel + ", MAX HEALTH:" + maxHealth);

    }
}
