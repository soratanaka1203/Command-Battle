using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Character : MonoBehaviour
{
    [SerializeField] Image characterImage;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI hpText;

    private int maxHP;
    private int currentHP;
    public string characterName {  get { return nameText.text; } }
    public int attack {get; private set; }
    private int skillMaxUses;
    public int skillCurrentUses {get; private set;}
 

    public void Setup(string name, int maxHp, int attack, Sprite sprite, int skillMaxUses = 0)
    {
        nameText.text = name;
        //characterImage.sprite = sprite;
        this.maxHP = maxHp;
        this.currentHP = maxHp;
        this.attack = attack;  
        this.skillMaxUses = skillMaxUses;
        this.skillCurrentUses = skillMaxUses;
        UpdateHPText();
    }

    public bool TakeDamage(int damage)
    {
        currentHP = Mathf.Max(0, currentHP - damage);
        UpdateHPText();
        //体力が0以下になったらtrueを返し、倒されたことを知らせる
        if (currentHP <= 0) return true;
        else return false;
    }

    public bool PlaySkill()
    {
        if (skillCurrentUses >= 0)
        {
            skillCurrentUses--;
            //スキルを発動出来たらtrueを返す
            return true;
        }
        else return false;
    }

    private void UpdateHPText()
    {
        hpText.text = $"残り体力：{ currentHP} / { maxHP}";
    }
}
