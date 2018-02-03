using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TurretInfo {
    public Sprite sprite;
    public Sprite barrelSprite;
    public string turretName;
    public bool wall;
    public float fireRate;
    public float bulletSpeed;
    public GameObject bulletEffect;
    public int cost;
}

public class Turret : MonoBehaviour {

    public List<Transform> targets = new List<Transform>();
    float shootTimer;
    float fireRate;
    public int type;
    public bool wall;
    public bool placed;
    public SpriteRenderer spr;
    public SpriteRenderer barrelSpr;
    public Player owner;
    public float bulletSpeed;

	// Use this for initialization
	void Awake () {
        spr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        barrelSpr = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update () {

        //depth
        C.c.SetDepth(transform);

        if (!placed) {
            
            return;
        }

        if (!wall) {
            if (shootTimer > 0) shootTimer -= Time.deltaTime;
            else {
                if (targets.Count > 0) {
                    if (targets[0] != null) {
                        var aim = targets[0].position - (transform.position + new Vector3(.5f,-.5f));

                        var angle = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;
                        barrelSpr.transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);

                        var inst = Instantiate(C.c.prefabs[1], transform.position + new Vector3 (.5f,-.5f), Quaternion.identity);
                        inst.GetComponent<Rigidbody2D>().velocity = new Vector2(aim.x, aim.y).normalized * bulletSpeed;
                        inst.GetComponent<Bullet>().SetBullet(type);
                        //C.am.PlaySound(0);
                        shootTimer = fireRate;
                    } else {
                        targets.RemoveAt(0);
                    }
                }
            }
        }
	}

    public void OnPlaced(int p) {
        owner = C.c.playerScript[p];
        placed = true;
        GetComponent<PolyNavObstacle>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = true;
        C.ben.SetColor(spr, Color.white, 1);
    }

    public void UpdateTurret(int type) {
        this.type = type;
        spr.sprite = C.c.turrentData[type].sprite;
        barrelSpr.sprite = C.c.turrentData[type].barrelSprite;
        wall = C.c.turrentData[type].wall;
        bulletSpeed = C.c.turrentData[type].bulletSpeed;
        fireRate = C.c.turrentData[type].fireRate;
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
