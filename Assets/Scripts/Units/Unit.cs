using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public enum StatusEffect { None, Burn, Bleed, Shock, Stun, Blind, Slow, Charm, Immobile, Confused, Disarmed, Cracked, Push, Pull, Guard, Focus, Rage, Bloodlust }

public enum Stat { Power, Defense, Aoe, ActionSpeed, EpGain, Range, CritChance, CritDamage, Lifesteal, Movespeed}

public class Unit : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer highlight;

    public Vector3 origin;

    [Header("Stats")]

    public Sprite spr;

    public string unitName;

    public int currHP;
    public int maxHP;
    public int currEP;
    public int maxEP;

    public int power;
    public int originalPower;
    public int passivePower;
    public int auraPower;

    public int def;
    public int originalDefense;
    public int passiveDef;
    public int auraDef;

    public int attackAoe;
    public int originalAoe;
    public int bonusAoe;
    public int auraAoe;

    public float actionSpeed;
    public float originalActionSpeed;
    public float passiveActSpeed;
    public float auraActSpeed;

    public float epGain;
    public float originalEpGain;
    public float bonusEpGain;
    public int auraEpGain;

    public float range;
    public float originalRange;
    public float bonusRange;

    public int critChance = 5;
    public int originalCritChance;
    public int passiveCritChance;
    public int auraCritChance;

    public float critDamage = 1.5f;
    public float originalCritDamage;
    public float passiveCritDamage;
    public float auraCritDamage;

    public float lifesteal;
    public float originalLifesteal;
    public float passiveLifeSteal;
    public float auraLifesteal;

    public float moveSpeed;
    public float originalMoveSpeed;
    public float passiveMoveSpeed;
    public float auraMoveSpeed;

    public string speciesName;
    public string speciesDes;

    public string profName;
    public string profDes;

    public Team team;

    [Header("Battle")]

    public bool paused;

    public Unit target;

    public float distanceToTarget;

    public float actionTimer;

    public bool acting;

    public bool dead;

    public bool dragging;

    public bool clicked;

    public bool attackPriming;
    public bool attacking;
    public float attackPrimeTimer;
    public float attackPrimeTime;
    public float attackTimer;
    public float attackTime;
    public float attackStartSpeed;
    public float attackEndSpeed;
    public float primeStartSpeed;
    public float primeEndSpeed;

    public GameObject projectile;

    public ActiveAbility active;

    public PassiveAbility passive;

    public List<ActiveStatus> statuses;

    public List<StatusEffect> resistedStatuses;

    public Party party;


    Rigidbody2D rigidbody2D;

    public Slider hpSlider;
    public Slider epSlider;

    [Header("Counters")]

    public int hitNumber;
    public int atkNumber;





    //weapon
    //armor
    //helmet

    //passives

    //actives

    UnitDetails details;

    public Coroutine coroutine;

    Coroutine meleeCoroutine;




    // Start is called before the first frame update
    void Start()
    {
        FindTarget();
        currHP = maxHP;
        currEP = 0;

        originalPower = power;
        originalDefense = def;
        originalActionSpeed = actionSpeed;
        originalAoe = attackAoe;
        originalCritChance = critChance;
        originalCritDamage = critDamage;
        originalEpGain = epGain;
        originalLifesteal = lifesteal;
        originalMoveSpeed = moveSpeed;
        originalRange = range;


        details = GameObject.FindGameObjectWithTag("UnitDetails").GetComponent<UnitDetails>();
        rigidbody2D = GetComponent<Rigidbody2D>();


        UpdateHP();
        UpdateEP();

    }

    public void CombatOver()
    {
        transform.position = origin;
        Ressurect();
        RemovePassiveBonusStat();
        ResetAuras();
        AuraStatIncreases();
        statuses.Clear();
        resistedStatuses.Clear();
        hitNumber = 0;
        atkNumber = 0;
        currEP = 0;
        currHP = maxHP;

        if (passive.condition == PassiveCondition.Permanent && passive.type == PassiveType.StatChange && passive.target == PassiveTarget.Self)
        {
            PermanentSelfPassiveStatIncrease();
        }

        if (passive.condition == PassiveCondition.Permanent && passive.type == PassiveType.StatusResistance)
        {
            PermanentStatusResistance();
        }

        rigidbody2D.velocity = Vector2.zero;
    }

    public void CombatStart()
    {
        
        

    }

    void UpdateStats()
    {
        power = originalPower + passivePower + StatusPower() + auraPower;
        def = originalDefense + passiveDef + StatusDefense() + auraDef;
        actionSpeed = Mathf.Clamp(originalActionSpeed + passiveActSpeed + StatusActionSpeed() + auraActSpeed, 0.5f, 10);
        moveSpeed = originalMoveSpeed + passiveMoveSpeed + StatusMoveSpeed() + auraMoveSpeed;
        critChance = originalCritChance + passiveCritChance + StatusCritChance() + auraCritChance;
        critDamage = originalCritDamage + passiveCritDamage + StatusCritDamage() + auraCritDamage;
        lifesteal = originalLifesteal + passiveLifeSteal + StatusLifeSteal() + auraLifesteal;
        epGain = originalEpGain + bonusEpGain;
        range = originalRange + bonusRange;
        attackAoe = originalAoe + bonusAoe;

    }

    int StatusPower()
    {
        int number = 0;

        for (int i = 0; i < statuses.Count; i++)
        {
            if (statuses[i].power != 0)
            {
                number += statuses[i].power;
            }
        }

        return number;
    }

    int StatusDefense()
    {
        int number = 0;

        for (int i = 0; i < statuses.Count; i++)
        {
            if (statuses[i].def != 0)
            {
                number += statuses[i].def;
            }
        }

        return number;
    }

    float StatusActionSpeed()
    {
        float number = 0;

        for (int i = 0; i < statuses.Count; i++)
        {
            if (statuses[i].actionSpeed != 0)
            {
                number += statuses[i].actionSpeed;
            }
        }

        return number;
    }

    float StatusMoveSpeed()
    {
        float number = 0;

        for (int i = 0; i < statuses.Count; i++)
        {
            if (statuses[i].moveSpeed != 0)
            {
                number += statuses[i].moveSpeed;
            }
        }

        return number;
    }

    int StatusCritChance()
    {
        int number = 0;

        for (int i = 0; i < statuses.Count; i++)
        {
            if (statuses[i].critChance != 0)
            {
                number += statuses[i].critChance;
            }
        }

        return number;
    }

    float StatusCritDamage()
    {
        float number = 0;

        for (int i = 0; i < statuses.Count; i++)
        {
            if (statuses[i].critDamage != 0)
            {
                number += statuses[i].critDamage;
            }
        }

        return number;
    }

    float StatusLifeSteal()
    {
        float number = 0;

        for (int i = 0; i < statuses.Count; i++)
        {
            if (statuses[i].lifesteal != 0)
            {
                number += statuses[i].lifesteal;
            }
        }

        return number;
    }


    void TeammateCountStatBuff()
    {



        if (passive.power > 0)
        {
            passivePower = (party.units.Count * passive.power);
        }

        if (passive.def > 0)
        {
            passiveDef = (party.units.Count * passive.def);
        }

        if (passive.moveSpeed > 0)
        {
            passiveMoveSpeed = (party.units.Count * passive.moveSpeed);
        }

        if (passive.actionSpeed > 0)
        {
            passiveActSpeed = (party.units.Count * passive.actionSpeed);
        }

        if (passive.critChance > 0)
        {
            passiveCritChance = (party.units.Count * passive.critChance);
        }

        if (passive.critDamage > 0)
        {
            passiveCritDamage = (party.units.Count * passive.critDamage);
        }

        if (passive.lifesteal > 0)
        {
            passiveLifeSteal = (party.units.Count * passive.lifesteal);
        }

    }


    // Update is called once per frame
    void Update()
    {

        if (paused)
        {
   

            rigidbody2D.velocity = Vector3.zero;

            if (meleeCoroutine != null)
            {
                StopCoroutine(meleeCoroutine);
            }

            
        }

        if (dead || paused) return;

        CheckStatuses();

        UpdateStats();

        if (passive.condition == PassiveCondition.TimeIntervals || passive.condition == PassiveCondition.AfterTime || passive.condition == PassiveCondition.BeforeTime)
        {
            
            CheckTimedPassives();
            
        }

        if (passive.condition == PassiveCondition.TeammateCount && passive.type == PassiveType.StatChange)
        {
            TeammateCountStatBuff();
        }

        if (passive.condition == PassiveCondition.BeforeHitAmount || passive.condition == PassiveCondition.AfterHitAmount || passive.condition == PassiveCondition.BeforeAtkAmount || passive.condition == PassiveCondition.AfterAtkAmount)
        {
            if (passive.type == PassiveType.StatChange)
            {
                HitAtkBasedStatBuffs();
            }
        }

        if (passive.condition == PassiveCondition.BelowHp || passive.condition == PassiveCondition.AboveHp)
        {
            HPThreshHoldPassives();
        }



        if (target != null)
        {
            if (target.currHP <= 0 )
            {
                FindTarget();
            }

            if (target != null)
            {
                distanceToTarget = Vector3.Distance(this.transform.position, target.transform.position);
            }
            
        }
        else
        {


            FindTarget();
        }

        

        if (distanceToTarget <= range + 2 && !acting)
        {
            actionTimer += Time.deltaTime;

            if (actionTimer >= actionSpeed)
            {
                acting = true;
                actionTimer = 0;
                Action();
            }

        }
        else if (!dragging)
        {
            if (target != null && !acting)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);

                if (target.transform.position.x > transform.position.x)
                {
                    spriteRenderer.flipX = false;
                }
                else
                {
                    spriteRenderer.flipX = true;
                }

            }
        }

        if (attackPriming)
        {
            attackPrimeTimer += Time.deltaTime;
        }

        if (attacking)
        {
            attackTimer += Time.deltaTime;
        }
        
    }

    void UpdateHP()
    {
        hpSlider.maxValue = maxHP;
        hpSlider.value = currHP;
        StartCoroutine(ShowHP());
    }

    void UpdateEP()
    {
        epSlider.maxValue = maxEP;
        epSlider.value = currEP;
        StartCoroutine(ShowEP());
    }

    IEnumerator ShowHP()
    {
        hpSlider.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        hpSlider.gameObject.SetActive(false);

    }

    IEnumerator ShowEP()
    {
        epSlider.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        epSlider.gameObject.SetActive(false);

    }

    void CheckStatuses()
    {
        for (int i = 0; i < statuses.Count; i++)
        {
            statuses[i].statusTimer += Time.deltaTime;

            if (statuses[i].statusTimer > statuses[i].statusTime)
            {
                statuses.Remove(statuses[i]);
                i--;
            }
            else if (statuses[i].ticDamage > 0)
            {
                statuses[i].ticTimer += Time.deltaTime;

                if (statuses[i].ticTimer > statuses[i].ticTime)
                {
                    TakeDamage(statuses[i].ticDamage);
                    statuses[i].ticTimer = 0;
                }
            }
            
        }
    }


    public bool CheckIfHasStatus(StatusEffect status)
    {
        for (int i = 0; i < statuses.Count; i++)
        {
            if (statuses[i].status == status)
            {
                return true;
            }
        }

        return false;

    }

    public void RemoveStatus(StatusEffect status)
    {
        for (int i = 0; i < statuses.Count; i++)
        {
            if (statuses[i].status == status)
            {
                statuses.Remove(statuses[i]);
                i--;
            }
        }

    }

    void Action()
    {
        if (dead || paused || CheckIfHasStatus(StatusEffect.Stun)) return;

        if (currEP >= maxEP && active != null)
        {
            StartCoroutine(ActiveMovement());
            currEP = 0;
            UpdateEP();
        }
        else
        {
            BasicAttack();
        }
    }

    void BasicAttack()
    {
        if (dead || paused || CheckIfHasStatus(StatusEffect.Stun)) return;


        atkNumber++;

        if (projectile != null && range > 6)
        {
            RangedAttack();
        }
        else
        {
            meleeCoroutine = StartCoroutine(MeleeAttack());
        }





    }

    void Ability()
    {

    }

    public void TakeDamage(float damageTaken)
    {
        if (dead || paused) return;

        int damage = Mathf.RoundToInt(damageTaken - this.def);

        if (damage <= 0)
        {
            damage = 1;
        }

        currHP -= damage;

        currEP += Mathf.RoundToInt(epGain);

        hitNumber++;

        if (passive.condition == PassiveCondition.AfterEveryHit)
        {
            AfterHitEffects();
        }

        if (currHP <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(FlashRed());
        }

        UpdateHP();

        UpdateEP();

    }

    public void Heal(float heal)
    {
        if (dead || paused) return;

  

        currHP += Mathf.RoundToInt(heal);

        

        if (currHP >= maxHP)
        {
            currHP = maxHP;
            StartCoroutine(FlashGreen());
        }
        else
        {
            StartCoroutine(FlashGreen());
        }

        UpdateHP();

    }

    void Ressurect()
    {

        dead = false;
        currHP = maxHP;
        var sequence = DOTween.Sequence();
        sequence.Join(spriteRenderer.DOFade(1f, 1f));
        sequence.Join(highlight.DOFade(1f, 1f));
    }

    void Die()
    {
        
        dead = true;
        currHP = 0;
        var sequence = DOTween.Sequence();
        sequence.Join(spriteRenderer.DOFade(0f, 1f));
        sequence.Join(highlight.DOFade(0f, 1f));
    }

    IEnumerator FlashRed()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(.25f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    IEnumerator FlashGreen()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        yield return new WaitForSeconds(.25f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }


    void FindTarget()
    {
        

        List<Unit> units = new List<Unit>();

        units.AddRange(GameObject.FindObjectsOfType<Unit>());

        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].currHP <= 0)
            {
                units.Remove(units[i]);
                i--;
            }
        }

        if (team == Team.Ally)
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

  

        target = GetClosestEnemy(units);


    }

    Unit GetClosestEnemy(List<Unit> enemies)
    {
        Unit bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Unit potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }

    private void OnMouseOver()
    {
        details.OpenDetails();
        details.UpdateDetails(this, true);

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

    private void OnMouseExit()
    {
        coroutine = StartCoroutine(ExitWait());
    }

    IEnumerator ExitWait()
    {
        yield return new WaitForSeconds(1f);

        details.CloseDetails();
    }

    public void GoToOrigin()
    {
        transform.position = origin;
    }

    bool RollForCrit()
    {
        int number = Random.Range(0, 101);

        if (number > critChance)
        {
            return false;
        }

        return true;
    }


    void AoeStatus(int aoe, Unit aoeTarget)
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
            float distanceToUnit = Vector3.Distance(aoeTarget.transform.position, units[i].transform.position);

            if (distanceToUnit < aoe)
            {
                GiveStatus(units[i], active.status.status, active.status.statusTime, active.status.statusTimer, active.status.ticDamage, active.status.ticTime, active.status.ticTimer,
                    active.status.power, active.status.def, active.status.lifesteal, active.status.actionSpeed, active.status.moveSpeed, active.status.critDamage, active.status.critChance, active.status.description);
            }

        }

    }

    void AoeAttack(int damage, int aoe, Unit aoeTarget)
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
                float distanceToUnit = Vector3.Distance(aoeTarget.transform.position, units[i].transform.position);

                if (distanceToUnit < aoe)
                {
                    units[i].TakeDamage(damage);

                    if (lifesteal > 0)
                    {
                        Heal(damage * lifesteal);
                    }
                }

            }

    }

    void AoeHeal(int heal, int aoe, Unit aoeTarget)
    {

        List<Unit> units = new List<Unit>();

        units.AddRange(GameObject.FindObjectsOfType<Unit>());

        if (target.team == Team.Enemy)
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
        else
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

        for (int i = 0; i < units.Count; i++)
        {
            float distanceToUnit = Vector3.Distance(aoeTarget.transform.position, units[i].transform.position);

            if (distanceToUnit < aoe)
            {
                units[i].Heal(heal);
            }

        }

    }

    bool CheckForMiss()
    {
        for (int i = 0; i < statuses.Count; i++)
        {
            if (statuses[i].status == StatusEffect.Blind)
            {
                int number = Random.Range(0, 100);

                if (number < 50)
                {
                    return true;
                }
            }
        }

        return false;
    }

    IEnumerator MeleeAttack()
    {
        float speed = 0;
        float t = 0;

        Vector2 primingDirection;
        Vector2 attackingDirection;
        

        if (target.transform.position.x < transform.position.x)
        {
            primingDirection = new Vector2(-1, 0);
            attackingDirection = new Vector2(1, 0);
        }
        else
        {
            primingDirection = new Vector2(1, 0);
            attackingDirection = new Vector2(-1, 0);
        }

        attackPriming = true;

        while (attackPrimeTimer < attackPrimeTime)
        {
     

            t += Time.deltaTime;
            speed = Mathf.Lerp(primeStartSpeed, primeEndSpeed, t);

            rigidbody2D.velocity = primingDirection * speed;

            yield return 0;
        }

        rigidbody2D.velocity = Vector3.zero;

        attackPriming = false;
        attackPrimeTimer = 0;
        attacking = true;

        while (attackTimer < attackTime)
        {

            t += Time.deltaTime;
            speed = Mathf.Lerp(attackStartSpeed, attackEndSpeed, t);

            rigidbody2D.velocity = attackingDirection * speed;

            yield return 0;
        }

        rigidbody2D.velocity = Vector3.zero;

        if (target != null && !CheckForMiss())
        {

            int damage;

            if (RollForCrit())
            {
                damage = Mathf.RoundToInt(power * critDamage);
            }
            else
            {
                damage = power;
            }

            target.TakeDamage(damage);
            currEP += Mathf.RoundToInt(epGain);

            //After Hit

            if (target.passive.condition == PassiveCondition.AfterEveryHit && target.passive.type == PassiveType.Damage && target.passive.target == PassiveTarget.Attacker)
            {
                if (passive.flatDamage > 0)
                {
                    TakeDamage(target.passive.flatDamage);
                }
                else
                {
                    TakeDamage(target.passive.damageByPowerMultiple);
                }

                
            }
            else if (target.passive.condition == PassiveCondition.AfterEveryHit && target.passive.type == PassiveType.Status && target.passive.target == PassiveTarget.Attacker)
            {
                Debug.Log(name);

                GiveStatus(this, target.passive.status.status, target.passive.status.statusTime, target.passive.status.statusTimer, target.passive.status.ticDamage, target.passive.status.ticTime, target.passive.status.ticTimer,
                    target.passive.status.power, target.passive.status.def, target.passive.status.lifesteal, target.passive.status.actionSpeed, target.passive.status.moveSpeed, target.passive.status.critDamage, target.passive.status.critChance, target.passive.status.description);
            }
            //After Atk

            if (passive.condition == PassiveCondition.AfterEveryAtk)
            {
                AfterAtkEffects();
            }

            //Lifesteal

            if (lifesteal > 0)
            {
                Heal(damage * lifesteal);
            }

            ///Aoe strikes

            if (attackAoe > 0)
            {
                AoeAttack(damage, attackAoe, target);
            }

            

        }

        attacking = false;
        attackTimer = 0;

        acting = false;

        UpdateEP();

    }

   
    public void RangedAttack()
    {
        if (!CheckForMiss())
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);

            int damage;

            if (RollForCrit())
            {
                damage = Mathf.RoundToInt(power * critDamage);
            }
            else
            {
                damage = power;
            }

            target.TakeDamage(damage);

            if (passive.condition == PassiveCondition.AfterEveryAtk)
            {
                AfterAtkEffects();
            }

            if (lifesteal > 0)
            {
                Heal(damage * lifesteal);
            }

            newProjectile.GetComponent<Projectile>().SetUp(target, damage, attackAoe);

            currEP += Mathf.RoundToInt(epGain);

            acting = false;

            UpdateEP();
        }

        acting = false;

    }

    Unit FindHealTarget()
    {

        List<Unit> units = new List<Unit>();

        units.AddRange(GameObject.FindObjectsOfType<Unit>());

        

        for (int i = 0; i < units.Count; i++)
        {
            float distanceToUnit = Vector3.Distance(this.transform.position, units[i].transform.position);

            if (distanceToUnit > active.range)
            {
                units.Remove(units[i]);
            }

        }

        if (target.team == Team.Enemy)
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
        else
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

        Unit healUnit = units[0];

        int number = 10000;

        for (int i = 0; i < units.Count; i++)
        {

            Debug.Log(units[i].name);

            if (units[i].currHP < number && !units[i].dead)
            {
                number = units[i].currHP;
                healUnit = units[i];
            }
        }

        Debug.Log(this.team + healUnit.name);

        return healUnit;

    }

    bool CheckIfStatusResisted(StatusEffect status)
    {
        for (int i = 0; i < resistedStatuses.Count; i++)
        {
            if (resistedStatuses[i] == status)
            {
                return true;
            }
        }

        return false;
    }

    void GiveStatus(Unit target, StatusEffect status, float statusTime, float statusTimer, int damage, float ticTime, float ticTimer,
        int power, int defense, float lifesteal, float actionspeed, float movespeed, float critdamage, int critchance, string des)
    {
        if (target.CheckIfHasStatus(status))
        {
            RemoveStatus(status);

            ActiveStatus newStatus = new ActiveStatus();

            newStatus.status = status;
            newStatus.description = des;
            newStatus.statusTime = statusTime;
            newStatus.statusTimer = statusTimer;
            newStatus.ticDamage = damage;
            newStatus.ticTime = ticTime;
            newStatus.ticTimer = ticTimer;
            newStatus.power = power;
            newStatus.def = defense;
            newStatus.lifesteal = lifesteal;
            newStatus.actionSpeed = actionspeed;
            newStatus.moveSpeed = movespeed;
            newStatus.critDamage = critdamage;
            newStatus.critChance = critchance;


            if (!CheckIfStatusResisted(newStatus.status))
            {
                target.statuses.Add(newStatus);
            }
        }
        else
        {
            ActiveStatus newStatus = new ActiveStatus();

            newStatus.status = status;
            newStatus.description = des;
            newStatus.statusTime = statusTime;
            newStatus.statusTimer = statusTimer;
            newStatus.ticDamage = damage;
            newStatus.ticTime = ticTime;
            newStatus.ticTimer = ticTimer;
            newStatus.power = power;
            newStatus.def = defense;
            newStatus.lifesteal = lifesteal;
            newStatus.actionSpeed = actionspeed;
            newStatus.moveSpeed = movespeed;
            newStatus.critDamage = critdamage;
            newStatus.critChance = critchance;

            if (!CheckIfStatusResisted(newStatus.status))
            {
                target.statuses.Add(newStatus);
            }

        }
    }

    void ActiveEffect()
    {
        if (active.target == ActiveAbility.AbilityTarget.Foe)
        {
            if (active.type == ActiveAbility.AbilityType.Damage)
            {

                float damage = power * active.damageByPowerMultiple;

                target.TakeDamage(damage);

                if (lifesteal > 0)
                {
                    Heal(damage * lifesteal);
                }
            }
            else if (active.type == ActiveAbility.AbilityType.Status)
            {

                GiveStatus(target, active.status.status, active.status.statusTime, active.status.statusTimer, active.status.ticDamage, active.status.ticTime, active.status.ticTimer,
                    active.status.power, active.status.def, active.status.lifesteal, active.status.actionSpeed, active.status.moveSpeed, active.status.critDamage, active.status.critChance, active.status.description);

            }
  
        }
        else if (active.target == ActiveAbility.AbilityTarget.Ally)
        {
            if (active.type == ActiveAbility.AbilityType.Heal)
            {
                FindHealTarget().Heal(active.healAmount);
            }
            else if (active.type == ActiveAbility.AbilityType.Status)
            {
                //status effect
            }
        }
        else if (active.target == ActiveAbility.AbilityTarget.Self)
        {
            if (active.type == ActiveAbility.AbilityType.Heal)
            {
                Heal(active.healAmount);
            }
            else if (active.type == ActiveAbility.AbilityType.Status)
            {
                GiveStatus(this, active.status.status, active.status.statusTime, active.status.statusTimer, active.status.ticDamage, active.status.ticTime, active.status.ticTimer,
                    active.status.power, active.status.def, active.status.lifesteal, active.status.actionSpeed, active.status.moveSpeed, active.status.critDamage, active.status.critChance, active.status.description);
            }

        }
        else if (active.target == ActiveAbility.AbilityTarget.FoeAoe)
        {

            if (active.type == ActiveAbility.AbilityType.Damage)
            {
                if (active.damageByPowerMultiple > 0)
                {
                    GameObject newProjectile = Instantiate(active.animation, target.transform.position, Quaternion.identity);

                    int damage = Mathf.RoundToInt(active.damageByPowerMultiple * power);

                    AoeAttack(damage, active.aoe, target);
                }
            }
            else if (active.type == ActiveAbility.AbilityType.Status)
            {

                GameObject newProjectile = Instantiate(active.animation, target.transform.position, Quaternion.identity);

                AoeStatus(active.aoe, target);
            }

            

                
            
        }
        else if (active.target == ActiveAbility.AbilityTarget.Multitarget)
        {
            List<Unit> units = GetMultitargets(active.extraTargets + 1);

            for (int i = 0; i < units.Count; i++)
            {
                if (active.damageByPowerMultiple > 0)
                {
                    int damage = Mathf.RoundToInt(active.damageByPowerMultiple * power);

                    AoeAttack(damage, active.aoe, target);
                }
            }

        }
        else if (active.target == ActiveAbility.AbilityTarget.SelfAoe)
        {
            if (active.type == ActiveAbility.AbilityType.Heal)
            {
                AoeHeal(active.healAmount, active.aoe, this);
            }
        }
        else if (active.target == ActiveAbility.AbilityTarget.FullAllyAoe)
        {
            if (active.type == ActiveAbility.AbilityType.Status)
            {
                for (int i = 0; i < party.units.Count; i++)
                {
                    GiveStatus(party.units[i], active.status.status, active.status.statusTime, active.status.statusTimer, active.status.ticDamage, active.status.ticTime, active.status.ticTimer,
                    active.status.power, active.status.def, active.status.lifesteal, active.status.actionSpeed, active.status.moveSpeed, active.status.critDamage, active.status.critChance, active.status.description);
                }
            }
        }
        else if (active.target == ActiveAbility.AbilityTarget.FullFoeAoe)
        {
            if (active.type == ActiveAbility.AbilityType.Damage)
            {
                for (int i = 0; i < target.party.units.Count; i++)
                {
                    if (active.damageByPowerMultiple > 0)
                    {
                        int damage = Mathf.RoundToInt(active.damageByPowerMultiple * power);

                        target.party.units[i].TakeDamage(damage);
                    }
                }
            }
        }






        acting = false;
    }

    List<Unit> GetMultitargets(int targetAmount)
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
            if (units.Count > targetAmount)
            {
                units.Remove(units[i]);
                i--;
            }
        }

        return units;

    }

    IEnumerator ActiveMovement()
    {
        float speed = 0;
        float t = 0;

        Vector2 primingDirection;
        Vector2 attackingDirection;


        if (target.transform.position.x < transform.position.x)
        {
            primingDirection = new Vector2(-1, 0);
            attackingDirection = new Vector2(1, 0);
        }
        else
        {
            primingDirection = new Vector2(1, 0);
            attackingDirection = new Vector2(-1, 0);
        }

        attackPriming = true;

        while (attackPrimeTimer < attackPrimeTime)
        {
      

            t += Time.deltaTime;
            speed = Mathf.Lerp(primeStartSpeed, primeEndSpeed, t);

            rigidbody2D.velocity = primingDirection * speed;

            yield return 0;
        }

        rigidbody2D.velocity = Vector3.zero;

        attackPriming = false;
        attackPrimeTimer = 0;
        attacking = true;

        while (attackTimer < attackTime)
        {

            t += Time.deltaTime;
            speed = Mathf.Lerp(attackStartSpeed, attackEndSpeed, t);

            rigidbody2D.velocity = attackingDirection * speed;

            yield return 0;
        }

        rigidbody2D.velocity = Vector3.zero;

        ActiveEffect();

        attacking = false;
        attackTimer = 0;

        

    }


    void AfterHitEffects()
    {
        if (passive.type == PassiveType.StatChange)
        {
            SingularPassiveStatIncrease();
        }

        if (passive.type == PassiveType.Heal)
        {
            Heal(passive.flatHeal);
        }

        if (passive.type == PassiveType.Status)
        {
            if (passive.target == PassiveTarget.Attacker)
            {

            }
        }
    }


    void AfterAtkEffects()
    {
        if (passive.type == PassiveType.StatChange)
        {
            SingularPassiveStatIncrease();
        }

        if (passive.type == PassiveType.Damage)
        {
            if (passive.target == PassiveTarget.CurrentTarget)
            {
                if (passive.damageByPowerMultiple > 0)
                {
                    target.TakeDamage(power * passive.damageByPowerMultiple);
                }
                else
                {
                    target.TakeDamage(passive.flatDamage);
                }

            }
        }
        
    }

    void ResetAuras()
    {
        auraCritDamage = 0;
        auraPower = 0;
        auraCritChance = 0;
        auraActSpeed = 0;
        auraAoe = 0;
        auraDef = 0;
        auraEpGain = 0;
        auraLifesteal = 0;
        auraMoveSpeed = 0;
        
    }

    void AuraStatIncreases()
    {
        if (passive.target == PassiveTarget.EveryEnemy)
        {
            for (int i = 0; i < target.party.units.Count; i++)
            {
                target.party.units[i].auraCritDamage += passive.critDamage;

                target.party.units[i].auraCritChance += passive.critChance;

                target.party.units[i].auraLifesteal += passive.lifesteal;

                target.party.units[i].auraPower += passive.power;

                target.party.units[i].auraDef += passive.def;

                target.party.units[i].auraMoveSpeed += passive.moveSpeed;

                target.party.units[i].auraActSpeed += passive.actionSpeed;
            }
        }
        else if (passive.target == PassiveTarget.EveryAlly)
        {
            for (int i = 0; i < party.units.Count; i++)
            {

                Debug.Log(party.units[i].name + party.units[i].auraPower);

                party.units[i].auraCritDamage += passive.critDamage;

                party.units[i].auraCritChance += passive.critChance;

                party.units[i].auraLifesteal += passive.lifesteal;

                party.units[i].auraPower += passive.power;

                party.units[i].auraDef += passive.def;

                party.units[i].auraMoveSpeed += passive.moveSpeed;

                party.units[i].auraActSpeed += passive.actionSpeed;

                Debug.Log(party.units[i].name + party.units[i].auraPower);
            }
        }
    }

    void PermanentSelfPassiveStatIncrease()
    {

         passiveCritDamage += passive.critDamage;

         passiveCritChance += passive.critChance;

         bonusAoe += passive.aoe;

         passiveLifeSteal += passive.lifesteal;

    }

    void HitAtkBasedStatBuffs()
    {

        if (passive.condition == PassiveCondition.BeforeHitAmount)
        {
            if (hitNumber > passive.hitAmount)
            {
                RemovePassiveBonusStat();
            }
            else
            {
                SingularPassiveStatIncrease();
            }
        }
        else if (passive.condition == PassiveCondition.AfterHitAmount)
        {
            if (hitNumber > passive.hitAmount)
            {
                SingularPassiveStatIncrease();
            }
            else
            {
                RemovePassiveBonusStat();
            }
        }
        else if (passive.condition == PassiveCondition.BeforeAtkAmount)
        {
            if (atkNumber > passive.atkAmount)
            {
                RemovePassiveBonusStat();
            }
            else
            {
                SingularPassiveStatIncrease();
            }
        }
        else if (passive.condition == PassiveCondition.AfterAtkAmount)
        {
            if (atkNumber > passive.atkAmount)
            {
                SingularPassiveStatIncrease();
            }
            else
            {
                RemovePassiveBonusStat();
            }
        }


    }

    void PermanentStatusResistance()
    {
        resistedStatuses.Add(passive.resistedEffect);
    }

    

    Unit GetRandomAlly()
    {
        List<Unit> units = new List<Unit>();

        units.AddRange(GameObject.FindObjectsOfType<Unit>());

        if (target.team == Team.Enemy)
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
        else
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

        int number = Random.Range(0, units.Count);

        return units[number];
    }

    Unit GetRandomEnemy()
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

        int number = Random.Range(0, units.Count);

        return units[number];
    }

    void CheckTimedPassives()
    {
        if (passive.ticTime > 0)
        {
            passive.ticTimer += Time.deltaTime;
        }  
        else
        {
            return;
        }

        if (passive.condition == PassiveCondition.TimeIntervals)
        {
            if (passive.ticTimer > passive.ticTime)
            {
                passive.ticTimer = 0;

                if (passive.type == PassiveType.Heal)
                {
                    if (passive.target == PassiveTarget.Self)
                    {
                        Heal(passive.flatHeal);
                    }
                    else if (passive.target == PassiveTarget.RandomAlly)
                    {
                        GetRandomAlly().Heal(passive.flatHeal);
                    }
                    else if (passive.target == PassiveTarget.EveryAlly)
                    {
                        for (int i = 0; i < target.party.units.Count; i++)
                        {
                            target.party.units[i].Heal(passive.flatHeal);
                        }
                    }

                }

                if (passive.type == PassiveType.Damage)
                {
                    if (passive.target == PassiveTarget.RandomEnemy)
                    {
                        GetRandomEnemy().TakeDamage(passive.flatDamage);
                    }
                    else if (passive.target == PassiveTarget.EveryEnemy)
                    {
                        for (int i = 0; i < target.party.units.Count; i++)
                        {
                            target.party.units[i].TakeDamage(passive.flatDamage);
                        }
                    }
                    
                }

                if (passive.type == PassiveType.StatChange)
                {

                   IncrementalPassiveStatIncrease();

                }

            }
        }
        
        if (passive.condition == PassiveCondition.BeforeTime)
        {
            if (passive.ticTimer > passive.ticTime && passive.timePassiveDeactivated == false)
            {
                passive.timePassiveDeactivated = true;

                if (passive.type == PassiveType.StatChange)
                {

                    RemovePassiveBonusStat();

                }

            }
            else if (passive.ticTimer < passive.ticTime && passive.timePassiveActivated == false)
            {
                passive.timePassiveActivated = true;

                if (passive.type == PassiveType.StatChange)
                {

                    SingularPassiveStatIncrease();

                }

            }
        }

        if (passive.condition == PassiveCondition.AfterTime)
        {
            if (passive.ticTimer < passive.ticTime && passive.timePassiveDeactivated == false)
            {
                passive.timePassiveDeactivated = true;

                if (passive.type == PassiveType.StatChange)
                {
                    RemovePassiveBonusStat();
                }

            }
            else if (passive.ticTimer > passive.ticTime && passive.timePassiveActivated == false)
            {
                passive.timePassiveActivated = true;

                if (passive.type == PassiveType.StatChange)
                {
                    SingularPassiveStatIncrease();
                }

            }
        }


    }



    void RemovePassiveBonusStat()
    {
        passiveCritDamage = 0;

        passiveCritChance = 0;

        passiveMoveSpeed = 0;

        passivePower = 0;

        passiveDef = 0;

        passiveActSpeed = 0;

        passiveLifeSteal = 0;

        bonusAoe = 0;
    }

    void SingularPassiveStatIncrease()
    {
        passiveCritDamage = passive.critDamage;

        passiveCritChance = passive.critChance;

        passiveMoveSpeed = passive.moveSpeed;

        passivePower = passive.power;

        passiveDef = passive.def;

        passiveActSpeed = passive.actionSpeed;

        passiveLifeSteal = passive.lifesteal;

        bonusAoe = passive.aoe;
    }

    void IncrementalPassiveStatIncrease()
    {
        passiveCritDamage += passive.critDamage;

        passiveCritChance += passive.critChance;

        passiveMoveSpeed += passive.moveSpeed;

        passivePower += passive.power;

        passiveDef += passive.def;

        passiveActSpeed += passive.actionSpeed;

        passiveLifeSteal += passive.lifesteal;

        bonusAoe += passive.aoe;
    }


    void HPThreshHoldPassives()
    {
        if (passive.condition == PassiveCondition.BelowHp)
        {
            if (currHP < maxHP * passive.hpThreshold)
            {
                if (passive.type == PassiveType.StatChange)
                {
                    SingularPassiveStatIncrease();
                }

                if (passive.type == PassiveType.Heal && !passive.oneTimeHealActivated)
                {
                    passive.oneTimeHealActivated = true;

                    if (passive.flatHeal > 0)
                    {
                        Heal(passive.flatHeal);
                    }

                    if (passive.flatEpHeal > 0)
                    {
                        currEP += passive.flatEpHeal;
                    }

                    
                }
            }
            else
            {
                if (passive.type == PassiveType.StatChange)
                {
                    RemovePassiveBonusStat();
                }
            }
        }

        if (passive.condition == PassiveCondition.AboveHp)
        {
            if (currHP > maxHP * passive.hpThreshold)
            {
                if (passive.type == PassiveType.StatChange)
                {
                    SingularPassiveStatIncrease();
                }
            }
            else
            {
                if (passive.type == PassiveType.StatChange)
                {
                    RemovePassiveBonusStat();
                }
            }
        }



    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "UnitBlock" && team == Team.Ally)
        {

            transform.position = origin;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "UnitBlock" && team == Team.Ally)
        {
            transform.position = origin;
        }
    }

}


