using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Bullet : MonoBehaviour {

    bool destroyed;

    public void SetBullet(int index) {
        var inst = Instantiate(C.c.turrentData[index].bulletEffect, transform);
        inst.transform.localPosition = Vector3.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!destroyed) {
            DestroyBullet();
        }
    }

    public void DestroyBullet() {
        if (destroyed) return;
        destroyed = true;
        var inst = Instantiate(C.c.prefabs[2], transform.position, Quaternion.identity);
        Destroy(inst, 4f);
        Destroy(gameObject);
    }

}
