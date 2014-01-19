using UnityEngine;
using System.Collections;

public class scriptMarioCollider : MonoBehaviour {

	// MARIO COLLIDER

	public GameObject player;
	public AudioClip marioDamage;
	bool takeDamage = true;
	bool fade = false;

	public float smooth = 1f;

	void Start(){
		player = GameObject.FindWithTag("player");
	}

	void PlaySound(AudioClip clip)	{
		audio.clip = clip;
		AudioSource.PlayClipAtPoint(clip, transform.position);
	}

	void Update()
	{
		if (!takeDamage) {
			float intensityA = 0;
			float intensityB = 1;
			if (fade) {
				Color temp = player.renderer.material.color;
				temp.a = Mathf.Lerp(intensityA, intensityB, Time.deltaTime * smooth);
				player.renderer.material.color = temp;
				if (temp.a <= 0.5f) {
					fade = false;	
				}
			}
			else{
				Color temp = player.renderer.material.color;
				temp.a = Mathf.Lerp(intensityB, intensityA, Time.deltaTime * smooth);
				player.renderer.material.color = temp;
				if (temp.a >= 0.5f) {
					fade = true;	
				}
			}
		}
		else {
			Color temp = player.renderer.material.color;
			temp.a = 1;
			player.renderer.material.color = temp;
				}
	}

	void OnTriggerEnter(Collider other)	{
		scriptPlayerProperties pProp = player.GetComponent<scriptPlayerProperties>();
		scriptPlayerControl pControl = player.GetComponent<scriptPlayerControl>();
		if (other.tag == "enemy" && takeDamage) {
			if (pProp.playerState == scriptPlayerProperties.PlayerState.MarioSmall) {
				GameObject.Find("collisionBoxFeet").GetComponent<BoxCollider>().enabled = false;
				pProp.playerState = scriptPlayerProperties.PlayerState.MarioDead;
//				StartCoroutine(HitWait (2));
				pProp.changeMario = true;
			}
			if (pProp.playerState == scriptPlayerProperties.PlayerState.MarioLarge) {
				pProp.playerState = scriptPlayerProperties.PlayerState.MarioSmall;
				PlaySound(marioDamage);
				takeDamage = false;
				StartCoroutine(HitWait (2));
				pProp.changeMario = true;
			}
			if (pProp.playerState == scriptPlayerProperties.PlayerState.MarioFire) {
				pProp.playerState = scriptPlayerProperties.PlayerState.MarioSmall;
				PlaySound(marioDamage);
				takeDamage = false;
				StartCoroutine(HitWait (2));
				pProp.changeMario = true;
			}
		}
		if (other.tag == "flagBottom"){
			scriptFlagPoleCollider flagCol = GameObject.Find("Flag Pole").GetComponent<scriptFlagPoleCollider>();
			flagCol.flagBottomReached = true;
			pControl.flagPoleBotReached = true;
		}
	}

	IEnumerator HitWait(int time)
	{
		fade = true;
		yield return new WaitForSeconds(2);
		takeDamage = true;
	}
}
