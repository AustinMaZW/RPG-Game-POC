//using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    private bool isBattleActive;

    [SerializeField] GameObject battleScene;
    [SerializeField] List<BattleCharacters> activeCharacters = new List<BattleCharacters>();

    [SerializeField] Transform[] playersPositions, enemiesPositions;

    [SerializeField] BattleCharacters[] playersPrefabs, enemiesPrefabs;

    [SerializeField] int currentTurn;
    [SerializeField] bool waitingForTurn;
    [SerializeField] GameObject UIButtonHolder;

    [SerializeField] BattleMoves[] battleMovesList;

    [SerializeField] ParticleSystem characterAttackEffect;
    [SerializeField] CharacterDamageGUI damageText;

    [SerializeField] GameObject[] playersBattleStats;
    [SerializeField] TextMeshProUGUI[] playersNameText;
    [SerializeField] Slider[] playerHealthSlider, playerManaSlider;

    [SerializeField] GameObject enemyTargetPanel;
    [SerializeField] BattleTargetButtons[] targetButtons;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            StartBattle(new string[] { "Minotaur", "Minotaur", "Minotaur", "Minotaur" });
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            NextTurn();
        }
        CheckPlayerButtonHolder();
    }

    private void CheckPlayerButtonHolder()
    {
        if (isBattleActive)
        {
            if (waitingForTurn)
            {
                if (activeCharacters[currentTurn].IsPlayer())
                    UIButtonHolder.SetActive(true);
                else
                {
                    UIButtonHolder.SetActive(false);
                    StartCoroutine(EnemyMoveCoroutine());
                }
            }
        }
    }

    public void StartBattle(string[] enemiesToSpawn)
    {
        if (!isBattleActive)
        {
            SetUpBattle();
            AddingPlayers();
            AddingEnemies(enemiesToSpawn);
            UpdatePlayerStats();

            waitingForTurn = true;
            currentTurn = 0;
        }

    }

    private void AddingEnemies(string[] enemiesToSpawn)
    {
        //loopthrough enemies to spawn and find match in enemy prefabs
        //if there is match, then instantiate the enemy
        for (int i = 0; i < enemiesToSpawn.Length; i++)
        {
            if (enemiesToSpawn[i] != "")
            {
                for (int j = 0; j < enemiesPrefabs.Length; j++)
                {
                    if (enemiesPrefabs[j].characterName == enemiesToSpawn[i])
                    {
                        BattleCharacters newEnemy = Instantiate(
                            enemiesPrefabs[j],
                            enemiesPositions[i].position,
                            enemiesPositions[i].rotation,
                            enemiesPositions[i]
                            );

                        activeCharacters.Add(newEnemy);
                    }
                }
            }
        }
    }

    private void AddingPlayers()
    {
        for (int i = 0; i < GameManager.instance.GetPlayerStats().Length; i++)
        {
            if (GameManager.instance.GetPlayerStats()[i].gameObject.activeInHierarchy)
            {
                for (int j = 0; j < playersPrefabs.Length; j++)
                {
                    if (playersPrefabs[j].characterName == GameManager.instance.GetPlayerStats()[i].PlayerName())
                    {
                        BattleCharacters newPlayer = Instantiate(
                            playersPrefabs[j],
                            playersPositions[i].position,
                            playersPositions[i].rotation,
                            playersPositions[i]
                        );

                        activeCharacters.Add(newPlayer);
                        ImportPlayerStats(i);
                    }
                }
            }
        }
    }

    private void ImportPlayerStats(int i)
    {
        PlayerStats player = GameManager.instance.GetPlayerStats()[i];

        activeCharacters[i].currentHP = player.currentHP;
        activeCharacters[i].maxHP = player.maxHP;

        activeCharacters[i].currentMana = player.currentMana;
        activeCharacters[i].maxMana = player.maxMana;

        activeCharacters[i].dexterity = player.dexterity;
        activeCharacters[i].defence = player.defence;

        activeCharacters[i].wpnPower = player.weaponPower;
        activeCharacters[i].armorDefence = player.armorDefence;
    }

    private void SetUpBattle()
    {
        isBattleActive = true;
        GameManager.instance.battleIsActive = true;

        transform.position = new Vector3(
            Camera.main.transform.position.x,
            Camera.main.transform.position.y,
            transform.position.z
            );
        battleScene.SetActive(true);
    }

    private void NextTurn()
    {
        currentTurn++;
        if (currentTurn >= activeCharacters.Count)
            currentTurn = 0;

        waitingForTurn = true;
        UpdateBattle();
        UpdatePlayerStats();
    }

    private void UpdateBattle()
    {
        bool allEnemiesAreDead = true;
        bool allPlayersAreDead = true;

        for (int i = 0; i < activeCharacters.Count; i++)
        {
            if (activeCharacters[i].currentHP < 0)
            {
                activeCharacters[i].currentHP = 0;
            }

            if (activeCharacters[i].currentHP == 0)
            {
                //kill character
            }
            else
            {
                if (activeCharacters[i].IsPlayer())
                    allPlayersAreDead = false;
                else
                    allEnemiesAreDead = false;
            }
        }

        if (allEnemiesAreDead || allPlayersAreDead)
        {
            if (allEnemiesAreDead)
                print("WE WON!!!");
            else if (allPlayersAreDead)
                print("We Lost");

            battleScene.SetActive(false);
            GameManager.instance.battleIsActive = false;
            isBattleActive = false;
        }
        else
        {
            while(activeCharacters[currentTurn].currentHP == 0)
            {
                currentTurn++;
                if(currentTurn >= activeCharacters.Count)
                {
                    currentTurn = 0;
                }
            }
        }
    }

    public IEnumerator EnemyMoveCoroutine()
    {
        waitingForTurn = false;
        yield return new WaitForSeconds(1f);
        EnemyAttack();

        yield return new WaitForSeconds(1f);
        NextTurn();
    }

    private void EnemyAttack()
    {
        List<int> players = new List<int>();

        for (int i = 0; i < activeCharacters.Count; i++)
        {
            if (activeCharacters[i].IsPlayer() && activeCharacters[i].currentHP > 0)
            {
                players.Add(i);
            }
        }

        int selectedPlayerToAttack = players[Random.Range(0, players.Count)];

        int selectedAttack = Random.Range(0, activeCharacters[currentTurn].AttackMovesAvailable().Length);
        int movePower = 0;
        for (int i = 0; i < battleMovesList.Length; i++)
        {
            if (battleMovesList[i].moveName == activeCharacters[currentTurn].AttackMovesAvailable()[selectedAttack])
            {

                movePower = GettingMovePowerAndEffectInstantiation(selectedPlayerToAttack, i);
            }
        }

        InstantiateEffectOnAttackingCharacter();

        DealDamageToCharacters(selectedPlayerToAttack, movePower);

        UpdatePlayerStats();
    }

    private void InstantiateEffectOnAttackingCharacter()
    {
        //instantiate the attack effect on the character that is attacking
        Instantiate(characterAttackEffect,
            activeCharacters[currentTurn].transform.position,
            activeCharacters[currentTurn].transform.rotation);
    }

    private void DealDamageToCharacters(int selectedCharacterToAttack, int movePower)
    {
        float attackPower = activeCharacters[currentTurn].dexterity + activeCharacters[currentTurn].wpnPower;
        float defenceAmount = activeCharacters[selectedCharacterToAttack].defence + activeCharacters[selectedCharacterToAttack].armorDefence;

        float damageAmount = (attackPower / defenceAmount) * movePower * Random.Range(0.95f, 1.05f);
        int damageToDeal = (int)damageAmount;

        damageToDeal = CalculateCritical(damageToDeal);

        activeCharacters[selectedCharacterToAttack].TakeHPDamage(damageToDeal);

        CharacterDamageGUI characterDamageText = Instantiate(
            damageText,
            activeCharacters[selectedCharacterToAttack].transform.position,
            activeCharacters[selectedCharacterToAttack].transform.rotation);

        characterDamageText.SetDamage(damageToDeal);
    }

    private int CalculateCritical (int damageToDeal)
    {
        if(Random.value <= 0.1f)
        {
            Debug.Log($"CRIT! CRIT! CRIT! Instead of {damageToDeal}, {damageToDeal * 2} was dealt");
            return damageToDeal * 2;
        }

        return damageToDeal;
    }

    public void UpdatePlayerStats()
    {
        for(int i =0; i< playersNameText.Length; i++)
        {
            if(activeCharacters.Count > i)
            {
                if (activeCharacters[i].IsPlayer())
                {
                    BattleCharacters playerData = activeCharacters[i];

                    playersNameText[i].text = playerData.characterName;

                    playerHealthSlider[i].maxValue = playerData.maxHP;
                    playerHealthSlider[i].value = playerData.currentHP;

                    playerManaSlider[i].maxValue = playerData.maxMana;
                    playerManaSlider[i].value = playerData.currentMana;
                }
                else
                {
                    playersBattleStats[i].SetActive(false);
                }
            }
            else
            {
                playersBattleStats[i].SetActive(false);

            }
        }
    }

    public void PlayerAttack(string moveName, int selectEnemyTarget)
    {
        int movePower = 0;

        for(int i = 0; i < battleMovesList.Length; i++)
        {
            if(battleMovesList[i].moveName == moveName)
            {
                movePower = GettingMovePowerAndEffectInstantiation(selectEnemyTarget, i);
            }
        }

        InstantiateEffectOnAttackingCharacter();

        DealDamageToCharacters(selectEnemyTarget, movePower);

        NextTurn();

        enemyTargetPanel.SetActive(false);
    }

    public void OpenTargetMenu(string moveName)
    {
        enemyTargetPanel.SetActive(true);

        List<int> enemies = new List<int>();
        for(int i = 0; i< activeCharacters.Count; i++)
        {
            if (!activeCharacters[i].IsPlayer())
            {
                enemies.Add(i);
            }
        }

        Debug.Log(enemies.Count);

        for(int i = 0; i< targetButtons.Length; i++)
        {
            if(enemies.Count > i)
            {
                targetButtons[i].gameObject.SetActive(true);
                targetButtons[i].moveName = moveName;
                targetButtons[i].activeBattleTarget = enemies[i];
                targetButtons[i].targetName.text = activeCharacters[enemies[i]].characterName;
                Debug.Log(activeCharacters[enemies[i]].characterName);
            }
        }
    }

    private int GettingMovePowerAndEffectInstantiation(int selectCharacterTarget, int i)
    {
        int movePower;
        Instantiate(
               battleMovesList[i].theEffectToUse,
               activeCharacters[selectCharacterTarget].transform.position,
               activeCharacters[selectCharacterTarget].transform.rotation
               );

        movePower = battleMovesList[i].movePower;
        return movePower;
    }
}
