using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    Vector2 vec;
    public float speed = 1;
    private bool isBulletStart;

    void Start()
    {

    }

    void Update()
    {
        if (!isBulletStart)
        {
            Vector3 mousePositionVec3 = Input.mousePosition;
            mousePositionVec3.z = -Camera.main.transform.position.z;

            vec = Camera.main.ScreenToWorldPoint(mousePositionVec3);

            float zRotation = Mathf.Atan2(vec.y - transform.position.y, vec.x - transform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
            isBulletStart = true;
        }

        transform.Translate(Vector2.right * speed);

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground"
            || collision.gameObject.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
    }
}
