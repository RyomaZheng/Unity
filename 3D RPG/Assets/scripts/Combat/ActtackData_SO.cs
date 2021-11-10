using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Character stats/Attack Data")]
public class ActtackData_SO : ScriptableObject
{
    // 攻击范围
    public float attackRange;

    // 技能范围
    public float skillRange;

    // 冷却时间
    public float coolDown;

    // 最少伤害
    public int minDamage;

    // 最大伤害
    public int maxDamage;

    // 暴击加成百分比
    public float criticalMultiplier;

    // 暴击率
    public float criticalChance;

    public void ApplyWeaponData(ActtackData_SO weapon)
    {
        this.attackRange = weapon.attackRange;
        this.skillRange = weapon.skillRange;
        this.coolDown = weapon.coolDown;
        this.criticalMultiplier = weapon.criticalMultiplier;
        this.criticalChance = weapon.criticalChance;

        this.minDamage += weapon.minDamage;
        this.maxDamage += weapon.maxDamage;
    }

    public void RevertWeaponData(ActtackData_SO weapon)
    {
        attackRange = weapon.attackRange;
        skillRange = weapon.skillRange;
        coolDown = weapon.coolDown;
        criticalMultiplier = weapon.criticalMultiplier;
        criticalChance = weapon.criticalChance;

        minDamage = weapon.minDamage;
        maxDamage = weapon.maxDamage;
    }
}
