using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RerollButton : MonoBehaviour, IPointerDownHandler
{

    public UnitCardContainer container;

    public void OnPointerDown(PointerEventData eventData)
    {
        container.Reroll(false);
    }

}
