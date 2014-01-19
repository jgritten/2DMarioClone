using UnityEngine;
using System.Collections;

public class scriptFlag : MonoBehaviour {
	
	// Flag Waving

	public scriptAniSprite aniPlay;
	public int flagWaveSpeed = 5;

	void Start(){
		scriptAniSprite aniPlay = GetComponent<scriptAniSprite>();
	}

	// Update is called once per frame
	void Update () {
		aniPlay.AniSprite(1,10,0,0,21, flagWaveSpeed);
	}
}
