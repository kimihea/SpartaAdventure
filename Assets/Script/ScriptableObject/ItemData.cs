using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItempType
{
    Equipable,
    Consumable,
    Resource
}

public enum ConsumableType
{
    Health,
    Stamina
}

[Serializable]
public class ItemDateConsumable
{
    public ConsumableType type;
    public float value;
}


[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItempType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDateConsumable[] consumables;
    [Header("Equip")]
    public GameObject equipPrefab;
}
