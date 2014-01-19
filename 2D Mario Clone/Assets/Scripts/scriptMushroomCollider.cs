using UnityEngine;
using System.Collections;

public class scriptMushroomCollider : MonoBehaviour {

    // Mushroom Collider Direction
    // Description: Check for collision - everytime there is a collision with an object tag name "block", it will send it in opposite direction
    // Instruction: Assign to collison game object attached to pickup item

    public bool mushroomDirection = true;       // enable mushroom direction


	void OnTriggerEnter (Collider other)
    {
        if (other.tag == "block" || other.tag == "pickup_grow" || other.tag == "pickup_life" || other.tag == "block_breakable")
        {
            mushroomDirection = !mushroomDirection;     // change direction
        }
//		if (other.tag == "back_collider") {
//			print ("ignore back collider");
//			Physics.IgnoreCollision(other.collider, collider);
//		}
	}
}
