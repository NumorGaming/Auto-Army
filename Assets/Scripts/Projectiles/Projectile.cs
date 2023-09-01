using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public Unit target;

    public int damage;

    public float moveSpeed;

    public bool aoe;

    public float aoeDistance;

    Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
        }

        
    }

    public void SetUp(Unit unit, int damage, float aoeDistance)
    {
        target = unit;

        this.damage = damage;

        this.aoeDistance = aoeDistance;

        if (this.aoeDistance > 0)
        {
            aoe = true;
        }
        else
        {
            aoe = false;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject == target.gameObject)
        {

            if (aoe)
            {
                List<Unit> units = new List<Unit>();

                units.AddRange(GameObject.FindObjectsOfType<Unit>());

                if (target.team == Team.Enemy)
                {
                    for (int i = 0; i < units.Count; i++)
                    {
                        if (units[i].team == Team.Ally)
                        {
                            units.Remove(units[i]);
                            i--;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < units.Count; i++)
                    {

                        if (units[i].team == Team.Enemy)
                        {
                            units.Remove(units[i]);
                            i--;
                        }
                    }
                }

                for (int i = 0; i < units.Count; i++)
                {
                    float distanceToUnit = Vector3.Distance(this.transform.position, units[i].transform.position);

                    if (distanceToUnit < aoeDistance)
                    {
                        units[i].TakeDamage(damage);
                    }

                }

                anim.SetBool("Explode", true);

            }
            else
            {
                target.TakeDamage(damage);

                GameObject.Destroy(gameObject);
            }


            
        }

        


    }

    void DestroyAfterAnimation()
    {
        GameObject.Destroy(gameObject);
    }


}
