using UnityEngine;
using System.Collections;

public class scriptEnemyGumba : MonoBehaviour {

	// Gumba Enemy script

	public scriptAniSprite aniPlay;			// get the component for animation

	public float moveSpeed = 10;			// speed of Gumba
	public float atkMoveSpeed = 20;			// atk speed of Gumba
	public float jumpSpeed = 3;				// jump enabled for flyingGumbas
	public float atkRange = 1; 				// range which the Enemy will engage with atk
	public float atkSearchRng = 3;	       	// range which the Enemy will swtich to atkMoveSpeed
	public float deathForce = 3;			// rebound force after jumping on the Enemy
	public bool changeDirection = true;		// Enable ability to Change direction
	public bool playerDead = false;			// bool if Player is Dead or Alive
	public GameObject homePos;				// Center position from which to Partol
	public float roamingRng = 5;
	public AudioClip gumbaDeath;

	public enum GumbaState { moveLeft, moveRight, moveStop, jump, enemyDie, Home }		// enum of Enemy States
	public GumbaState gumbaState = GumbaState.moveLeft;								// initial state of enemy Gumba

	public GameObject player;				// player transform.position

	public bool gizmoToggle = true;			// toggle the display of debug radius
	bool isAlive = true;					// is Gumba Alive
	public bool isEnabled = false;			// gumba Woken u

	float fromLoadTime = 0;
	Vector3 velocity = Vector3.zero;		// store the enemy velocity
	float gravity = 20;						// weight / fall speed of enemy
	public bool isRight = false;			// direction  *** fuck walkerboys for not being consistant
	Vector3 myTransform;					// store Spawn location
	float resetMoveSpeed;					// store original move speed
	float distanceToTarget = 0;				// distance to Player
	float distanceToHome = 0;				// distance from Spawn location
	CharacterController controller;			// get controller

	void Start (){
		gumbaState = GumbaState.moveStop;
		player = GameObject.FindWithTag("player");
		scriptPlayerProperties pProp = player.GetComponent<scriptPlayerProperties>();
		myTransform = transform.position;
		controller = GetComponent<CharacterController>();
		scriptAniSprite aniPlay = GetComponent<scriptAniSprite>();
	}

	// ******  FIGURE OUT HOW TO KILL WHEN THE BLOCK UNDERNEATH IS BUMPED

	void Update () {
		homePos.transform.position = myTransform;
		scriptPlayerProperties pProp = player.GetComponent<scriptPlayerProperties>();
		BoxCollider bCollider = GetComponent<BoxCollider>();
		if (pProp.playerState == scriptPlayerProperties.PlayerState.MarioDead) {
			playerDead = true;
			gumbaState = GumbaState.moveStop;
			bCollider.enabled = false;
				}
		else {
			bCollider.enabled = true;
			playerDead = false;
				}
		distanceToTarget = Mathf.Abs (player.transform.position.x - transform.position.x);
		if (distanceToTarget <= 15 && isAlive && !playerDead) {
			isEnabled = true;
		}
		else {
			isEnabled = false;
				}
		if (isEnabled) {
			collider.enabled = true;
			if ( isRight) {
				gumbaState = GumbaState.moveRight;
			}
			else{
				gumbaState = GumbaState.moveLeft;
			}
			distanceToHome = Vector3.Distance(homePos.transform.position, transform.position);
			if (distanceToHome > roamingRng) {
				GoHome();
			}
		}

		if (controller.isGrounded) {
			velocity.y = 0;
			switch (gumbaState) {
			case GumbaState.moveLeft:
					PatrolLeft ();
				break;
			case GumbaState.moveRight:
					PatrolRight();
				break;
			case GumbaState.moveStop:
				if (isRight) {
					IdleRight();
					break;
				}
				else {
					IdleLeft();
					break;
				}
			case GumbaState.jump:
				if (isRight) {
					JumpRight();
					break;
				}
				else{
					JumpLeft ();
					break;
				}
			case GumbaState.enemyDie:
				isAlive = false;
				if (isRight) {
					DieRight();
					break;
				}
				else {
					DieLeft();
					break;
				}
			case GumbaState.Home:
				GoHome();
				break;
			default:
			break;
				}
			}
		controller.Move(velocity * Time.deltaTime);
		if (!controller.isGrounded) {
			velocity.y -= gravity * Time.deltaTime;		// Apply Gravity
				}
	}

