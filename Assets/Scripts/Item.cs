using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Data/Item")]
public class Item : ScriptableObject
{
    public string itemName;        // �A�C�e����
    public string description;     // ����
    public int quantity;           //������
    public ItemEffectType effect;  // ���ʃ^�C�v�iHP�񕜁A��Ԉُ�񕜂Ȃǁj
    public int value;              // ���ʗʁiHP�񕜗ʂȂǁj
}

public enum ItemEffectType
{
    Heal,       // HP��
    Cure,       // ��Ԉُ��
    Buff        // �\�͋���
}
