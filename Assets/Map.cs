using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{

    public List<AreaNode> areaNodes;
    public List<Image> lines;
    public List<Environment> envList;
    public Player player;


    public void CreateMap()
    {
        for (int i = 0; i < 13; i++)
        {
            if (i == 2 || i == 5 || i == 8 || i == 11)
            {
                BossNode(i);
            }
            else if (i == 12)
            {
                BossNode(i);
            }
            else
            {
                NormalNode(i);
            }

            
        }

    }

    public void OpenEnvironments()
    {

        for (int i = 0; i < envList.Count; i++)
        {
           envList[i].environmentObj.SetActive(false);
        }

        for (int i = 0; i < envList.Count; i++)
        {

            if (i == player.level - 1)
            {
                envList[i].environmentObj.SetActive(true);
            }

        }
    }

    void NormalNode(int i)
    {

        int number = Random.Range(0, InformationDatabase.i.environmentList.Count);

        Environment getEnv = InformationDatabase.i.environmentList[number];

        areaNodes[i].environment.sprite = getEnv.spr;

        
        envList.Add(getEnv);
        

    }

    void BossNode(int i)
    {

        int number = Random.Range(0, InformationDatabase.i.environmentList.Count);

        Environment getEnv = InformationDatabase.i.environmentList[number];

        int number2 = Random.Range(0, InformationDatabase.i.bossesList.Count);

        GameObject getBoss = InformationDatabase.i.bossesList[number2];

        areaNodes[i].environment.sprite = getEnv.spr;
        areaNodes[i].boss.sprite = getBoss.GetComponent<Unit>().spr;

        
        envList.Add(getEnv);
        
    }


    public void ClearedMapNodes()
    {
        for (int i = 0; i < areaNodes.Count - 1; i++)
        {
            if (i < player.level)
            {
                areaNodes[i].Select();
            }
            else
            {
                areaNodes[i].DeSelect();
            }
        }

        for (int i = 0; i < areaNodes.Count - 1; i++)
        {
            if (i < player.level - 1)
            {
                lines[i].color = Color.green;
            }
            else
            {
                lines[i].color = Color.white;
            }
        }



    }


}




[System.Serializable]
public class Environment
{
    public string name;
    public Sprite spr;
    public GameObject environmentObj;
}