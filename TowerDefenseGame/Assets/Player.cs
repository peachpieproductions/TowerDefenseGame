using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    Rigidbody2D rb;
    SpriteRenderer spr;
    float attackTimer;
    public bool buildMode;
    public GameObject buildObject;
    public int buildID;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

        //depth
        var pos = transform.position;
        pos.z = (pos.y + 100) * .001f;
        transform.position = pos;

        //Build Mode
        if (Input.GetKeyDown(KeyCode.B)) {
            buildMode = !buildMode;
        }
        if (buildMode) {
            if (buildObject != null) {
                var p = buildObject.transform.position;
                p.x = Mathf.Floor(C.mouseWorldPos.x) + .5f;
                p.y = Mathf.Floor(C.mouseWorldPos.y) + .5f;
                buildObject.transform.position = p;
                if (Input.mouseScrollDelta.y != 0) {
                    buildID += (int)Input.mouseScrollDelta.y;
                    buildObject.GetComponent<Turret>().UpdateTurret(buildID);
                }

                if (Input.GetMouseButtonDown(0)) {
                    buildObject.GetComponent<Turret>().OnPlaced();
                    C.ben.SetColor(buildObject.GetComponent<Turret>().spr,Color.white, 1);
                    buildObject = null;
                }

                if (Input.GetMouseButtonDown(1)) { //FUCKED UP
                    var mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(mpos, Vector2.zero,.1f);
                    
                    if (hit.collider != null) {
                        Destroy(hit.transform.gameObject);
                    }
                }


            } else {
                buildObject = Instantiate(C.c.prefabs[3], transform.position, Quaternion.identity);
                buildObject.GetComponent<Turret>().UpdateTurret(buildID);
                C.ben.SetColor(buildObject.GetComponent<Turret>().spr,Colors.LimeGreen, .7f);
            }
        }

        if (attackTimer > 0) attackTimer -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0)) {
            if (attackTimer <= 0) {
                attackTimer = .5f;
                GetComponent<Animator>().SetTrigger("attack");
                var flip = 1; if (spr.flipX) flip = -1;
                foreach(Collider2D coll in Physics2D.OverlapBoxAll((Vector2)transform.position + Vector2.right * flip, new Vector2(2,2),0f)) {
                    if (coll.CompareTag("Enemy")) {
                        coll.GetComponent<Enemy>().EnemyHit(3f);
                        coll.GetComponent<Enemy>().fightingPlayer = true;
                        coll.GetComponent<Enemy>().agent.maxSpeed = 3.5f;
                        coll.GetComponent<Rigidbody2D>().velocity += (Vector2)(coll.transform.position - transform.position) * 4;
                    }
                }
                
            }
        }
		
        if (Input.GetKey(KeyCode.W)) {
            rb.velocity += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S)) {
            rb.velocity += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A)) {
            rb.velocity += Vector2.left;
            spr.flipX = true;

        }
        if (Input.GetKey(KeyCode.D)) {
            rb.velocity += Vector2.right;
            spr.flipX = false;
        }

        if (rb.velocity.magnitude > 10) rb.velocity = rb.velocity.normalized * 10;
        rb.velocity *= .9f;

        GetComponent<Animator>().SetInteger("direction", (int)rb.velocity.magnitude);

    }
}
