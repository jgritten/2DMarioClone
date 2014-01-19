using UnityEngine;
using System.Collections;

public class scriptPlayerControl : MonoBehaviour {

    public scriptAniSprite aniPlay;
    public bool direction           = true;     // false = Left, true = Right
    public bool jumpEnabled        	= false;
	public bool pipeWarpEnabled 	= false;	// if pipe collider is enabled for warp, this = true;
	public bool isAlive 			= true;		// is player Alive
	public bool movementEnabled 	= true;
	public bool flagReached 		= false;
	public bool flagPoleBotReached 	= false;
	public bool castleReached 		= false;
	public bool nextLvlPipeReached  = false;

    public Transform particleJump;              // hiting ground particle

	public GameObject player;					// player character


    public AudioClip soundJump;                 // basic jump sound
    public AudioClip soundCrouchJump;           // crouch jump sound
	public AudioClip pipeWarp;					// warp into a pipe

    private float soundDelay        = 0;
    private float soundRate         = 0;

    public float fromLoadTime = 0;
    public float walkSpeed          = 10;       // walking speed
    public float runSpeed           = 15;       // running speed (Holding L-Shift)
    public float fallSpeed          = 2.0F;     // speed of falling
    public float walkJump           = 10.0F;    // jump height from walk or standing
    public float runJump            = 13.0F;    // jump height from running
    public float crouchJump         = 15.0F;    // jump height from crouch
    public float gravity            = 20.0F;    // force applied on char
    public float startPos           = 0.0F;     // y location for start position

    public Vector3 velocity = Vector3.zero;    // speed of player

    void PlaySound(AudioClip soundName, float delay)
    {
        if (!audio.isPlaying && Time.time > soundRate)
        {
            soundRate = Time.time + delay;
            audio.clip = soundName;
            AudioSource.PlayClipAtPoint(soundName, transform.position);
        }
    }

	void Awake()
	{
		player = GameObject.FindWithTag("player");
	}

