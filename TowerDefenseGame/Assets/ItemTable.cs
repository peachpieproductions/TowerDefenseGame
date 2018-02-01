using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTable : MonoBehaviour {

    public SpriteRenderer item1spr;
    public SpriteRenderer item2spr;
    public ParticleSystem particleStream1;
    public ParticleSystem particleStream2;
    Transform playerNearby;

    private void Start() {
        C.c.SetDepth(transform);
    }

    private void Update() {
        if (playerNearby != null) {
            if (Vector2.Distance(transform.position + Vector3.down * .5f, playerNearby.position) > 1.4f) {
                particleStream2.Play();
            } else {
                particleStream1.Play();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            playerNearby = collision.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            playerNearby = null;
        }
    }


}
