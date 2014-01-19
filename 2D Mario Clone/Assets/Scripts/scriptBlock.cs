using UnityEngine;
using System.Collections;

public class scriptBlock : MonoBehaviour {

	// Block Component
	// Description: Complete system for interactive blocks(Question(mushroom / Coin), breakable, solid, coins, 1up, (possible star release) & (possible Future Vine release)

	// block types - Coin, breakable, solid, question

	public GameObject player;
    public GameObject innerBlockCollider;

	public enum BlockType { blockBump, blockCoin, blockBreakable, blockSolid, blockQuestion }  // set of labels for blocks
	public enum PickUpType { none, pickupMushroomGrow, pickupMushroomLife, pickupFireFlower } // pickups from blocks
	public enum BreakType { breakableGeo, breakableParticles }						// breakable types

	public BlockType BlockState = new BlockType();							// block states for enum
	public BlockType BlockStateAfter = new BlockType();						// block state after use
	public PickUpType PickupState = new PickUpType();							// pickup state
	public BreakType BreakState = new BreakType();							// breakable state

	public int blockCoinAmount = 3;					// how many coins contained within a Coin Block
	public float blockScrollSpeed = 0.5f;			// texture scroll speed

	public Material materialBlock1;						// regular Brick block
	public Material materialBlock2;						// solid block
	public Material materialBlock3;						// piece block
	public Material materialBlock4;						// question block

	public Transform pickupCoin;							// prefab coin
	public Transform pickupMushroomGrow;					// prefab mushroom Grow
	public Transform pickupMushroomLife;					// prefab 1up
	public Transform pickupFireFlower;						// prefab FireFlower
	public Transform breakableGeo;							// prefab Breakabe Geo
	public Transform breakableParticles;					// prefab breakable Particle Effect

	public AudioClip soundLifeUp;							// Gain Life Audio Clip
	public AudioClip soundMushroom;							// spawn mushroom from block
	public AudioClip soundBump;							// Audio clip for head hitting block
	public AudioClip soundPickup;							// audio clip for pickup sound
    public AudioClip soundBreak;                            // audio clip for bricks breaking

