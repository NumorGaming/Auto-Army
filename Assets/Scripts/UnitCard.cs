using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UnitCard : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{

    public Unit unit;
    public int cost;
    public TextMeshProUGUI speciesText;
    public TextMeshProUGUI profText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI nameText;
    public Image image;

    public GameObject blankUnit;

    UnitDetails details;

    Party party;
    public Player player;

    public Coroutine coroutine;

    // Start is called before the first frame update
    void Start()
    {
        GenerateUnit();

        party = GameObject.FindGameObjectWithTag("Party").GetComponent<Party>();
        player = GameObject.FindGameObjectWithTag("GameController").GetComponent<Player>();
        details = GameObject.FindGameObjectWithTag("UnitDetails").GetComponent<UnitDetails>();

    }

    bool CheckIfPriceIsWrong(int cost)
    {
        if (player.level >= 13)
        {
            if (cost < 5) return true;
        }
        else if (player.level >= 12)
        {
            if (cost < 4) return true;
        }
        else if (player.level >= 9)
        {
            if (cost > 5 || cost < 3) return true;
        }
        else if (player.level >= 6)
        {
            if (cost > 4 || cost < 2) return true;
        }
        else if (player.level >= 3)
        {
            if (cost > 3 || cost < 1) return true;
        }
        else if (player.level >= 2)
        {
            if (cost > 3) return true;
        }
        else if (player.level >= 1)
        {
            if (cost > 2) return true;
        }

        return false;

    }


    public void GenerateUnit()
    {
        unit = new Unit();
        cost = 0;

        int number = Random.Range(0, InformationDatabase.i.speciesList.Count);
        int number2 = Random.Range(0, InformationDatabase.i.profList.Count);

        Species getSpecies = InformationDatabase.i.speciesList[number];
        Profession getProf = InformationDatabase.i.profList[number2];

        if (CheckIfPriceIsWrong(getSpecies.cost + getProf.cost))
        {
            GenerateUnit();
            return;
        }
        else
        {
            unit.spr = getSpecies.spr;
            unit.speciesDes = getSpecies.description;
            unit.speciesName = getSpecies.name;
            unit.power += getSpecies.power;
            unit.def += getSpecies.def;
            unit.actionSpeed += getSpecies.actionSpeed;
            unit.epGain += getSpecies.epGain;
            unit.range += getSpecies.range;
            unit.moveSpeed += getSpecies.moveSpeed;
            unit.maxHP += getSpecies.hp;
            unit.maxEP += getSpecies.ep;
            unit.passive = getSpecies.passive;
            cost += getSpecies.cost;


            unit.profName = getProf.name;
            unit.profDes = getProf.description;
            unit.power += getProf.power;
            unit.def += getProf.def;
            unit.actionSpeed += getProf.actionSpeed;
            unit.epGain += getProf.epGain;
            unit.range += getProf.range;
            unit.moveSpeed += getProf.moveSpeed;
            unit.maxHP += getProf.hp;
            unit.maxEP += getProf.ep;

            cost += getProf.cost;

            if (getSpecies.passive.costAmount > 0)
            {
                cost = Mathf.RoundToInt(cost * getSpecies.passive.costAmount);
            }

            if (getSpecies.passive.flatCostReduction > 0)
            {
                cost -= getSpecies.passive.flatCostReduction;
            }

            if (getProf.projectile != null)
            {
                unit.projectile = getProf.projectile;
            }

            if (getProf.active != null)
            {
                unit.active = getProf.active;
            }

            int number3 = Random.Range(0, InformationDatabase.i.namesList.Count);



            unit.unitName = InformationDatabase.i.namesList[number3];

            ;

            image.sprite = unit.spr;
            image.SetNativeSize();
            speciesText.text = unit.speciesName;
            profText.text = unit.profName;
            nameText.text = unit.unitName;
            costText.text = "Cost: " + cost.ToString();
        }

    }

    void BuyUnit()
    {
        party.SpawnUnit(unit);
        player.SpendGold(cost);
        this.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (cost <= player.gold)
        {
            BuyUnit();
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        details.OpenDetails();
        details.UpdateDetails(unit, false);

        UnitDetails[] deets = GameObject.FindObjectsOfType<UnitDetails>();

        for (int i = 0; i < deets.Length; i++)
        {
            if (deets[i].coroutine != null)
            {
                StopCoroutine(deets[i].coroutine);
            }
        }

        UnitCard[] cards = GameObject.FindObjectsOfType<UnitCard>();

        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].coroutine != null)
            {
                StopCoroutine(cards[i].coroutine);
            }
        }

        Unit[] units = GameObject.FindObjectsOfType<Unit>();

        for (int i = 0; i < units.Length; i++)
        {
            if (units[i].coroutine != null)
            {
                StopCoroutine(units[i].coroutine);
            }
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
        coroutine = StartCoroutine(ExitWait());
       
    }

    IEnumerator ExitWait()
    {
        yield return new WaitForSeconds(1f);

        details.CloseDetails();
    }
}

