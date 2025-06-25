using UnityEngine;
using System.Collections;
using UnityEditor.U2D.Animation;

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

    private Character player;
    private Character enemy;


    private void Start()
    {
        currentState = BattleState.PlayerTurn;
    }
    public void OnPlayerCommandSelected(BattleCommand battleCommand)
    {
        if (currentState == BattleState.PlayerTurn) return;
        switch (battleCommand)
        {
            case BattleCommand.Attack:
                PlayerAttack();
                break;
            case BattleCommand.Skill:
                // スキル選択UI表示など
                break;
            case BattleCommand.Item:
                //UI表示
                break;
        }
    }

    private void PlayerAttack()
    {

    }

    private void Init()
    {
        // キャラクター表示を生成
        GameObject playerGO = Instantiate(playerPrefab, canvasTransform);
        //GetComponent<RectTransform>();
        player = playerGO.GetComponent<Character>();
        player.Setup(playerData.characterName, playerData.hp, playerData.playerSprite);

        GameObject enemyGO = Instantiate(enemyPrefab, canvasTransform);
        enemy = enemyGO.GetComponent<Character>();
        enemy.Setup(enemyData.enemyName, enemyData.hp, enemyData.enemySprite);
    }
}


public enum BattleState {PlayerTurn, EnemyTurn};
