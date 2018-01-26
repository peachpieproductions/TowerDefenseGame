using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    float hp = 10;
    public bool fightingPlayer;
    float hit;
    SpriteRenderer spr;
    public PolyNavAgent agent;
    Animator anim;
    float attackTimer;
    Rigidbody2D rb;
    internal bool repath = true;

	// Use this for initialization
	void Start () {
        C.c.enemyList.Add(this);
        spr = GetComponent<SpriteRenderer>();
        agent = GetComponent<PolyNavAgent>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(SlowUpdate());
        StartCoroutine(Navigate());
    }

    private void Update() {
        if (hit > 0) { hit -= Time.deltaTime; if (hit <= 0) { spr.color = Color.white; } }
    }

    IEnumerator SlowUpdate() {
        while (true) {
            //depth
            var pos = transform.position;
            pos.z = (pos.y + 100) * .001f;
            transform.position = pos;

            //rb.velocity *= .95f;

            if (agent.movingDirection.x < 0) spr.flipX = true;
            else spr.flipX = false;

            if (fightingPlayer) {
                if (attackTimer <= 0) {
                    if (Vector3.Distance(transform.position, C.c.player.position) < 1) {
                        anim.SetTrigger("Attack");
                    }
                } else attackTimer -= .25f;
            }

            yield return new WaitForSeconds(.25f);
        }
    }

    public IEnumerator Navigate() {
        while (true) {
            var goal = new Vector2(C.c.enemyGoal.position.x, C.c.enemyGoal.position.y);
            if (fightingPlayer) {
                goal = new Vector2(C.c.player.position.x, C.c.player.position.y);
                repath = true;
            }
            if (repath) {
                agent.SetDestination(goal);
                repath = false;
            }
            float waitTime = 3f; if (fightingPlayer) waitTime = .2f;
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.CompareTag("Bullet")) {
            collision.transform.GetComponent<Bullet>().DestroyBullet();
            EnemyHit(4);
            rb.velocity *= .1f;
        }
    }

    public void EnemyHit(float hitPoints) {
        hp -= 4;
        hit = .25f;
        spr.color = Colors.RedDevil;
        if (hp <= 0) { //death
            for(var i = 0; i < Random.Range(0,4); i++) {
                var inst = Instantiate(C.c.prefabs[4], transform.position, Quaternion.identity);
                var typeInt = Random.Range(0, C.c.itemData.Length);
                inst.GetComponent<Item>().SetItem(typeInt, Random.Range(0, C.c.itemData[typeInt].itemData.Length));
                inst.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
            }
            C.c.enemyList.Remove(this);
            Destroy(gameObject);
        }
    }

}
