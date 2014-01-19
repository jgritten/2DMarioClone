using UnityEngine;
using System.Collections;

public class scriptItemPickup : MonoBehaviour 
{
    // *** ITEM PICKUP ***

    [AddComponentMenu("WalkerBoys/Interactive/Pickup Script")]          // assign this script to menu

    public enum PickupType { Grow, ExtraLife, Flower, Coin }            // List of Pickup Items: Mushroom(Grow), GreenMushroom(ExtraLife), FireFlower(Flower), Coin(Coin)

    public PickupType pickupType        = PickupType.Grow;              // starting value for pickup type
    public int pickupValue              = 0;                            // value for each pickup type selected
    public Transform itemParticle;                                      // particle displayed after pickup
    public AudioClip soundItemPickup;                                   // sound played on Pickup
    public AudioClip soundLifeUp;
    public float soundDelay             = 0;                            // sound delay
    public float soundRate              = 0;                            // sound play speed

    GameObject playerGameObject;                                        // gameObject with "player" Tag
    GameObject hudGameObject;                                           // get gameObject with "hud" Tag
    bool extraLifeEnabled               = false;                        // toggle for extra life

    void Start()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("player");
        hudGameObject = GameObject.FindGameObjectWithTag("hud");
    }
	
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "collisionBoxBody")
        {
            scriptPlayerProperties pProp = playerGameObject.GetComponent<scriptPlayerProperties>();
            ApplyPickup(pProp);
            renderer.enabled = false;
			if (gameObject.tag == "pickup_life" || gameObject.tag == "pickup_grow" || gameObject.tag == "pickup_flower") {
				Destroy (transform.parent.gameObject);
			}
			else{
			Destroy (gameObject);
			}
        }
    }

	void ApplyPickup (scriptPlayerProperties playerStatus) 
    {
        scriptHudController hudConnect = hudGameObject.GetComponent<scriptHudController>();

        switch (pickupType)
        {
            case PickupType.Grow:
                if (playerStatus.playerState == scriptPlayerProperties.PlayerState.MarioSmall)
                {
                    playerStatus.playerState = scriptPlayerProperties.PlayerState.MarioLarge;
                    playerStatus.changeMario = true;
                }
                PlaySound(soundItemPickup, 0);
                break;
            case PickupType.ExtraLife:
                playerStatus.lives += 1;
                PlaySound(soundItemPickup, 0);
                break;
            case PickupType.Flower:
                if (playerStatus.playerState == scriptPlayerProperties.PlayerState.MarioLarge)
                {
                    playerStatus.playerState = scriptPlayerProperties.PlayerState.MarioFire;
                    playerStatus.hasFire = true;
                    playerStatus.changeMario = true;
                }
                else if (playerStatus.playerState == scriptPlayerProperties.PlayerState.MarioSmall)
                {
                    playerStatus.playerState = scriptPlayerProperties.PlayerState.MarioLarge;
                    playerStatus.changeMario = true;
                }
                else{}
                PlaySound(soundItemPickup, 0);
                break;
            case PickupType.Coin:
                playerStatus.coins += 1;
                if (playerStatus.coins > 0 && (playerStatus.coins % 100) == 0)
                {
                    PlaySound(soundLifeUp, 0);
                    playerStatus.lives += 1;
                }
                else
                {
                    PlaySound(soundItemPickup, 0);
                }
                break;
            default:
                break;
        }
	}

    void PlaySound(AudioClip soundName, float soundDelay)
    {
        if (!audio.isPlaying && Time.time > soundRate)
        {
            soundRate = Time.time + soundDelay;
            audio.clip = soundName;
            AudioSource.PlayClipAtPoint(soundName, transform.position);
        }
    }
}
