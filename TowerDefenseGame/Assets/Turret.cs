using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TurretInfo {
    public Sprite sprite;
    public string turretName;
    public bool wall;
    public float shootTimer;
}

public class Turret : MonoBehaviour {

    public List<Transform> targets = new List<Transform>();
    float shootTimer;
    public int type;
    public bool wall;
    public bool placed;
    public SpriteRenderer spr;

	// Use this for initialization
	void Awake () {
        spr = transform.GetChild(0).GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

        //depth
        var pos = transform.position;
        pos.z = (pos.y + 100) * .001f;
        transform.position = pos;

        if (!placed) {
            
            return;
        }

        if (!wall) {
            if (shootTimer > 0) shootTimer -= Time.deltaTime;
            else {
                if (targets.Count > 0) {
                    if (targets[0] != null) {
                        var aim = targets[0].position - transform.position;
                        var inst = Instantiate(C.c.prefabs[1], transform.position, Quaternion.identity);
                        inst.GetComponent<Rigidbody2D>().velocity = new Vector2(aim.x, aim.y) * 5f;
                        C.am.PlaySound(0);
                        shootTimer = 1f;
                    }
                }
            }
        }
	}

    public void OnPlaced() {
        placed = true;
        GetComponent<PolyNavObstacle>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = true;
    }

    public void UpdateTurret(int type) {
        this.type = type;
        spr.sprite = C.c.turrentData[type].sprite;
        wall = C.c.turrentData[type].wall;
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
