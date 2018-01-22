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

	// Use this for initialization
	void Start () {
        spr = GetComponent<SpriteRenderer>();
        agent = GetComponent<PolyNavAgent>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        //depth
        var pos = transform.position;
        pos.z = (pos.y + 100) * .001f;
        transform.position = pos;

        var goal = new Vector2(C.c.enemyGoal.position.x, C.c.enemyGoal.position.y);
        if (fightingPlayer) goal = new Vector2(C.c.player.position.x, C.c.player.position.y);
        agent.SetDestination(goal);

        rb.velocity *= .95f;

        if (agent.movingDirection.x < 0) spr.flipX = true;
        else spr.flipX = false;

        if (fightingPlayer) {
            if (attackTimer <= 0) {
                if (Vector3.Distance(transform.position, C.c.player.position) < 1) {
                    anim.SetTrigger("Attack");
                }
            } else attackTimer -= Time.deltaTime;
        }

        if (hit > 0) { hit -= Time.deltaTime; if (hit < 0) { spr.color = Color.white; } }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.CompareTag("Bullet")) {
            collision.transform.GetComponent<Bullet>().DestroyBullet();
            EnemyHit(4);
        }
    }

    public void EnemyHit(float hitPoints) {
        hp -= 4;
        hit = .5f;
        spr.color = Color.red;
        if (hp <= 0) {
            Destroy(gameObject);
        }
    }

}
