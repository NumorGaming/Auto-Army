using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitDetails : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI epText;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI defText;
    public TextMeshProUGUI actionSpeedText;
    public TextMeshProUGUI movementSpeedText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI classText;
    public TextMeshProUGUI speciesText;
    public TextMeshProUGUI epGain;
    public TextMeshProUGUI range;
    public TextMeshProUGUI active;
    public TextMeshProUGUI passive;
    public TextMeshProUGUI ai;
    public TextMeshProUGUI status;

    public Image image;

    public GameObject child;

    public Unit currentUnit;

    public Coroutine coroutine;



    public void CloseDetails()
    {

        child.SetActive(false);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OpenDetails();

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

        CloseDetails();
    }

    public void OpenDetails()
    {
        child.SetActive(true);
    }

    public void UpdateDetails(Unit unit, bool statusT)
    {
        currentUnit = unit;

        image.sprite = unit.spr;
        image.SetNativeSize();

        hpText.text = "Health: " + unit.currHP + "/" + unit.maxHP;
        epText.text = "Energy: " + unit.currEP + "/" + unit.maxEP;
        powerText.text = "Power: " + unit.power;
        defText.text = "Defense: " + unit.def;
        actionSpeedText.text = "Speed: " + unit.actionSpeed;
        movementSpeedText.text = "Move: " + unit.moveSpeed;
        nameText.text = unit.unitName;
        classText.text = unit.profName;
        speciesText.text = unit.speciesName;
        epGain.text = "EP Gain: " + unit.epGain;
        range.text = "Range: " + unit.range;
        active.text = "Active: " + unit.active.name;
        passive.text = "Passive: " + unit.passive.name;

        if (!statusT) return;

        if (unit.statuses.Count > 0)
        {
            status.text = "Status: " + unit.statuses[0].status;
        }
        else
        {
            status.text = "Status: ";
        }

        
        //ai;
        //status;
    }


}
