using UnityEngine;
using System.Collections;

public class scriptTubePoints : MonoBehaviour {

    // Tube Portal Script
    // Description: Provide porting from one tube to another
    // Instruction: Inspector needs an audio file for the tube and a transform for the location

    public float timeToPort = 2f;       // time to port (Wait in Seconds) after triggering the port event
    public Transform tubePortalTo;      // position location for the tube porting
    public AudioClip soundTube;         // audio file used for porting
	public float smooth = 3f;
	GameObject player;
	GameObject camera;

    bool moveDown = false;              // toggle for player move down tube
    bool moveUp = false;                // toggle for player move up tube

	void Start()
	{
		player = GameObject.FindWithTag("player");
		camera = GameObject.FindWithTag("MainCamera");
	}

	void Update()
	{
		if (moveDown) {
					Vector3 oldPos = player.transform.position;
					Vector3 newPos = new Vector3(player.transform.position.x, (player.transform.position.y - 5), player.transform.position.z);
					player.transform.position = Vector3.Lerp (oldPos, newPos, Time.deltaTime * smooth); 
					if (player.transform.position.y < -5) {
				moveDown = false;
				player.transform.position = tubePortalTo.transform.position;
				camera.transform.position = new Vector3(player.transform.position.x, camera.transform.position.y, camera.transform.position.z);
				StartCoroutine(TubeWait (1));
					}
		}
		if (moveUp) {
			Vector3 oldPos = player.transform.position;
			Vector3 newPos = new Vector3(player.transform.position.x, (player.transform.position.y + 5), player.transform.position.z);
			player.transform.position = Vector3.Lerp (oldPos, newPos, Time.deltaTime * smooth); 
			if (player.transform.position.y > -.4f) {
				player.GetComponent<scriptPlayerControl>().enabled = true;
				moveUp = false;
			}
		}
	}

	IEnumerator TubeWait(int time)
	{
		yield return new WaitForSeconds(time);
		AudioSource.PlayClipAtPoint(soundTube, player.transform.position);
		moveUp = true;
	}

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "player")
        {
            if (Input.GetKeyDown("down"))													// If Controls are pushed down...
            {
				float velX = other.GetComponent<scriptPlayerControl>().velocity.x;			// Get Player Velocity
				bool moveDir = other.GetComponent<scriptPlayerControl>().direction;			// detect direction for tube animation
				if (moveDir) {			// crouch Left
					velX = 0;																// Zero out Player Velocity
					scriptAniSprite aniSprite = other.GetComponent<scriptAniSprite>();		// Get Animation script
					aniSprite.AniSprite(16,16,0,8,16,24);									// Exeute animation frames for crouching
				}
				else {					// crouch Right
					velX = 0;																// Zero out Player Velocity
					scriptAniSprite aniSprite = other.GetComponent<scriptAniSprite>();		// Get Animation script
					aniSprite.AniSprite(16,16,0,9,16,24);									// get animation frame for crouching
				}
				other.GetComponent<scriptPlayerControl>().enabled = false;					// Disabled player control while animating / teleporting
				moveDown = true;															// Execute MoveDown
				AudioSource.PlayClipAtPoint(soundTube, player.transform.position);			// Play Audio warp sound
            }
			if (Input.GetKeyDown ("up")) {
				// Future Code for going UP Tubes.   Include && tube.tag("up") and vise versa for down tubes
			}
        }
    }
}
