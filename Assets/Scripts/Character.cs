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
    public int attack { get; private set; }

    public void Setup(string name, int maxHp, int attack, Sprite sprite)
    {
        nameText.text = name;
        //characterImage.sprite = sprite;
        this.maxHP = maxHp;
        this.currentHP = maxHp;
        this.attack = attack;
        UpdateHPText();
    }

    public bool TakeDamage(int damage)
    {
        currentHP = Mathf.Max(0, currentHP - damage);
        UpdateHPText();
        //�̗͂�0�ȉ��ɂȂ�����true��Ԃ��A�|���ꂽ���Ƃ�m�点��
        if (currentHP <= 0) return true;
        else return false;
    }

    private void UpdateHPText()
    {
        hpText.text = $"�c��̗́F{ currentHP} / { maxHP}";
    }
}
