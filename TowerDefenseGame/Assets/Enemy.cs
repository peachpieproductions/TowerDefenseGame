using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<PolyNavAgent>().SetDestination(new Vector2(C.c.player.position.x, C.c.player.position.y));
    }
}
