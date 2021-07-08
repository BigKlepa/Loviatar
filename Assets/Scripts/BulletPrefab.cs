using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPrefab : MonoBehaviour
{
    // Start is called before the first frame update
    public float bulletSpeed = 20.0f;
    public CircleCollider2D collideri;
    public Vector2 suuntaVektori;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("removeBullet", 0.7f);
    }

    
    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Enemy")
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    void removeBullet()
    {
        Destroy(gameObject);
    }
}
