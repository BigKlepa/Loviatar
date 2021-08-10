using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WAudio : MonoBehaviour
{

    
	float moveSpeed = 3.0f;
	public AudioSource[] audioTaulukko;
	public AudioSource step;
	private AudioSource bang;
	private AudioSource woosh;
	bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
		audioTaulukko = GetComponents<AudioSource>();
		step = audioTaulukko[0];
		woosh = audioTaulukko[1];
		bang = audioTaulukko[2];

    }

    // Update is called once per frame
    void Update()
    {
		playWalk();
    }


	//ukkelin kävelyäänet @Aleksi Sorsa
	void playWalk()
    {
        if (Input.GetAxis("Vertical") >= 0.01f || Input.GetAxis("Horizontal") >= 0.01f || Input.GetAxis("Vertical") <=-0.01f || Input.GetAxis("Horizontal") <=-0.01f)
		{
			isMoving = true;
		}
		else if (Input.GetAxis ("Vertical") == 0 || Input.GetAxis ("Horizontal") == 0)
		{
			isMoving = false;
		}

		//if (rb.velocity.x != 0)
		//	isMoving = true;

		//else
		//	isMoving = false;

		if (isMoving) {
			if (!step.isPlaying)
				step.Play ();
		}
		else
			step.Stop ();

    }
    public void playSound(int luku)
    {
		audioTaulukko[luku].Play();
    }
	public void playBang()
    {
		bang.Play();
    }
	public void playSwing()
    {
		woosh.Play();
    }
  
}
