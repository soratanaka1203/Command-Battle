using UnityEngine;
using System.Collections;
using UnityEditor.U2D.Animation;
using TMPro;
using Unity.VisualScripting;

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


    private void Start()
    {
        currentState = BattleState.PlayerTurn;
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
                // �X�L���I��UI�\���Ȃ�
                currentState = BattleState.EnemyTurn;//���̎���
                StartCoroutine(EnemyAttack());
                break;
            case BattleCommand.Item:
                //UI�\��
                currentState = BattleState.EnemyTurn;//���̎���
                StartCoroutine(EnemyAttack());
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

    private void Init()
    {
        // �L�����N�^�[�\���𐶐�
        GameObject playerObj = Instantiate(playerPrefab, canvasTransform);
        RectTransform playerPosition = playerObj.GetComponent<RectTransform>();
        playerPosition.position = playerSpawnPoint.position;
        player = playerObj.GetComponent<Character>();
        player.Setup(playerData.characterName, playerData.hp, playerData.attack, playerData.playerSprite);

        GameObject enemyObj = Instantiate(enemyPrefab, canvasTransform);
        RectTransform enemyPosition = enemyObj.GetComponent<RectTransform>();
        enemyPosition.position = enemySpawnPoint.position;
        enemy = enemyObj.GetComponent<Character>();
        enemy.Setup(enemyData.enemyName, enemyData.hp, enemyData.attack, enemyData.enemySprite);
    }
}


public enum BattleState {PlayerTurn, EnemyTurn};
