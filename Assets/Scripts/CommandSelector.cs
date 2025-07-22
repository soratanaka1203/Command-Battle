using UnityEngine;
using UnityEngine.UI;

public class CommandSelector : MonoBehaviour
{
    [SerializeField] Button attackButton;
    [SerializeField] Button skillButton;
    [SerializeField] Button ItemButton;

    [SerializeField] BattleSystem battleSystem;

    void Start()
    {
        attackButton.onClick.AddListener(() => OnCommandSelected(BattleCommand.Attack));
        skillButton.onClick.AddListener(() => OnCommandSelected(BattleCommand.Skill));
        ItemButton.onClick.AddListener(() => OnCommandSelected(BattleCommand.Item));
    }

    void OnCommandSelected(BattleCommand command)
    {
        Debug.Log("選択されたコマンド: " + command);
        battleSystem.OnPlayerCommandSelected(command);  // バトル処理へ渡す
    }
}

public enum BattleCommand { Attack, Skill, Item};
