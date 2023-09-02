using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Active", menuName = "Active/Create new active")]
public class ActiveAbility : ScriptableObject
{

    [TextArea]
    public string description;

    public enum AbilityType {None, Damage, Status, Heal, Create, Movement, StatChange}

    public enum AbilityTarget { Foe, FoeAoe, Self, SelfAoe, Ally, AllyAoe, FullFoeAoe, FullAllyAoe, Everyone, Multitarget}

    [Header("Ability Categories")]

    public AbilityType type;

    public AbilityTarget target;

    [Header("Ability Potency")]

    public int extraTargets;

    public int range;

    public float damageByPowerMultiple;

    public int healAmount;

    public int aoe;

    public GameObject animation;

    [Header("Status Effect")]

    public StatusEffectContainer status;

}

[System.Serializable]
public class StatusEffectContainer
{
    public string name;

    [TextArea]
    public string description;

    public StatusEffect status;

    public float statusTime;

    public float statusTimer;

    public float ticTime;

    public float ticTimer;

    public int ticDamage;

    [Header("Stats")]
    public float critDamage;
    public int critChance;
    public float moveSpeed;
    public int power;
    public int def;
    public float lifesteal;
    public float actionSpeed;
}

[System.Serializable]
public class ActiveStatus
{
    public StatusEffect status;

    [TextArea]
    public string description;

    public float statusTime;

    public float statusTimer;

    public float ticTime;

    public float ticTimer;

    public int ticDamage;

    [Header("Stats")]
    public float critDamage;
    public int critChance;
    public float moveSpeed;
    public int power;
    public int def;
    public float lifesteal;
    public float actionSpeed;
}

