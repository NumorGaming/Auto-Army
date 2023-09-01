using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    public int gold;
    public int level;
    public TextMeshProUGUI goldText;

    public void SpendGold(int spent)
    {
        gold -= spent;
        goldText.text = "Gold: " + gold;
    }

    public void GainGold(int gain)
    {
        gold += gain;
        goldText.text = "Gold: " + gold;
    }

}
