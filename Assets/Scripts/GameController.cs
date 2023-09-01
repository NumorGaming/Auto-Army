using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class GameController : MonoBehaviour
{

    public enum GameState { Start, Generating, Buying, Battle, Busy, End}

    GameState state;

    public GameObject startMenu;
    public GameObject cardsMenu;
    public GameObject endMenu;

    public UnitCardContainer cardContainer;

    public Party allyParty;
    public Party enemyParty;

    public Player player;

    public int level = 1;
    public int maxLevel = 12;

    public TextMeshProUGUI battleText;



    // Start is called before the first frame update
    void Start()
    {
        state = GameState.Start;
        startMenu.SetActive(true);
        DisableDragging();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == GameState.Battle)
        {
            CheckForBattleEnd();
        }
    }

    public void CheckForBattleEnd()
    {
        if (allyParty.CheckIfPartyDead() || enemyParty.CheckIfPartyDead())
        {
            BattleOver();
        }
        
    }

    public void StartGame()
    {

        state = GameState.Generating;

        GenerateEnemyParty();

        state = GameState.Buying;

        battleText.text = "Battle " + "(" + level + "/" + maxLevel + ")";

        cardsMenu.SetActive(true);

        startMenu.SetActive(false);

        EnableDragging();

        player.level = level;
        player.GainGold(6);

    }

    public void StartBattle()
    {
        state = GameState.Battle;

        DisableDragging();

        cardsMenu.SetActive(false);

        UnPauseUnits();

        List<Unit> units = new List<Unit>();

        units.AddRange(GameObject.FindObjectsOfType<Unit>());

        for (int i = 0; i < units.Count; i++)
        {
            units[i].CombatStart();
        }

    }

    public void PauseUnits()
    {
        List<Unit> units = new List<Unit>();

        units.AddRange(GameObject.FindObjectsOfType<Unit>());

        for (int i = 0; i < units.Count; i++)
        {
            units[i].paused = true;
        }
    }

    public void UnPauseUnits()
    {
        List<Unit> units = new List<Unit>();

        units.AddRange(GameObject.FindObjectsOfType<Unit>());

        for (int i = 0; i < units.Count; i++)
        {
            units[i].paused = false;
        }
    }

    public void BattleOver()
    {

        GoNextLevel();
        PauseUnits();
    }

    void GoNextLevel()
    {
        enemyParty.ClearUnits();

        level++;
        player.level = level;

        if (level > maxLevel)
        {
            state = GameState.End;

            endMenu.SetActive(true);
        }
        else
        {
            state = GameState.Generating;

            GenerateEnemyParty();

            for (int i = 0; i < allyParty.units.Count; i++)
            {
                allyParty.units[i].CombatOver();
            }

            state = GameState.Buying;

            battleText.text = "Battle " + "(" + level + "/" + maxLevel + ")";

            cardContainer.Reroll(true);

            if (level >=  13)
            {
                player.GainGold(10);
            }
            else if (level >= 12)
            {
                player.GainGold(6);
            }
            else if (level >= 9)
            {
                player.GainGold(5);
            }
            else if (level >= 6)
            {
                player.GainGold(4);
            }
            else if (level >= 3)
            {
                player.GainGold(3);
            }
            else if (level >= 2)
            {
                player.GainGold(2);
            }

            cardsMenu.SetActive(true);

            startMenu.SetActive(false);

            EnableDragging();
        }


        
    }

    void GenerateEnemyParty()
    {
        for (int i = 0; i < level + 1; i++)
        {
            enemyParty.CreateRandomUnit();
        }
    }

    void DisableDragging()
    {
        List<DragAndDrop> units = new List<DragAndDrop>();

        units.AddRange(GameObject.FindObjectsOfType<DragAndDrop>());

        for (int i = 0; i < units.Count; i++)
        {
            units[i].allowed = false;
        }
    }

    void EnableDragging()
    {
        List<DragAndDrop> units = new List<DragAndDrop>();

        units.AddRange(GameObject.FindObjectsOfType<DragAndDrop>());

        for (int i = 0; i < units.Count; i++)
        {
            units[i].allowed = true;
        }
    }


}
