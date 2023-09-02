using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class GameController : MonoBehaviour
{

    public enum GameState { Start, Generating, Buying, Battle, Busy, End, Map}

    GameState state;

    public GameObject startMenu;
    public GameObject cardsMenu;
    public GameObject endMenu;

    public Map map;

    public UnitCardContainer cardContainer;

    public Party allyParty;
    public Party enemyParty;

    public Player player;

    public int level = 0;
    public int maxLevel = 13;

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

    public void OpenMap()
    {
        state = GameState.Map;

        map.ClearedMapNodes();

        map.gameObject.SetActive(true);
    }

    public void StartGame()
    {

        state = GameState.Map;

        map.CreateMap();

        OpenMap();

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
        level++;
        player.level = level;
        OpenMap();
        PauseUnits();

    }

    public void GoNextLevel()
    {
        
        enemyParty.ClearUnits();

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
            else if (level >= 1)
            {
                player.GainGold(6);
            }

            cardsMenu.SetActive(true);

            startMenu.SetActive(false);

            EnableDragging();

            cardContainer.Reroll(true);

            map.OpenEnvironments();

            map.gameObject.SetActive(false);

        }


        
    }

    void GenerateEnemyParty()
    {
        for (int i = 0; i < level + 1; i++)
        {
            enemyParty.CreateRandomUnit();
        }

        if (player.level == 3 || player.level == 6 || player.level == 9 || player.level == 12 || player.level == 13)
        {
            enemyParty.SpawnUnit(map.GetBoss().GetComponent<Unit>());
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
