using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    public List<Transform> targets = new List<Transform>();
    float shootTimer;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (shootTimer > 0) shootTimer -= Time.deltaTime;
        else {
            if (targets.Count > 0) {
                Destroy(targets[0].gameObject);
                C.am.PlaySound(0);
                shootTimer = 1f;
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy")) {
            targets.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Enemy")) {
            targets.Remove(collision.transform);
        }
    }

}
