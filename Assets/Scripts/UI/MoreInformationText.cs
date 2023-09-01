using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MoreInformationText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool species;
    public bool profession;
    public bool hp;
    public bool ep;
    public bool power;
    public bool def;
    public bool actspd;
    public bool mvspd;
    public bool epgain;
    public bool range;
    public bool active;
    public bool passive;
    public bool ai;
    public bool status;

    public TextMeshProUGUI infoText;
    public GameObject infoObj;
    public UnitDetails details;
    public Unit unit;

 
    public void OnPointerEnter(PointerEventData eventData)
    {
        infoObj.SetActive(true);

        unit = details.currentUnit;

        if (species)
        {
            infoText.text = unit.speciesDes;
        }
        else if (profession)
        {
            infoText.text = unit.profDes;
        }
        else if (hp)
        {
            infoText.text = "Units are removed from battle when their HP drops to 0. Your units respawn after you win battles.";
        }
        else if (ep)
        {
            infoText.text = "When a unit's energy fills up they use their active ability. Energy is gained after attacking or being hit.";
        }
        else if (power)
        {
            infoText.text = "Power determines how much damage most of your attacks do.";
        }
        else if (def)
        {
            infoText.text = "Defense lowers incoming damage by a flat amount. Incoming damage cannot go below 1.";
        }
        else if (actspd)
        {
            infoText.text = "Action speed determines how often a unit acts. You want this stat to go down rather than up.";
        }
        else if (mvspd)
        {
            infoText.text = "Move speed determines how quickly a unit moves across the battlefield.";
        }
        else if (epgain)
        {
            infoText.text = "EP gain determines how much your ep is increased when you attack or are hit.";
        }
        else if (range)
        {
            infoText.text = "Range determines how far away a unit can attack from.";
        }
        else if (active)
        {
            infoText.text = unit.active.description;
        }
        else if (passive)
        {
            infoText.text = unit.passive.description;
        }
        else if (ai)
        {
            infoText.text = "AI determines how a unit acts during battle. NOT IMPLEMENTED";
        }
        else if (status)
        {
            if (unit.statuses.Count > 0)
            {
                infoText.text = unit.statuses[0].description;
            }
            else
            {
                infoText.text = "This unit does not have any status effects.";
            }
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoObj.SetActive(false);
    }


}