	Vector3 breakablePos;							// position of breakable prefab
	Vector3 pickupPos;								// position of pickup when instantiated
	Vector3 coinPos;								// position of coun when instantiated
	bool blockAni = false;							// enable block animation
	bool coinMove = false;							// enable coin jump when called
    public bool blockUsed = false;                         // block has been expended (Items have been collected by player) 
	int blockCoinAmmountReset;						// set to = blockCounAmount and resets when it hits Zero

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "collisionBoxHead")
		{
			blockAni = true;						// enable block animation
		}
	}
	
	void PlaySound(AudioClip soundName, float soundDelay)
	{
		audio.clip = soundName;
		AudioSource.PlayClipAtPoint(soundName, transform.position);
	}
	
	void Start()
	{
		
	}
	
	void Update () 
	{	
		player = GameObject.FindWithTag("player");
		scriptPlayerProperties pProp = player.GetComponent<scriptPlayerProperties>();
		if (pProp.playerState == scriptPlayerProperties.PlayerState.MarioLarge || pProp.playerState == scriptPlayerProperties.PlayerState.MarioFire) {
			if (BlockState == BlockType.blockBump) {
				BlockState = BlockType.blockBreakable;
			}
			if (BlockState == BlockType.blockQuestion && PickupState == PickUpType.pickupMushroomGrow) {
				PickupState = PickUpType.pickupFireFlower;
			}
		}
		if (pProp.playerState == scriptPlayerProperties.PlayerState.MarioSmall) {
			if (BlockState == BlockType.blockBreakable) {
				BlockState = BlockType.blockBump;
			} 
			if (BlockState == BlockType.blockQuestion && PickupState == PickUpType.pickupFireFlower) {
				PickupState = PickUpType.pickupMushroomGrow;
			}
		}
		switch (BlockState)
		{
		case BlockType.blockBump:
			if (blockAni){
				animation.Play("BlockBounce");
				PlaySound (soundBump, 0);
				blockAni = false;
			}
			break;
		case BlockType.blockCoin:
			if (blockAni) {
				animation.Play("BlockBounce");
				Vector3 coinSpawn = transform.position;
				coinSpawn.z = 1;
				GameObject coin = ((Transform)Instantiate(pickupCoin, coinSpawn, transform.rotation)).gameObject;
				StartCoroutine(ParticleDestroy(coin, 1));
				blockAni = false;
				blockCoinAmount --;
				scriptPlayerProperties script = player.GetComponent<scriptPlayerProperties>();
				script.coins ++;
				if (script.coins % 100 == 0) {
					script.lives ++; 
					PlaySound (soundLifeUp,0);
				}
				else {
					PlaySound(soundPickup,0);
				}
				if (blockCoinAmount == 0) {
					renderer.material = materialBlock2;
					BlockState = BlockType.blockSolid;
				}
			}
			break;
		case BlockType.blockBreakable:
			if (blockAni) {
				animation.Play ("BlockBounce");
				if (BreakState == BreakType.breakableGeo) {
					GameObject brokenBrick = ((Transform)Instantiate(breakableGeo, transform.position, transform.rotation)).gameObject;    // Instantiate Breakable Geo once Created: 
                    PlaySound(soundBreak, 0);
					StartCoroutine(ParticleDestroy(gameObject, 0));
					StartCoroutine(ParticleDestroy(brokenBrick, 1));
				}
				blockAni = false;
			}
			break;
		case BlockType.blockQuestion:
			renderer.material = materialBlock4;
            if (blockUsed == false)
            {
                if (blockAni)
                {
                    animation.Play("BlockBounce");
                    PlaySound(soundBump, 0);
                    if (blockCoinAmount != 0) {
                        blockCoinAmount--;
                        if (blockCoinAmount == 0)
                        {
                            blockUsed = true;
                        }
					    animation.Play ("BlockBounce");
					    PlaySound (soundPickup, 0);
					    Vector3 coinCollect = transform.position;
						coinCollect.z = 0.2f;
					    GameObject coin = ((Transform)Instantiate(pickupCoin, coinCollect, transform.rotation)).gameObject;
					    scriptPlayerProperties script = player.GetComponent<scriptPlayerProperties>();
					    script.coins ++;
					    StartCoroutine(ParticleDestroy(coin, .3f));
				    }
				    if (PickupState == PickUpType.pickupMushroomLife) {
                        blockUsed = true;
					    animation.Play ("BlockBounce");
					    PlaySound(soundMushroom,0);
						Vector3 mushroomSpawn = transform.position;
						mushroomSpawn.y = transform.position.y + 1;
						mushroomSpawn.z = 0.1f;
					    GameObject mushroomLife = ((Transform)Instantiate(pickupMushroomLife, mushroomSpawn, transform.rotation)).gameObject;
				    }
                    if (PickupState == PickUpType.pickupMushroomGrow) {
                        blockUsed = true;
                        animation.Play("BlockBounce");
                        PlaySound(soundMushroom, 0);
						Vector3 mushroomSpawn = transform.position;
						mushroomSpawn.y = transform.position.y + 1;
						mushroomSpawn.z = 0.1f;
                        GameObject mushroomGrow = ((Transform)Instantiate(pickupMushroomGrow, mushroomSpawn, transform.rotation)).gameObject;
                    }
                    if (PickupState == PickUpType.pickupFireFlower)
                    {
						blockUsed = true;
						animation.Play("BlockBounce");
						PlaySound(soundMushroom, 0);
						Vector3 flowerSpawn = transform.position;
						flowerSpawn.y = transform.position.y + 1;
						flowerSpawn.z = 0.1f;
						GameObject fireFlower = ((Transform)Instantiate(pickupFireFlower, flowerSpawn, transform.rotation)).gameObject;
                    }
                }
			}
            else
            {
                renderer.material = materialBlock2;
            }
            blockAni = false;
			break;
		case BlockType.blockSolid:
            scriptBlockCollider innerBlock = innerBlockCollider.GetComponent<scriptBlockCollider>();
            if (PickupState != PickUpType.none && blockUsed == false) {			
                renderer.enabled = false;
                innerBlock.colliderToggle = false;
            }
			renderer.material = materialBlock2;
			if (blockAni) {
                innerBlock.colliderToggle = true;
				renderer.enabled = true;
            if (blockUsed == false) {
				    if (blockCoinAmount != 0) {
                        blockUsed = true;
					    blockCoinAmount--;
					    animation.Play ("BlockBounce");
					    PlaySound (soundPickup, 0);
					    Vector3 coinCollect = transform.position;
						coinCollect.z = 0.2f;
					    GameObject coin = ((Transform)Instantiate(pickupCoin, coinCollect, transform.rotation)).gameObject;
					    scriptPlayerProperties script = player.GetComponent<scriptPlayerProperties>();
					    script.coins ++;
					    StartCoroutine(ParticleDestroy(coin, .3f));
				    }
				    if (PickupState == PickUpType.pickupMushroomLife) {
                        blockUsed = true;
					    animation.Play ("BlockBounce");
					    PlaySound(soundMushroom,0);
						Vector3 mushroomSpawn = transform.position;
						mushroomSpawn.y = transform.position.y + 1;
						mushroomSpawn.z = 0.2f;
					    GameObject mushroomLife = ((Transform)Instantiate(pickupMushroomLife, mushroomSpawn, transform.rotation)).gameObject;
				    }
                    if (PickupState == PickUpType.pickupMushroomGrow) {
                        blockUsed = true;
                        animation.Play("BlockBounce");
                        PlaySound(soundMushroom, 0);
						Vector3 mushroomSpawn = transform.position;
						mushroomSpawn.y = transform.position.y + 1;
						mushroomSpawn.z = 0.2f;
                        GameObject mushroomLife = ((Transform)Instantiate(pickupMushroomGrow, mushroomSpawn, transform.rotation)).gameObject;
                    }
                }
			}
			blockAni = false;
			break;
		}
	}
	IEnumerator ParticleDestroy(GameObject clone, float time)
	{
		yield return new WaitForSeconds(time);
		if (clone.tag == "pickup_coin") {
			Destroy (clone.gameObject);
		}
		else {
		Destroy(clone.transform.parent.gameObject);
		}
	}
}
