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
            C.c.SetDepth(transform);

            if (agent.movingDirection.x < 0) spr.flipX = true;
            else spr.flipX = false;

            if (fightingPlayer) {
                if (attackTimer <= 0) {
                    if (Vector3.Distance(transform.position, C.c.player[0].position) < 1) {
                        anim.SetTrigger("Attack");
                        attackTimer = .5f;
                        C.c.playerScript[0].hp -= 2f;
                        C.c.playerScript[0].hit = .25f;
                        C.ben.SetColor(C.c.playerScript[0].spr, Color.red);
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
                goal = new Vector2(C.c.player[0].position.x, C.c.player[0].position.y);
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
            EnemyHit(1);
            rb.velocity *= .1f;
        }
    }

    public void EnemyHit(float hitPoints) {
        hp -= hitPoints;
        hit = .25f;
        spr.color = Colors.Red;
        if (hp <= 0) { //death
            int coinDrops = Random.Range(0, 3);
            for (var i = 0; i < coinDrops; i++) {
                C.c.SpawnItem(true,transform.position);
            }
            if (Random.value < .3f) {
                C.c.SpawnItem(false, transform.position);
            } 
            if (Random.value < .1f) {
                C.c.SpawnItem(false, transform.position);
            }
            C.c.enemyList.Remove(this);
            Destroy(gameObject);
        }
    }

}
