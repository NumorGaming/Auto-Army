using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCardContainer : MonoBehaviour
{
    // Start is called before the first frame update

    public List<UnitCard> cards;
    public Player player;
    
    public void Reroll(bool free)
    {
        if (player.gold <= 0) return;

        if (!free)
        {
            player.SpendGold(1);
        }

        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].GenerateUnit();
            cards[i].gameObject.SetActive(true);
        }
    }
}
