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

    // 更新经验值
    public void UpdateExp(float point)
    {
        // 当前经验值 + 获取的经验值
        currentExp += point;
        // 如果当前经验值大于当前等级的基础经验值，就进行升级
        if (currentExp >= baseExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        // 所有你要提升的数据的方法
        // 提升等级
        currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);

        // 增加基础经验值
        baseExp += (baseExp * LevelMultiplier);
        currentExp = 0f;

        // 增加血条
        maxHealth += (int)(maxHealth * LevelMultiplier);

        // 恢复满血
        currentHealth = maxHealth;

        Debug.Log("LEVEL UP!" + currentLevel + ", MAX HEALTH:" + maxHealth);

    }
}
