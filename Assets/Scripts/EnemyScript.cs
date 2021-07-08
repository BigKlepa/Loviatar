using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    //healthi hommat @Joni
    public int maxHealth = 3;
    public int currentHealth;

    //slowi ja vulnerable combattia varten
    //toistaseks vulnerable vihollinen ottaa tupla damagetta
    // @Joni
    public bool slowed;
    public bool vulnerable;
    public bool stuck;
    public bool rage;
    public float slowTimer = 0;
    public float vulnerableTimer = 0;
    public float slowCD = 4.0f;
    public float vulnerableCD = 0.5f;
    public float attackCD = 2.0f;
    public float attackTimer = 0;
    

    public string enemyName;
    public int baseAttack;
    public float moveSpeed = 1.5f;
    public float slowedSpeed = 0.5f;

    //jahtaus @Sakari
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public GameObject attackPoint;
    public Transform homePosition;
    float horizontalMove = 0f;

    //roamaus  @Sakari
    public Transform[] moveSpots;
    private int randomSpot;
    private float waitTime;
    public float startWaitTime;
    public bool roaming = false;


    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        target = GameObject.FindWithTag("Player").transform;

        randomSpot = Random.Range(0, moveSpots.Length);

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;

        CheckDistance();
        CheckSlow();
        CheckVulnerable();






    }

    void CheckDistance()
    {
        animator.SetBool("IsMoving", true);




        //pientä lisäystä toho AI:hin @Joni
         //jatkaa jahtaamista vaikka onki hidastunu
        if (rage) { chaseRadius = 100f; }
        else { chaseRadius = 2.5f; }        // hidastettua mörköä ei siis pääse karkuun
        if (!stuck)
        {
            if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
            {
                rage = true;
               if (target.position.x < transform.position.x)
                {
                    animator.SetFloat("MoveX", -1f);
                }
                if (target.position.x > transform.position.x)
                {
                    animator.SetFloat("MoveX", 1f);
                }
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * 2.0f * Time.deltaTime);
                if (Mathf.Abs(target.position.x - transform.position.x) > (attackRadius * 1.4f))
                {
                   

                }
                if (Vector2.Distance(target.position, transform.position) <= attackRadius && attackTimer > attackCD)
                {
                    Attack();
                }
            }
            else
            {
              
                RoamAround();

            }
        }
    }

    void RoamAround()
    {
        if (!stuck && !rage)
        {
            transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, moveSpeed * Time.deltaTime);
            animator.SetFloat("MoveX", (moveSpots[randomSpot].position.x - transform.position.x));
            animator.SetFloat("MoveY", (moveSpots[randomSpot].position.y - transform.position.y));
            if (Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
            {
                if (waitTime <= 0)
                {
                    randomSpot = Random.Range(0, moveSpots.Length);

                    animator.SetFloat("MoveX", (target.position.x - transform.position.x));
                    animator.SetFloat("MoveY", (target.position.y - transform.position.y));
                    waitTime = startWaitTime;
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
        }
    }
    void CheckSlow()
    {
        slowTimer += Time.deltaTime;
        if ( slowTimer > slowCD)
        {
            slowed = false;
        }
        if (!vulnerable)
        {
            if (slowed)
            { moveSpeed = 0.5f; }
            else
            { moveSpeed = 1f; }
        }
    }
    void CheckVulnerable()
    {
        vulnerableTimer += Time.deltaTime;
        if (vulnerableTimer > vulnerableCD)
        {
            vulnerable = false;
        }
        if (vulnerable)
        {
            moveSpeed = 0.0f;
        }
        
    }
    void TakeDamage()
    {
        if (currentHealth < 0) 
        {
            stuck = true;
            Destroy(gameObject);
            return;
        }
        rage = true;
        currentHealth--;
        stuck = true;
        if (currentHealth == 0)
        {
            animator.SetTrigger("Death");
            Invoke("Die", 1f);
        }
        else
        {
            animator.SetTrigger("TakeDamage");
            Invoke("UnStuck", 1.0f);
            animator.SetTrigger("Rage");
        }


    
        if (vulnerable)
        {
            currentHealth--;
        }
    }
    void Die()
    {

        Destroy(gameObject);
    }

    void Attack()
    {
        attackTimer = 0;
        animator.SetTrigger("Attack");
        Collider2D[] targets = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRadius);
        if (targets == null)
        {
            Debug.Log("Targets NULL, eli ohi meni");
            return;
        }
        
        foreach (Collider2D target in targets)
        {
            target.GetComponent<PlayerController>().takeDamage();

            Debug.Log("Lehma teki damagee pelaajaa");
            
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Trap")
        {
            Debug.Log("Lehma hit Trap");
            slowed = true;
            vulnerable = true;
            vulnerableTimer = 0;
            slowTimer = 0;
            //

        }
        if (collision.tag == "Bullet")
        {
            Debug.Log("Lehma hit Bullet");
            vulnerableTimer = 0;
            TakeDamage();
        }

        if (collision.tag == "Axe")
        {
            TakeDamage();
            Debug.Log("Sai kirveestä");

        }
        //testausta lähinnä
        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRadius);
    }
    void UnStuck()
    {
        stuck = false;
    }
}
