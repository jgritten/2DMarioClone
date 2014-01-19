using UnityEngine;
using System.Collections;

[AddComponentMenu("WalkerBoys/Actor/scriptPlayerProperties")]

public class scriptPlayerProperties : MonoBehaviour 
{
    // PLAYER PROPERTIES
    // Description: Set and store pickups and state of player

    

    public enum PlayerState { MarioDead, MarioSmall, MarioLarge, MarioFire }            // 0 = Player Dead, 1 = Player Initial State
                                                                                        // 2 = Player Large, 3 = Player Fireball Powerup
	public GameObject player;
    public PlayerState playerState = PlayerState.MarioSmall;

    public int lives           = 3;
    public int key             = 0;
    public int coins           = 0;
    public GameObject projectileFire;
	public GameObject spawnLocation;
	public GameObject camera;
    public Transform projectileSocketRight;
    public Transform projectileSocketLeft;
    public Material materialMarioStandard;
    public Material materialMarioFire;
    public AudioClip soundDie;
    public AudioClip fireball;
    public float soundDelay = 0;
    public float soundRate = 0;

    public bool changeMario = false;
    public bool hasFire = false;

    private int coinLife = 20;
    public bool canShoot = true;
	
	void Start()
	{
		player = GameObject.FindWithTag("player");
	}

    void Update ()
    {
        scriptPlayerControl playerControls = GetComponent<scriptPlayerControl>();
        if (changeMario)
        {
        SetPlayerState();
        }
        if (canShoot)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (playerControls.direction)
                {
                    GameObject newFireBall = ((GameObject)Instantiate(projectileFire, projectileSocketRight.transform.position, transform.rotation));
                    StartCoroutine(ParticleDestroy(newFireBall, 5));
					newFireBall.GetComponent<scriptProjectileFireball>().moveSpeed = -18;
                    PlayAudio(fireball, 0);
                }
                else
                {
                    GameObject newFireBall = ((GameObject)Instantiate(projectileFire, projectileSocketLeft.transform.position, transform.rotation));
                    StartCoroutine(ParticleDestroy(newFireBall, 5));
					newFireBall.GetComponent<scriptProjectileFireball>().moveSpeed = 18;
                    PlayAudio(fireball, 0);
                }
            }
        }
	}
    void PlayAudio(AudioClip soundName, float soundDelay)
    {
        if (!audio.isPlaying && Time.time > soundRate)
        {
            soundRate = Time.time + soundDelay;
            audio.clip = soundName;
            AudioSource.PlayClipAtPoint(soundName, transform.position);
        }
    }
    void AddKeys(int numKey)      // Fucking Keys! Original Mario never had fucking keys! WTF
    {
        key += numKey;
    }
    void AddCoin(int numCoins)
    {
        coins = coins + numCoins;
    }
    void SetPlayerState()
    {
        scriptPlayerControl playerControls = GetComponent<scriptPlayerControl>();
        CharacterController charController = GetComponent<CharacterController>();

        switch (playerState)
        {
            case PlayerState.MarioDead:
                MarioDeath();
                hasFire = false;
                changeMario = false;
                canShoot = false;
                break;
            case PlayerState.MarioSmall:
                transform.localScale = new Vector3(1.25F, 1.25F, 0);
                transform.renderer.material = materialMarioStandard;
                hasFire = false;
                canShoot = false;
                changeMario = false;
                break;
            case PlayerState.MarioLarge:
                playerControls.gravity = 0;
                transform.Translate(0, .5F, 0);
                transform.localScale = new Vector3(2, 2, 0);
                playerControls.gravity = 20;
                transform.renderer.material = materialMarioStandard;
                changeMario = false;
                canShoot = false;
                break;
            case PlayerState.MarioFire:
                hasFire = true;
                transform.renderer.material = materialMarioFire;
                changeMario = false;
                canShoot = true;
                break;
            default:
                break;
        }
    }
    IEnumerator ParticleDestroy(GameObject clone, int time)
    {
        yield return new WaitForSeconds(time);
        Destroy(clone.gameObject);
    }

	IEnumerator MarioReset(GameObject clone, int time)
	{
		scriptCameraSmoothFollow cameraControls = camera.GetComponent<scriptCameraSmoothFollow>();
		CharacterController charControls = GetComponent<CharacterController>();
		scriptPlayerControl playerControls = player.GetComponent<scriptPlayerControl>();
		yield return new WaitForSeconds(time);
		clone.transform.position = spawnLocation.transform.position;
		cameraControls.midScreen = cameraControls.midScreenStart;
		cameraControls.cameraFollowX = true;
		cameraControls.transform.position = new Vector3(cameraControls.midScreenStart, 0.6f, -10);
		playerState = PlayerState.MarioSmall;
		lives -= 1;
		playerControls.isAlive = true;
		cameraControls.audio.Play ();
	}

    public void MarioDeath()
    {
        scriptPlayerControl playerControls = player.GetComponent<scriptPlayerControl>();
        CharacterController charControls = GetComponent<CharacterController>();
        scriptSpawnSaveSetup playerSpawn = GetComponent<scriptSpawnSaveSetup>();
		scriptCameraSmoothFollow cameraControls = camera.GetComponent<scriptCameraSmoothFollow>();
		playerControls.isAlive = false;
        playerSpawn.PlaySound(soundDie, 0);
		cameraControls.audio.Stop ();
//        StartCoroutine(ParticleDestroy(gameObject, 2));			// Enable When Mario needs to be Destroyed
		cameraControls.cameraFollowX = false;
        playerControls.gravity = 0;
        charControls.Move(Vector3.forward * .0001F);
        playerControls.gravity = 20;
        playerControls.velocity = Vector3.zero;
		playerControls.velocity.x = 0;
        playerControls.velocity.y = playerControls.walkJump;
        playerControls.velocity.z = -2;
        playerControls.walkSpeed = 0;
        playerControls.runSpeed = 0;
		StartCoroutine(MarioReset(gameObject, 3));					// Disable when Mario needs to be Destroyed
    }
}
