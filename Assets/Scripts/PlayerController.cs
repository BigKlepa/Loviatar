using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using VIDE_Data;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 3.0f;
    public float knockback = 2f;
    public GameObject ansaPrefab;

    //timerit
    public float ansaTimer = 4.0f;
    public float ansaCD = 4.0f;
    public float ansaSetupTimer = 1.5f;
    public float shootGunTimer = 0.5f;
    public float reloadGunTimer = 2.0f;
    public float gunTimer = 0f;
    public float reloadDuration = 2.0f;
    public Rigidbody2D projectile;
    public float bulletSpeed;


    //lampputestaushommeli
    public GameObject muzzleFlare;
    public GameObject ansaPoint;
    public GameObject firePoint;
    public GameObject bulletPrefab;
    //nä booleanit estää pelaajaa liikkumasta
    public bool settingTrap = false;
    public bool shootingGun = false;
    public bool alive = true;
    public bool pyssyLadattu = true;
    public bool reloading = false;
    public bool frozen = false;
    public bool attacking;
    public bool XorY;  //x = true

    //healthi jne
    public int maxHP = 2;
    public int currentHP;

    public Animator playerAnim;
    public Animator pyssyAnim;
    public Rigidbody2D rb;
    public DialogManager dm;
    private WAudio soundMngr;

    //dialogihommia WIP
    public VIDE_Assign inTrigger;
    public float dialogueRange = 2.0f;
    public Transform dialogueRinki;
    Vector2 movement;
    Vector2 lastMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        soundMngr = GetComponent<WAudio>();
        
    }
    void Update()
    {

        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            playerAnim.SetFloat("LastMoveX", Input.GetAxisRaw("Horizontal"));
            lastMovement.x = Input.GetAxisRaw("Horizontal");
            XorY = true;
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            playerAnim.SetFloat("LastMoveY", Input.GetAxisRaw("Vertical"));
            lastMovement.y = Input.GetAxisRaw("Vertical");
            XorY = false;
        }

        
            if ( !attacking && !settingTrap && !shootingGun && !reloading && !frozen && alive )
            {
                movement.x = Input.GetAxisRaw("Horizontal");
                movement.y = Input.GetAxisRaw("Vertical");
                playerAnim.SetFloat("Horizontal", movement.x);
                playerAnim.SetFloat("Vertical", movement.y);
                playerAnim.SetFloat("Speed", movement.sqrMagnitude);
            //ansan laittaminen. Se on nyt invokes tuol ni tä on vähä tämmöne, tietteks semmone
            
                if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Joystick1Button1) && ansaTimer > ansaCD)
                {
                    playerAnim.SetTrigger("Trap");
                    Invoke("setTrap", 1.5f);
                    ansaTimer = 0;
                    settingTrap = true;

                }

            //sit pysyl anpumine 
                if ((Input.GetKey(KeyCode.V) || Input.GetKey(KeyCode.Joystick1Button0)) && pyssyLadattu)
                {
                    
                    shootGun();
                }

            //sit pysyl lataamine
                if ((Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.Joystick1Button2)) && !pyssyLadattu)
                {
                    reloadGun();
                    reloading = true;
                    pyssyLadattu = true;
                    reloadGunTimer = 0;
                }
                if ((Input.GetKeyDown(KeyCode.L) || Input.GetKey(KeyCode.Joystick1Button3)))
                {
                    axeSwing();
                //katotaas
                }
                if ((Input.GetKeyDown(KeyCode.K)))
                {
                    dodgeRoll();
                }
            
        }
        
        if (frozen) { playerAnim.StartPlayback();  }
            else { playerAnim.StopPlayback(); }
    }

    // Update is called once per frame

    // tääl on pelkät liikkumiset ja timerit
    public void FixedUpdate()
    {
        //timerit yms tähän alkuu sitte ni tulee päivitettyä joka kierros
        //toim.huom. ei taida olla väliä päivitetäänkö noita timereitä fixedUpdatessa vai Updatessa
        ansaTimer += Time.deltaTime;
        gunTimer += Time.deltaTime;
        reloadGunTimer += Time.deltaTime;

        Vector3 charScale = transform.localScale;
        if (alive)
        {
            if (!attacking && !settingTrap && !shootingGun && !reloading && !frozen && alive)
            {
                rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

            }
            if (ansaTimer > ansaSetupTimer)      { settingTrap = false; }
            if (gunTimer > shootGunTimer )       { shootingGun = false; }
            if (reloadGunTimer > reloadDuration) { reloading = false;   }
        }
    }


    void axeSwing ()
    {
        Vector2 kirvesVektori = getAttackVektori();
        soundMngr.playSwing();
        playerAnim.SetTrigger("attack");
        attacking = true;
        Invoke("attackFalse", 0.5f);
    }

    void attackFalse()
    {
        attacking = false;
    }
    public void setTrap()
    {
        // nä kommentoidut kamat oli sitä tilesetin lukitusta varten. ne nyt on täs.
       // float ansaX = (float)Math.Floor(ansaPoint.transform.position.x) + 0.5f;
       // float ansaY = (float)Math.Floor(ansaPoint.transform.position.y) + 0.5f;
       // Vector3 vektori = new Vector3(ansaX, ansaY, transform.position.z);

        Instantiate(ansaPrefab, ansaPoint.transform.position, transform.rotation);
    }

    public void shootGun()
    {
        Vector2 pysyVektori = getAttackVektori();
        playerAnim.SetTrigger("Shoot"); //nä kaikki rojut vois varmaa laittaa tonne metodiinki
        pyssyAnim.SetTrigger("Shoot");
        Debug.Log(pysyVektori);
        soundMngr.playBang();
        gunTimer = 0;
        shootingGun = true;
        pyssyLadattu = false;
        GameObject projectile = Instantiate(bulletPrefab, firePoint.transform.position, transform.rotation);
        Rigidbody2D testi = projectile.GetComponent<Rigidbody2D>();
        testi.velocity = pysyVektori * bulletSpeed * Time.deltaTime;

    }
    public void reloadGun()
    {
        playerAnim.SetTrigger("Reload");
    }
    public void takeDamage()
    {
        //rb.AddForce(Vector2.left * knockback, ForceMode2D.Impulse);
        currentHP--;
        if (currentHP <= 0)
        {
            Die();
        }

    }
    public void Die()
    {
        alive = false;
        playerAnim.SetTrigger("Death");
        //joku skenetransitioni tähä sit
    }
    public void setFrozen(bool onko)
    {
        frozen = onko;
    }

    public Vector2 getAttackVektori()
    {
        Vector2 pysyVektori = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (pysyVektori.x == 0 && pysyVektori.y == 0)
        {
            if (XorY)
            {
                playerAnim.SetFloat("LastMoveY", 0f);
                pysyVektori.x = lastMovement.x;
            }
            else
            {
                playerAnim.SetFloat("LastMoveX", 0f);
                pysyVektori.y = lastMovement.y;
            }
        }
        return pysyVektori;

    }
    public void dodgeRoll()
    {
        playerAnim.SetTrigger("Dodge");

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<VIDE_Assign>() != null)
        {
            inTrigger = other.GetComponent<VIDE_Assign>();
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        inTrigger = null;
    }
    private void OnDrawGizmosSelected()
    {
        if (dialogueRinki == null && firePoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(dialogueRinki.position, dialogueRange);
        Gizmos.DrawWireSphere(firePoint.transform.position, 1.0f);

    }

}