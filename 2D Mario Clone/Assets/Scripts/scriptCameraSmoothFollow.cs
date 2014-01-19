using UnityEngine;
using System.Collections;

public class scriptCameraSmoothFollow : MonoBehaviour 
{
    // Smooth Camera

    public GameObject cameraTarget;             // object to look at or follow
    public GameObject player;                   // player object for moving

    public float smoothTime = 0.1F;             // time for camera dampen

    public bool cameraFollowX = true;           // camera follows on horizontal                                 
    public bool cameraFollowY = false;           // camera follows on vertical
    public bool cameraFollowHeight = true;     // camera will follow camera Target object height, not the Y
    public bool cameraZoom = false;             // toggle for zoom in an out
    public float cameraZoomMax = 2.0F;            // Zoom max
    public float cameraZoomMin = 4.0F;          // Zoom Min
    public float cameraZoomTime = .03F;          // speed which the camera will zoom
    public float cameraHeight = 1;              // heigth of camera adjustable in the inspector
    public float velocityX;                      // speed of camera
    public float velocityY;
	public float midScreen = 0;
	public float midScreenStart = 0;

    Transform thisTransform;                    // camera transform
    float curPos = 0;                           // current position of cameraTarget
    float playerJumpHeight = 0;                 // store jump height of player
    float playerStartHeight = 0;


    void Start()
    {
		midScreen = camera.transform.position.x;
		midScreenStart = midScreen;
        thisTransform = transform;
        playerStartHeight = player.transform.position.y - 0.2F;
    }

	void Update () 
    {
		if (player.transform.position.x > midScreen) {
			midScreen = player.transform.position.x;
			cameraFollowX = true;
				}
		else {
			cameraFollowX = false;
				}
        if (cameraFollowX)
        {
            float newPos = Mathf.SmoothDamp(transform.position.x, cameraTarget.transform.position.x, ref velocityX, smoothTime);
            thisTransform.position = new Vector3 (newPos, transform.position.y, transform.position.z);
        }
        if (cameraFollowY)
        {
            float newPos = Mathf.SmoothDamp(transform.position.y, cameraTarget.transform.position.y, ref velocityY, smoothTime);
            thisTransform.position = new Vector3(transform.position.x, newPos, transform.position.z);

        }
        if (!cameraFollowY && cameraFollowHeight)
        {
            thisTransform.position = new Vector3(thisTransform.position.x, cameraHeight, thisTransform.position.z);
        }
        scriptPlayerControl playerControl = player.GetComponent<scriptPlayerControl>();
        if (playerControl.transform.position.y < playerStartHeight)
        {
            cameraZoom = false;
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, cameraZoomMin, Time.time * cameraZoomTime);
        }
        if (playerControl.transform.position.y >= playerStartHeight)
        {
			cameraZoom = false;
//            cameraZoom = true;
        }
        if (cameraZoom)
        {
            curPos = player.transform.position.y;
            playerJumpHeight = curPos - playerControl.startPos;
            if (playerJumpHeight > cameraZoomMax)
            {
                playerJumpHeight = cameraZoomMax;
            }
            if (playerJumpHeight < 0)
            {
                camera.orthographicSize = cameraZoomMin;
                //playerJumpHeight *= -1;
            }
            else
            {
                camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, (playerJumpHeight + cameraZoomMin), Time.time * cameraZoomTime);
            }
            // **** Get hud scale to increase respectively with the Zoom out ****
        }
	}
}
