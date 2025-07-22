using UnityEngine;
using System.Collections;
using UnityEditor.U2D.Animation;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;

public class BattleSystem : MonoBehaviour
{
    private BattleState currentState;
    [SerializeField]private PlayerData playerData;
    [SerializeField]private EnemyData enemyData;
    [SerializeField]private GameObject playerPrefab;
    [SerializeField]private GameObject enemyPrefab;
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private RectTransform playerSpawnPoint;
    [SerializeField] private RectTransform enemySpawnPoint;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private Character player;
    private Character enemy;
    [SerializeField]private ItemPouchUI itemPouchUI;
    private List<Item> playerItems;


    private void Start()
    {
        currentState = BattleState.PlayerTurn;
        itemPouchUI.Initialize(this);
        itemPouchUI.gameObject.SetActive(false);
        Init();
        descriptionText.text = "���Ȃ��̔Ԃł�\n" +
                              "�s����I�����Ă�������";
    }

    public void OnPlayerCommandSelected(BattleCommand battleCommand)
    {
        if (currentState == BattleState.EnemyTurn) return;
        switch (battleCommand)
        {
            case BattleCommand.Attack:
                StartCoroutine(PlayerAttack());
                break;
            case BattleCommand.Skill:
                StartCoroutine(OnPlayerSkill());
                break;
            case BattleCommand.Item:
                //UI�\��
                itemPouchUI.Open(playerItems);
                break;
        }
    }

    IEnumerator PlayerAttack()
    {
        descriptionText.text = $"{enemy.characterName}�ɍU��\n" +
                               $"{player.attack}�̃_���[�W";

        if (enemy.TakeDamage(player.attack))
        {
            yield return new WaitForSeconds(1f);
            descriptionText.text = $"{enemy.characterName}��|����\n" +
                                   "�퓬�I��";
            yield break;
        }
        
        yield return new WaitForSeconds(2f);

        // 2�b��Ƀ^�[���ύX�ƃe�L�X�g�X�V
        currentState = BattleState.EnemyTurn;
        descriptionText.text = "����̔Ԃł��B";
        StartCoroutine(EnemyAttack());
    }

    IEnumerator EnemyAttack()
    {
        yield return new WaitForSeconds(2f);

        if (onSkill && remainingReflects >= 0)
        {

            if (RollChance80())
            {
                remainingReflects--;
                descriptionText.text = $"�X�L���̌��ʔ���\n" +
                                       $"�G�̍U���𔽎˂���\n"+
                                       $"�c�蔽�ˉ񐔁F{remainingReflects}";
                yield return new WaitForSeconds(2f);
            }

            //�c�蔽�ˉ񐔂�0�ȉ���������t���O�����낷
            if (remainingReflects <= 0)
            {
                onSkill = false;
            }

            if (enemy.TakeDamage(enemy.attack))
            {
                yield return new WaitForSeconds(1f);
                descriptionText.text = $"{enemy.characterName}��|����\n" +
                                    "�퓬�I��";
                yield break;
            }
            descriptionText.text = $"{enemy.characterName}��{enemy.attack}�̃_���[�W";

            yield return new WaitForSeconds(2f);
            currentState = BattleState.PlayerTurn;
            descriptionText.text = "���Ȃ��̔Ԃł��B\n" +
                                   "�s����I�����Ă��������B";
            yield break;
        }

        descriptionText.text = $"{enemy.characterName}�̍U��\n" +
                               $"{player.characterName}��{enemy.attack}�̃_���[�W";

        if (player.TakeDamage(enemy.attack))
        {
            yield return new WaitForSeconds(1f);
            descriptionText.text = $"{player.characterName}��{enemy.characterName}�̍U���ɂ���Ă���Ă��܂���\n" +
                                   "�Q�[���I�[�o�[";
            yield break;
        }

        yield return new WaitForSeconds(2f);

        currentState = BattleState.PlayerTurn;
        descriptionText.text = "���Ȃ��̔Ԃł��B\n" +
                               "�s����I�����Ă��������B";
    }

    private bool onSkill = false;
    private int remainingReflects = 0;
    IEnumerator OnPlayerSkill()
    {

        if (player.PlaySkill())
        {
            onSkill = true;
            remainingReflects = 3;
            descriptionText.text = $"{player.characterName}�̃X�L���𔭓�\n" +
                               "80%�̊m����3��܂Ŏ��̃_���[�W�𒵂˕Ԃ�"+
                               $"�c��X�L���g�p�\�񐔁F{player.skillCurrentUses + 1}";
                                                                                  //���P����Ă��邽�ߏC��

            yield return new WaitForSeconds(2f);

            currentState = BattleState.EnemyTurn;
            descriptionText.text = "����̔Ԃł��B";
            StartCoroutine(EnemyAttack());
        }
        else
        {
            descriptionText.text = "�X�L���𔭓��ł��܂���\n" +
                                   "������x�s����I�����Ă�������";
        }
    }

    public IEnumerator UseItem(Item item)
    {
        if (item.quantity <= 0)
        {
            descriptionText.text = $"{item.itemName}�͂��������Ă��܂���B";
            yield break;
        }

        // �A�C�e�����ʂ�K�p
        switch (item.effect)
        {
            case ItemEffectType.Heal:
                player.Heal(item.value);
                descriptionText.text = $"{item.itemName}���g����\n" +
                                       $"{player.characterName}��HP��{item.value}�񕜂���";
                break;

            //case ItemEffectType.Cure:
            //    player.CureStatus(); // ��Ԉُ��
            //    descriptionText.text = $"{item.itemName}���g�����I\n" +
            //                           $"��Ԉُ킪�񕜂����I";
            //    break;

                // ���̌��ʂ�ǉ��ł���
        }

        item.quantity--;

        yield return new WaitForSeconds(2f);

        // �G�̃^�[���Ɉڍs
        currentState = BattleState.EnemyTurn;
        descriptionText.text = "����̔Ԃł��B";
        StartCoroutine(EnemyAttack());
    }


    private bool RollChance80()
    {
        return Random.value < 0.8f; // 80%�̊m����true
    }

    private void Init()
    {
        // �L�����N�^�[�\���𐶐�
        GameObject playerObj = Instantiate(playerPrefab, canvasTransform);
        RectTransform playerPosition = playerObj.GetComponent<RectTransform>();
        playerPosition.position = playerSpawnPoint.position;
        player = playerObj.GetComponent<Character>();
        player.Setup(playerData.characterName, playerData.hp, playerData.attack, playerData.playerSprite, playerData.skillMaxUses);

        GameObject enemyObj = Instantiate(enemyPrefab, canvasTransform);
        RectTransform enemyPosition = enemyObj.GetComponent<RectTransform>();
        enemyPosition.position = enemySpawnPoint.position;
        enemy = enemyObj.GetComponent<Character>();
        enemy.Setup(enemyData.enemyName, enemyData.hp, enemyData.attack, enemyData.enemySprite);

        //�����A�C�e�����擾
        playerItems = playerData.playerItems;
    }
}


public enum BattleState {PlayerTurn, EnemyTurn};
