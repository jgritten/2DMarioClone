using UnityEngine;
using System.Collections;

public class scriptCameraBorderFollow : MonoBehaviour 
{
    public GameObject cameraTarget;         // object to look at/follow   PLAYERFOCUS object in this case
    public GameObject player;               // player object for moving

    public float cameraHeight = 0;          // height of Camera adjustable in the inspector
    public float smoothTime = 0.2F;         // time for camera to dampen
    public float borderX = 2.0F;            // border for width
    public float borderY = 2.0F;            // border for height

    private Vector2 velocity;               // speed of camera movement   **may need to split for c# independent camera axis
    public bool moveScreenRight = false;   // move screen enabled
    public bool moveScreenLeft = false;    // move screen enabled

    void Start ()
    {
        cameraHeight = camera.transform.position.y;     // camera height start position
    }

	void Update () 
    {
        scriptPlayerControl script = player.GetComponent<scriptPlayerControl>();        // get playerControls    **Looking Left = false **Looking Right = true **
        if (moveScreenLeft)
        {
            float newPos = Mathf.SmoothDamp(camera.transform.position.x, camera.transform.position.x - borderX, ref velocity.y, smoothTime);
            camera.transform.position = new Vector3(newPos, camera.transform.position.y, camera.transform.position.z);
            if (script.direction != false)
            {
                moveScreenLeft = false;
            }
        }
        if (moveScreenRight)
        {
            float newPos = Mathf.SmoothDamp(camera.transform.position.x, camera.transform.position.x + borderX, ref velocity.x, smoothTime);
            camera.transform.position = new Vector3(newPos, camera.transform.position.y, camera.transform.position.z);
            if (script.direction != true)
            {
                moveScreenRight = false;
            }
        }
        if (script.direction)                                                                           //
        {                 //MARIO                            //CAMERA                                   //
            if (cameraTarget.transform.position.x > camera.transform.position.x + borderX)              //
            {                                                                                           //
                moveScreenRight = true;                                                                 //    FACING RIGHT
            }            //MARIO                             //CAMERA                                   //
            if (cameraTarget.transform.position.x < camera.transform.position.x)              //
            {                                                                                           //
                moveScreenRight = false;                                                                //
            }
        }
        else
        {               //MARIO                              //CAMERA                                   //
            if (cameraTarget.transform.position.x < camera.transform.position.x - borderX)              //
            {                                                                                           //
				moveScreenLeft = false;
//				moveScreenLeft = true;                                                                  //
            }           // MARIO                             //CAMERA                                   //    FACING LEFT
            if (cameraTarget.transform.position.x > camera.transform.position.x)              //
            {                                                                                           //
                moveScreenLeft = false;                                                                 //
            }
        }
        if (moveScreenRight && moveScreenLeft)
        {
            moveScreenLeft = false;
            moveScreenRight = false;
        }
        //camera.transform.position.y = cameraHeight;                 // adjust height variable through the inspector
	}
}