	void OnTriggerEnter(Collider other)	{
		if (other.tag == "collisionBoxFeet") {
			isAlive = false;
			scriptPlayerControl playerCont = player.GetComponent<scriptPlayerControl>();
			playerCont.velocity.y = 5;
			fromLoadTime = Time.timeSinceLevelLoad;
			AudioSource.PlayClipAtPoint(gumbaDeath,transform.position);
			gumbaState = GumbaState.enemyDie;
				}
		if (other.tag == "block") {
				if (changeDirection) {
					StartCoroutine(DirectionChangeWait(0.2f));	
				}
			}
		if (other.tag == "enemy") {
			if (changeDirection) {
				StartCoroutine(DirectionChangeWait(0.03f));
			}
		}
	}

	// move enemy Left
	void PatrolLeft(){
		velocity.x = -moveSpeed * Time.fixedDeltaTime;		// Move controller to the Left
		aniPlay.AniSprite(16,16,0,7,16,24);
		isRight = false;
	}
	// move enemy Right
	void PatrolRight(){
		velocity.x = moveSpeed * Time.fixedDeltaTime;		// Move controller to the Right
		aniPlay.AniSprite(16,16,0,6,16,24);
		isRight = true;
	}
	// idle enemy Left
	void IdleLeft(){
		velocity.x = 0;										// Move controller to the Right
		aniPlay.AniSprite(16,16,0,2,31,24);
		isRight = false;
	}
	// idle enemy Right 
	void IdleRight(){
		velocity.x = 0;										// Move controller to the Right
		aniPlay.AniSprite(16,16,0,0,29,24);
		isRight = true;
	}
	// enemy Jump Left
	void JumpLeft(){
		velocity.y = jumpSpeed;
		aniPlay.AniSprite(16,16,7,9,1,24, fromLoadTime);
		isRight = false;
	}
	// enemy Jump Right
	void JumpRight(){
		velocity.y = jumpSpeed;
		aniPlay.AniSprite(16,16,7,8,1,24, fromLoadTime);
		isRight = true;
	}
	// enemy Die Left
	void DieLeft(){
		velocity.x = 0;
		aniPlay.AniSprite(16,16,0,11,16,46, fromLoadTime);
		StartCoroutine(DestroyEnemy(0.3f));
	}
	// enemy Die Right
	void DieRight(){
		velocity.x = 0;
		aniPlay.AniSprite(16,16,0,10,16,46, fromLoadTime);
		StartCoroutine(DestroyEnemy(0.3f));
	}
	// enemy Chase player based on atkSearchRange
	void ChasePlayer(){
		if (isRight) {
			velocity.x = atkMoveSpeed * Time.fixedDeltaTime;
				}
		else{
			velocity.x = -atkMoveSpeed * Time.fixedDeltaTime;
		}
	}
	// find home and go Home
	void GoHome(){
		StartCoroutine(DirectionChangeWait(0.3f));
	}
	// toggle the gizmos for the designer to see ranges
	void OnDrawGizmos(){
		if (gizmoToggle) {
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, atkRange);
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, atkSearchRng);
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(homePos.transform.position, roamingRng);
		}
	}	
	IEnumerator DestroyEnemy(float time){
		yield return new WaitForSeconds(time);
		Destroy(gameObject);
	}
	IEnumerator DirectionChangeWait(float time)
	{
		isRight	= !isRight;
		if (isRight) {
		gumbaState = GumbaState.moveRight;
		}
		else{
		gumbaState = GumbaState.moveLeft;
		}
		changeDirection = false;
		yield return new WaitForSeconds(time);
		changeDirection = true;
	}
}
