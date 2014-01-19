using UnityEngine;
using System.Collections;

public class scriptProjectileFireball : MonoBehaviour 
{
    // FIREBALL PROJECTILE
    // Description: Control the release of a fireball projectile
    //Instruction: Assign to the player properties component for projectile
	public AudioClip enemyDestroyed;

    public float moveSpeed = 1;             // Speed which the projecile will travel
    public float bounceHeight = 0.25F;      // bounce height Limit
    public float lifeSpan = 2;              // how long will the projectile last if no collision is detected
    public float hitPosition = 0;           // store the location of projectile when a collision is detected
    public bool bounceUp = false;           // set direction of fireball
    public float heightDiff = 0;            // difference between current Position and hit position
    public bool direction = true;
	

	void Update () 
    {
        if (bounceUp)
        {
            transform.Translate(-moveSpeed * Time.deltaTime, 6F * Time.deltaTime, 0);
            heightDiff = transform.position.y - hitPosition;
            if (bounceHeight <= heightDiff)
            {
                bounceUp = false;
            }
        }
        else
        {
            transform.Translate(-moveSpeed * Time.deltaTime, -6F * Time.deltaTime, 0);
            heightDiff = transform.position.y - hitPosition;
            if (bounceHeight <= heightDiff)
            {
                bounceUp = false;
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "block")
        {
			Destroy(gameObject);
        }
        else if (other.tag == "ground")
        {
            hitPosition = transform.position.y;
            bounceUp = !bounceUp;
        }
		else if (other.tag == "enemy")
		{
			AudioSource.PlayClipAtPoint(enemyDestroyed, transform.position);
			Destroy(gameObject);
			Destroy(other.gameObject);
		}
    }

    void KillFireball(GameObject clone)
    {
        Destroy(clone.gameObject);
    }
}
