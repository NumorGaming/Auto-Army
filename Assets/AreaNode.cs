using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaNode : MonoBehaviour
{

    public Image environment;
    public Image background;
    public Image boss;
    public GameObject bossObj;

    public void Select()
    {
        background.color = Color.green;
    }

    public void DeSelect()
    {
        background.color = Color.white;
    }


}
