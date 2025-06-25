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

    public void Setup(string name, int maxHp, Sprite sprite)
    {
        nameText.text = name;
        //characterImage.sprite = sprite;
        this.maxHP = maxHp;
        this.currentHP = maxHp;
        UpdateHPText();
    }

    public void TakeDamage(int damage)
    {
        currentHP = Mathf.Max(0, currentHP - damage);
        UpdateHPText();
    }

    private void UpdateHPText()
    {
        hpText.text = currentHP + " / " + maxHP;
    }
}
