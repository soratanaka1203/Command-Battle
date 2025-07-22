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
                StartCoroutine(OnPlayerSkill());
                break;
            case BattleCommand.Item:
                //UI表示
                itemPouchUI.Open(playerItems);
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
            yield break;
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

        if (onSkill && remainingReflects >= 0)
        {

            if (RollChance80())
            {
                remainingReflects--;
                descriptionText.text = $"スキルの効果発動\n" +
                                       $"敵の攻撃を反射した\n"+
                                       $"残り反射回数：{remainingReflects}";
                yield return new WaitForSeconds(2f);
            }

            //残り反射回数が0以下だったらフラグをおろす
            if (remainingReflects <= 0)
            {
                onSkill = false;
            }

            if (enemy.TakeDamage(enemy.attack))
            {
                yield return new WaitForSeconds(1f);
                descriptionText.text = $"{enemy.characterName}を倒した\n" +
                                    "戦闘終了";
                yield break;
            }
            descriptionText.text = $"{enemy.characterName}に{enemy.attack}のダメージ";

            yield return new WaitForSeconds(2f);
            currentState = BattleState.PlayerTurn;
            descriptionText.text = "あなたの番です。\n" +
                                   "行動を選択してください。";
            yield break;
        }

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

    private bool onSkill = false;
    private int remainingReflects = 0;
    IEnumerator OnPlayerSkill()
    {

        if (player.PlaySkill())
        {
            onSkill = true;
            remainingReflects = 3;
            descriptionText.text = $"{player.characterName}のスキルを発動\n" +
                               "80%の確率で3回まで次のダメージを跳ね返す"+
                               $"残りスキル使用可能回数：{player.skillCurrentUses + 1}";
                                                                                  //↑１ずれているため修正

            yield return new WaitForSeconds(2f);

            currentState = BattleState.EnemyTurn;
            descriptionText.text = "相手の番です。";
            StartCoroutine(EnemyAttack());
        }
        else
        {
            descriptionText.text = "スキルを発動できません\n" +
                                   "もう一度行動を選択してください";
        }
    }

    public IEnumerator UseItem(Item item)
    {
        if (item.quantity <= 0)
        {
            descriptionText.text = $"{item.itemName}はもう持っていません。";
            yield break;
        }

        // アイテム効果を適用
        switch (item.effect)
        {
            case ItemEffectType.Heal:
                player.Heal(item.value);
                descriptionText.text = $"{item.itemName}を使った\n" +
                                       $"{player.characterName}のHPが{item.value}回復した";
                break;

            //case ItemEffectType.Cure:
            //    player.CureStatus(); // 状態異常回復
            //    descriptionText.text = $"{item.itemName}を使った！\n" +
            //                           $"状態異常が回復した！";
            //    break;

                // 他の効果を追加できる
        }

        item.quantity--;

        yield return new WaitForSeconds(2f);

        // 敵のターンに移行
        currentState = BattleState.EnemyTurn;
        descriptionText.text = "相手の番です。";
        StartCoroutine(EnemyAttack());
    }


    private bool RollChance80()
    {
        return Random.value < 0.8f; // 80%の確率でtrue
    }

    private void Init()
    {
        // キャラクター表示を生成
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

        //所持アイテムを取得
        playerItems = playerData.playerItems;
    }
}


public enum BattleState {PlayerTurn, EnemyTurn};
