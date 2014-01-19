using UnityEngine;
using System.Collections;

public class scriptSpawnSaveSetup : MonoBehaviour 
{
    // Spawn Save Player Location Setup
    // Description: Saves Player Position / Location (Save Points) for Spawning after Death
    // Instructions:    Place save points (gameObjects with collision) in the scene with tag names 'savePoint'
    //                  Place Killboxes in the scene with tag name 'killbox' - currently sends player to most recent savepoint Location

    public Transform mainCamera;            // toggle camera controls
    public Transform startPoint;            // where player begins after loading into the level
    public AudioClip soundDie;              // Audio clip for death

    private float soundRate = 0;            // holds current time + delta time
    private Vector3 curSavePos;
    private float soundDelay = 0;           // amount of time delayed when sound is played


    void Start()
    {
        if (startPoint != null)
        {
            transform.position = startPoint.position;
        }
    }

    public void PlaySound(AudioClip clip, float delay)
    {
        soundRate = Time.time + delay;
        audio.clip = soundDie;
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "savepoint")
        {
            curSavePos = transform.position;
        }
        if (other.tag == "killbox")
        {
            //script.MarioDeath();                  // re-enabed to actually die when you fall **Final Build**
            PlaySound(soundDie, 0);
			scriptPlayerProperties pProp = GetComponent<scriptPlayerProperties>();
			pProp.MarioDeath();
//            StartCoroutine(DeathPause());
        }
    }

	void Update () 
    {
	    
	}

    IEnumerator DeathPause()
    {
        scriptCameraSmoothFollow script = mainCamera.GetComponent<scriptCameraSmoothFollow>();
        scriptPlayerControl control = GetComponent<scriptPlayerControl>();
        scriptPlayerProperties pProp = GetComponent<scriptPlayerProperties>();

        script.cameraFollowX = false;
        script.cameraFollowY = false;
        script.cameraFollowHeight = false;
        script.cameraZoom = false;
        print("Camera toggle Off");
        yield return new WaitForSeconds(3);
        pProp.lives -= 1;
        transform.position = curSavePos;
        script.cameraFollowX = true;
        control.walkSpeed = 0;
        control.runSpeed = 0;
        yield return new WaitForSeconds(0.5F);
        control.walkSpeed = 10;
        control.runSpeed = 15;
        control.direction = true;
        script.cameraFollowX = false;
        script.cameraZoom = true;
        print("Camera toggle On");
    }
}
