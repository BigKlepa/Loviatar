using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimaatioStart : MonoBehaviour
{
    public Animator panimator;
    // Start is called before the first frame update
    void Start()
    {
        panimator.Play(0, -1, Random.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
