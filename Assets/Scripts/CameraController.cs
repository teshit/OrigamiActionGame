using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject target;
    public Vector2 offset;
    public float startPosition;
    public float goalPosition;


    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = new Vector3(target.transform.position.x, offset.x, offset.y);

        if (transform.position.x < startPosition)
        {
            transform.position = new Vector3(startPosition, offset.x, offset.y);
        }

        if (transform.position.x >= goalPosition)
        {
            transform.position = new Vector3(goalPosition, offset.x, offset.y);
        }
    }
}
