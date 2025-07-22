using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Player")]
public class PlayerData : ScriptableObject
{
    public string characterName;
    public int hp;
    public int attack;
    public int skillMaxUses;
    public Sprite playerSprite;
    public List<Item> playerItems;
}
