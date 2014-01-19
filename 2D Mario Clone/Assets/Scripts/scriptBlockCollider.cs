using UnityEngine;
using System.Collections;

public class scriptBlockCollider : MonoBehaviour {

    public bool colliderToggle = true;

    void Start()
    {
        if (colliderToggle == true)
        {
            collider.enabled = true;
        }
    }

    void Update()
    {
        if (colliderToggle == true)
        {
            collider.enabled = true;
        }
        else
        {
            collider.enabled = false;
        }
    }
}
