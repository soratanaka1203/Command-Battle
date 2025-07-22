using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Data/Item")]
public class Item : ScriptableObject
{
    public string itemName;        // アイテム名
    public string description;     // 説明
    public int quantity;           //所持数
    public ItemEffectType effect;  // 効果タイプ（HP回復、状態異常回復など）
    public int value;              // 効果量（HP回復量など）
}

public enum ItemEffectType
{
    Heal,       // HP回復
    Cure,       // 状態異常回復
    Buff        // 能力強化
}
