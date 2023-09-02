using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : MonoBehaviour
{

    public GameObject blankUnit;
    public float radius;
    public List<GameObject> unitObjects;
    public List<Unit> units;

    int spawnUnitCost;

    public Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("GameController").GetComponent<Player>();
    }

    public void ClearUnits()
    {
        for (int i = 0; i < unitObjects.Count; i++)
        {
            GameObject.Destroy(unitObjects[i]);
        }

        unitObjects.Clear();
        units.Clear();
    }

    public bool CheckIfPartyDead()
    {
        int number = 0;


        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].dead)
            {
                number++;
            }
        }

        if (number >= units.Count)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void SpawnUnit(Unit unit)
    {

        GameObject newUnit = Instantiate(blankUnit, Random.insideUnitSphere * radius + transform.position, Quaternion.identity, this.transform);

        Unit newUnitUnit = newUnit.GetComponent<Unit>();

        newUnitUnit.spr = unit.spr;
        newUnitUnit.speciesName = unit.speciesName;
        newUnitUnit.power += unit.power;
        newUnitUnit.def += unit.def;
        newUnitUnit.actionSpeed += unit.actionSpeed;
        newUnitUnit.epGain += unit.epGain;
        newUnitUnit.range += unit.range;
        newUnitUnit.moveSpeed += unit.moveSpeed;
        newUnitUnit.maxHP += unit.maxHP;
        newUnitUnit.maxEP += unit.maxEP;
        newUnitUnit.profName = unit.profName;
        newUnitUnit.origin = newUnitUnit.transform.position;
        newUnitUnit.projectile = unit.projectile;
        newUnitUnit.active = unit.active;
        newUnitUnit.passive = unit.passive;
        newUnitUnit.party = this;


        newUnitUnit.hatPos = unit.hatPos;
        newUnitUnit.wpnPos = unit.wpnPos;
        newUnitUnit.highlightPos = unit.highlightPos;
        newUnitUnit.uiPos = unit.uiPos;

        if (unit.wpn != null)
        {
            newUnitUnit.wpn = unit.wpn;
        }

        if (unit.hat != null)
        {
            newUnitUnit.hat = unit.hat;
        }

        newUnitUnit.UpdateUnitSprites();

        newUnit.name = unit.unitName;

        newUnitUnit.unitName = unit.unitName;

        unitObjects.Add(newUnit);
        units.Add(newUnitUnit);
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

    public void CreateRandomUnit()
    {
        Unit unit = new Unit();

        int number = Random.Range(0, InformationDatabase.i.speciesList.Count);

        Species getSpecies = InformationDatabase.i.speciesList[number];

        int number2 = Random.Range(0, InformationDatabase.i.profList.Count);

        Profession getProf = InformationDatabase.i.profList[number2];


        if (CheckIfPriceIsWrong(getSpecies.cost + getProf.cost))
        {
            

            CreateRandomUnit();
            return;
        }
        else
        {
            

            unit.spr = getSpecies.spr;
            unit.speciesName = getSpecies.name;
            unit.speciesDes = getSpecies.description;
            unit.power += getSpecies.power;
            unit.def += getSpecies.def;
            unit.actionSpeed += getSpecies.actionSpeed;
            unit.epGain += getSpecies.epGain;
            unit.range += getSpecies.range;
            unit.moveSpeed += getSpecies.moveSpeed;
            unit.maxHP += getSpecies.hp;
            unit.maxEP += getSpecies.ep;
            unit.passive = getSpecies.passive;
            spawnUnitCost += getSpecies.cost;



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
            spawnUnitCost += getProf.cost;

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

            SpawnUnit(unit);
        }

    }

}
