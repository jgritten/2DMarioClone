using UnityEngine;
using System.Collections;

public class scriptMushroom : MonoBehaviour {

    // Controls Mushroom Movement and Bounce Direction

//    public GameObject mushroomDirection;
    public int velocity;
	public bool moving = false;

	void Awake()
	{
		StartCoroutine(SproutBeforeMove(.5f));
	}

	void Update () 
    {
		if (moving){
	        scriptMushroomCollider script = GetComponent<scriptMushroomCollider>();
	        bool moveDirection;
	        moveDirection = script.mushroomDirection;
	        if (moveDirection)
	        {
	            velocity = 3;       // set speed to move right
	        }
	        else
	        {
	            velocity = -3;      // set speed to move left
	        }

	        transform.Translate(velocity * Time.deltaTime, 0, 0);
		}
	}

	IEnumerator SproutBeforeMove(float wait)
	{
		yield return new WaitForSeconds(wait);
		moving = true;
	}
}
