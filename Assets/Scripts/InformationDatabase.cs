using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationDatabase : MonoBehaviour
{

    public static InformationDatabase i;

    public List<Species> speciesList;

    public List<Profession> profList;

    public List<string> namesList;

    // Start is called before the first frame update
    private void Awake()
    {
        i = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


[System.Serializable]
public class Species // species gives a special passive, and also gives stats. // this also determines their appearence
{
    public string name;
    [TextArea]
    public string description;
    public Sprite spr;
    public int hp;
    public int ep;
    public int power;
    public int def;
    public float actionSpeed;
    public float epGain;
    public int range;
    public float moveSpeed;
    public int cost;
    public PassiveAbility passive;
}

[System.Serializable]

public class Profession // class gives a active ability, and also gives stats.
{
    public string name;
    [TextArea]
    public string description;
    public int hp;
    public int ep;
    public int power;
    public int def;
    public float actionSpeed;
    public float epGain;
    public int range;
    public float moveSpeed;
    public int cost;
    public GameObject projectile;
    public ActiveAbility active;
    
}