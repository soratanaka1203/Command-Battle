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
        descriptionText.text = "あなたの番です\n" +
                              "行動を選択してください";
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
                // スキル選択UI表示など
                currentState = BattleState.EnemyTurn;//仮の実装
                StartCoroutine(EnemyAttack());
                break;
            case BattleCommand.Item:
                //UI表示
                currentState = BattleState.EnemyTurn;//仮の実装
                StartCoroutine(EnemyAttack());
                break;
        }
    }

    IEnumerator PlayerAttack()
    {
        descriptionText.text = $"{enemy.characterName}に攻撃\n" +
                               $"{player.attack}のダメージ";

        if (enemy.TakeDamage(player.attack))
        {
            yield return new WaitForSeconds(1f);
            descriptionText.text = $"{enemy.characterName}を倒した\n" +
                                   "戦闘終了";
        }
        
        yield return new WaitForSeconds(2f);

        // 2秒後にターン変更とテキスト更新
        currentState = BattleState.EnemyTurn;
        descriptionText.text = "相手の番です。";
        StartCoroutine(EnemyAttack());
    }

    IEnumerator EnemyAttack()
    {
        yield return new WaitForSeconds(2f);

        descriptionText.text = $"{enemy.characterName}の攻撃\n" +
                               $"{player.characterName}に{enemy.attack}のダメージ";

        if (player.TakeDamage(enemy.attack))
        {
            yield return new WaitForSeconds(1f);
            descriptionText.text = $"{player.characterName}は{enemy.characterName}の攻撃によってやられてしまった\n" +
                                   "ゲームオーバー";
            yield break;
        }

        yield return new WaitForSeconds(2f);

        currentState = BattleState.PlayerTurn;
        descriptionText.text = "あなたの番です。\n" +
                               "行動を選択してください。";
    }

    private void Init()
    {
        // キャラクター表示を生成
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
