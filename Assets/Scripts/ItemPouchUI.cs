using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPouchUI : MonoBehaviour
{
    [SerializeField] private GameObject itemButtonPrefab;
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject panel;
    private List<Item> playerItems;

    private BattleSystem battleSystem; // BattleSystem参照

    public void Initialize(BattleSystem system)
    {
        battleSystem = system;
    }

    public void Open(List<Item> items)
    {
        panel.SetActive(true);
        playerItems = items;
        RefreshUI();
    }

    public void Close()
    {
        panel.SetActive(false);
    }

    private void RefreshUI()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        foreach (var item in playerItems)
        {
            var buttonObj = Instantiate(itemButtonPrefab, contentParent);
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text =
                $"{item.itemName} x{item.quantity}";

            buttonObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                Close(); // ポーチを閉じる
                battleSystem.StartCoroutine(battleSystem.UseItem(item));
            });
        }
    }
}
