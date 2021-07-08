using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnsaScript : MonoBehaviour
{
    public Animator ansaAnim;
    // Start is called before the first frame update
    void Start()
    {
        ansaAnim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            ansaAnim.SetTrigger("Snap");
            Debug.Log("Lehma osu");
            Invoke("removeTrap", 0.5f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            ansaAnim.SetTrigger("Snap");
            Debug.Log("muu muu");
            Invoke("removeTrap", 0.20f);
        }
    }
    void removeTrap()
    {
        Destroy(gameObject);
    }
}
