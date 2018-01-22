using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
        if (Input.GetKey(KeyCode.W)) {
            rb.velocity += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S)) {
            rb.velocity += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A)) {
            rb.velocity += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D)) {
            rb.velocity += Vector2.right;
        }

        if (rb.velocity.magnitude > 10) rb.velocity = rb.velocity.normalized * 10;
        rb.velocity *= .9f;

        GetComponent<Animator>().SetFloat("direction", rb.velocity.magnitude);

    }
}
