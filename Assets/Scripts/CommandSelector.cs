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
        Debug.Log("�I�����ꂽ�R�}���h: " + command);
        battleSystem.OnPlayerCommandSelected(command);  // �o�g�������֓n��
    }
}

public enum BattleCommand { Attack, Skill, Item};