    void Update()
    {
		scriptFlagPoleCollider flag = GameObject.Find("Flag Pole").GetComponent<scriptFlagPoleCollider>();
		if (flag.flagReached) {
			flagReached = true;
				}
		if (!flagReached){
        Vector3 particlePlacement = new Vector3(transform.position.x, transform.position.y - .7F, transform.position.z);        // set particle at base of sprite
        CharacterController controller = GetComponent<CharacterController>();
	        if (controller.isGrounded && isAlive && movementEnabled)
        {
            startPos = transform.position.y;
            jumpEnabled = true;
            if (Input.GetKeyDown("space") || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                fromLoadTime = Time.timeSinceLevelLoad;
            }

            velocity = new Vector3(Input.GetAxis("Horizontal"), 0, 0);

            if (velocity.x == 0)                                                                        // Idle Animation as per direction facing
            {
                if (direction)
                {
                    aniPlay.AniSprite(16, 16, 0, 0, 16, 16, fromLoadTime);
                }
                else
                {
                    aniPlay.AniSprite(16, 16, 0, 1, 16, 16, fromLoadTime);
                }
            }
            if (velocity.x > 0)                                                                         // Diretion flag as per velocity
            {
                direction = true;
            }
            else if (velocity.x < 0)
            {
                direction = false;
            }

            if (Input.GetKey(KeyCode.LeftShift))                                                        // Toggle for Run Speed
            {
                velocity *= runSpeed;
                if (velocity.x != 0)
                {
                    if (direction)
                    {
                        aniPlay.AniSprite(16, 16, 0, 6, 16, 24, fromLoadTime);
                    }
                    else
                    {
                        aniPlay.AniSprite(16, 16, 0, 7, 16, 24, fromLoadTime);
                    }
                }
            }
            else
            {
                velocity *= walkSpeed;
                if (velocity.x != 0)
                {
                    if (direction)
                    {
                        aniPlay.AniSprite(16, 16, 0, 2, 10, 16, fromLoadTime);
                    }
                    else
                    {
                        aniPlay.AniSprite(16, 16, 0, 3, 10, 16, fromLoadTime);
                    }
                }
            }
            if (Input.GetKeyDown("space"))                                                              // Jump Input
            {
                GameObject newSmoke = ((Transform)Instantiate(particleJump, particlePlacement, transform.rotation)).gameObject;
                StartCoroutine(DestroyParticle(newSmoke));
                jumpEnabled = false;
                if (Input.GetAxis("Vertical") < 0)
                {
                    velocity.y = crouchJump;
                    PlaySound(soundCrouchJump, 0);
                }
                else if (Input.GetKey(KeyCode.LeftShift) && (velocity.x > walkSpeed || velocity.x < -walkSpeed))
                {
                    velocity.y = runJump;
                    PlaySound(soundJump, 0);
                }
                else
                {
                    velocity.y = walkJump;
                    PlaySound(soundJump, 0);
                }
            }
            if (Input.GetAxis("Vertical") < 0)                                                          // Crouch Input
            {
				if (pipeWarpEnabled) {
					PlaySound(pipeWarp, 0);
					animation.Play("Tube");
				}
                if (walkSpeed > 0 || runSpeed > 0)
                {
                    walkSpeed -= .5f;
                    if (walkSpeed < 0)
                    {
                        walkSpeed = 0;
                    }
                    runSpeed -= .5f;
                }
                if (direction)
                {
                    aniPlay.AniSprite(16, 16, 0, 8, 16, 24);
                }
                else
                {
                    aniPlay.AniSprite(16, 16, 0, 9, 16, 24);
                }
            }
            else
            {
                walkSpeed = 10;
                runSpeed = 15;
            }
        }
	        if (!controller.isGrounded && isAlive && movementEnabled)
        {
            jumpEnabled = false;
            walkSpeed = 10;
            runSpeed = 15;
            if (Input.GetButtonUp("Jump"))                      // Jump Control Height
            {
                velocity.y = velocity.y - fallSpeed;            // subtract current height from 1 if jump button is up
            }

            if (Input.GetKeyDown("space") || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                fromLoadTime = Time.timeSinceLevelLoad;
            }

            velocity.x = Input.GetAxis("Horizontal");
            velocity.x *= walkSpeed;

            if (!jumpEnabled)
            {
                if (direction)
                {
                    aniPlay.AniSprite(16, 16, 0, 2, 10, 16, fromLoadTime);
                }
                else
                {
                    aniPlay.AniSprite(16, 16, 0, 3, 10, 16, fromLoadTime);
                }
            }

            if (controller.collisionFlags == CollisionFlags.Above)
            {
                velocity.y -= 1;
            }

            if (Input.GetKey("space"))
            {
                jumpEnabled = true;
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    direction = false;
                    aniPlay.AniSprite(16, 16, 0, 11, 16, 12, fromLoadTime);
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    direction = true;
                    aniPlay.AniSprite(16, 16, 0, 10, 16, 12, fromLoadTime);
                }
                else if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
                {
                    if (direction)
                    {
                        aniPlay.AniSprite(16, 16, 0, 10, 16, 12, fromLoadTime);
                    }
                    else
                    {
                        aniPlay.AniSprite(16, 16, 0, 11, 16, 12, fromLoadTime);                        
                    }
                }
            }
            else if (jumpEnabled)
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    direction = false;
                    aniPlay.AniSprite(16, 16, 0, 11, 16, 12, fromLoadTime);
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    direction = true;
                    aniPlay.AniSprite(16, 16, 0, 10, 16, 12, fromLoadTime);
                }
                else
                {
                    if (direction)                                       // complete jump animation after jump is let go
                    {
                        aniPlay.AniSprite(16, 16, 0, 10, 16, 12, fromLoadTime);
                    }
                    else
                    {
                        aniPlay.AniSprite(16, 16, 0, 11, 16, 12, fromLoadTime);
                    }
                }
            }
        }
			if (!isAlive) {
			GameObject.Find("collisionBoxFeet").GetComponent<BoxCollider>().enabled = false;

			}
			else {
			GameObject.Find("collisionBoxFeet").GetComponent<BoxCollider>().enabled = true;
			}
			if (!movementEnabled) {
				velocity = Vector3.zero;
				walkSpeed = 0;
				runSpeed = 0;
				fallSpeed = 0;
				walkJump = 0;
				runJump = 0;
				crouchJump = 0;
				gravity = 0;
			}
		else {
			gravity = 20;
		}
//		if (movementEnabled && flagReached) {
//			walkSpeed = 5;
//			runSpeed = 5;
//			fallSpeed = 2;
//			walkJump = 0;
//			runJump = 0;
//			crouchJump = 0;
//			gravity = 20;
//			}

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        }
		else {
			if (flagPoleBotReached) {
//				print ("enable auto walk");
				CharacterController controller = GetComponent<CharacterController>();
				scriptPlayerProperties pProp = player.GetComponent<scriptPlayerProperties>();
				pProp.canShoot = false;
				controller.Move (Vector3.right * Time.deltaTime);
				aniPlay.AniSprite(16, 16, 0, 2, 10, 16, fromLoadTime);
				velocity.x = walkSpeed / 4;
				controller.Move (velocity * Time.deltaTime);
				velocity.y -= gravity * Time.deltaTime;
			}
			if (castleReached) {
				// Stop and Look at FireWorks!
			}
			if (nextLvlPipeReached) {
				// Slow right down. Warp Pipe noise, Load Thank you Screen and End credits
			}
		}
	}

    IEnumerator DestroyParticle(GameObject clone)
    {
        yield return new WaitForSeconds(2);
        Destroy(clone.gameObject);
    }
}