public enum Team { Ally, Enemy, Neutral}

public enum PassiveCondition { Permanent, TimeIntervals, BelowHp, AboveHp, AfterTime, BeforeTime, BeforeHitAmount, AfterHitAmount, BeforeAtkAmount, AfterAtkAmount, TeammateCount, AfterEveryAtk, AfterEveryHit}

public enum PassiveType {StatusResistance, Damage, DamageAfterAtk, StatChange, Heal, Discount, Status}

public enum PassiveTarget { None, Attacker, CurrentTarget, RandomAlly, RandomEnemy, Self, EveryEnemy, EveryAlly}

[System.Serializable]
public class PassiveAbility
{
    public string name;
    [TextArea]
    public string description;
    public PassiveType type;
    public PassiveCondition condition;
    public PassiveTarget target;
    [Header("Healing")]
    public int flatHeal;
    public int flatEpHeal;
    public bool oneTimeHealActivated;
    [Header("Stats")]
    public float critDamage;
    public int critChance;
    public float moveSpeed;
    public int power;
    public int def;
    public float lifesteal;
    public float actionSpeed;
    [Header("Time")]
    public float ticTime;
    public float ticTimer;
    public bool timePassiveActivated;
    public bool timePassiveDeactivated;
    [Header("Health")]
    public float hpThreshold;
    [Header("Combat")]
    public int hitAmount;
    public int atkAmount;
    [Header("Damage")]
    public float damageByPowerMultiple;
    public float flatDamage;
    [Header("Range")]
    public int range;
    public int aoe;
    [Header("Status")]
    public StatusEffectContainer status;
    public StatusEffect resistedEffect;
    [Header("Discount")]
    public float costAmount;
    public int flatCostReduction;

}
