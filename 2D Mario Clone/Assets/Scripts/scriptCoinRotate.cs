using UnityEngine;
using System.Collections;

public class scriptCoinRotate : MonoBehaviour {

    public int coinRotateSpeed = 20;

	void Update () 
    {
        scriptAniSprite aniPlay = GetComponent<scriptAniSprite>();
        aniPlay.AniSprite(16, 2, 0, 0, 21, coinRotateSpeed);
	}
}
