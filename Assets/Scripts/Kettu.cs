using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kettu : MonoBehaviour
{
    public float speed;
    //private float waitTime;
    //public float startWaitTime;

    public Transform moveSpots;
    public Transform uusPos;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;



    // Start is called before the first frame update
    void Start()
    {
        //waitTime = startWaitTime;
        moveSpots.position = new Vector3(17.11f, 2.37f, -44.94f);
        uusPos.position = new Vector3(20f, 2.37f, -44.94f);
        //moveSpots.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveSpots.position, speed * Time.deltaTime);


        /*transform.position = Vector2.MoveTowards(transform.position, moveSpots.position, speed * Time.deltaTime);

        if(Vector2.Distance(transform.position, moveSpots.position) < 0.2f)
        {
            if(waitTime <= 0)
            {
                moveSpots.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }*/
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            transform.position = Vector2.MoveTowards(transform.position, uusPos.position, speed * Time.deltaTime);
        }
        Debug.Log("Gay");
    }


}
