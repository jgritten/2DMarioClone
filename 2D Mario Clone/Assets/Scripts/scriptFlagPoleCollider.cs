using UnityEngine;
using System.Collections;

public class scriptFlagPoleCollider : MonoBehaviour {
	
	// Flag Pole Collider for Ending the first Level

	public GameObject player;
	public bool flagReached = false;
	public bool flagBottomReached = false;


	void Start () {
		player = GameObject.FindWithTag("player");
	}

	void Update(){
//		CharacterController cCont = player.GetComponent<CharacterController>();
		scriptPlayerControl pControl = player.GetComponent<scriptPlayerControl>();
		if (flagReached) {
			scriptFlag flag = GameObject.Find("Flag").GetComponent<scriptFlag>();
			flag.transform.position = new Vector3(flag.transform.position.x, player.transform.position.y, flag.transform.position.z);
			pControl.movementEnabled = false;
			pControl.aniPlay.AniSprite(16,16,0,8,16,24);
			player.transform.Translate(Vector3.down * Time.deltaTime);
			if (flagBottomReached) {
				print ("Flag Collider FlagReached = false");
				flagReached = false;
			}
		}
		else {
			pControl.movementEnabled = true;
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "player") {
			flagReached = true;
		}
	}
}
