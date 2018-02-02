using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NPCInfo {
    public AnimatorOverrideController animOverride;
    public string npcName;
    public ItemType[] preferredItems;
}

public class NPC : MonoBehaviour {

    public PolyNavAgent agent;
    SpriteRenderer spr;
    Animator anim;
    Rigidbody2D rb;
    internal bool repath = true;
    public Transform target;
    public int index;
    public float leaveTimer;
    public bool isLeaving;
    public int itemsBought;
    public int itemsBoughtThisFrame;

    void Start() {
        //C.c.enemyList.Add(this);
        spr = GetComponent<SpriteRenderer>();
        agent = GetComponent<PolyNavAgent>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        leaveTimer = 20f;
        StartCoroutine(SlowUpdate());
        StartCoroutine(Navigate());

        C.c.npcList.Add(this);

        index = C.c.npcIdCanSpawnList[Random.Range(0, C.c.npcIdCanSpawnList.Count)];
        C.c.npcIdCanSpawnList.Remove(index);
        
        anim.runtimeAnimatorController = C.c.npcData[index].animOverride;
    }

    private void OnDestroy() {
        C.c.npcIdCanSpawnList.Add(index);
        C.c.npcList.Remove(this);
    }

    public void AddHeldItemSprite(int type, int index) {
        if (itemsBought > 0) {
            var inst = Instantiate(transform.GetChild(0).GetChild(0), transform.GetChild(0));
            inst.transform.position += new Vector3(0, itemsBought * .25f);
            inst.GetComponent<SpriteRenderer>().sprite = C.c.itemData[type].itemData[index].sprite;
        } else {
            transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = C.c.itemData[type].itemData[index].sprite;
        }
        itemsBought++;
    }

    IEnumerator SlowUpdate() {
        while (true) {
            //depth
            C.c.SetDepth(transform);

            if (agent.movingDirection.x < 0) spr.flipX = true;
            else spr.flipX = false;

            if (spr.flipX) transform.GetChild(0).localPosition = new Vector2(-.43f, .4f);
            else transform.GetChild(0).localPosition = new Vector2(.43f, .4f);

            anim.SetFloat("Speed", agent.currentSpeed);

            //leaving
            leaveTimer -= .25f;
            if (leaveTimer <= 0) {
                if (Random.value < .25f) {
                    isLeaving = true;
                    repath = true;
                    target = C.c.customerSpawnPoint;
                } else leaveTimer = 5f;
            } if (isLeaving && Vector3.Distance(transform.position,target.position) < 1) {
                Destroy(gameObject);
            }

            if (target != null && !isLeaving) {
                if (Vector3.Distance(target.position,transform.position) < 2) { //near table
                    if (Random.value < .1f) { //attempt to buy
                        var table = target.GetComponent<ItemTable>();
                        if (table != null) {
                            itemsBoughtThisFrame = 0;
                            if (C.c.npcData[index].preferredItems.Length > 0) { //has preferred items 50%
                                foreach(ItemType t in C.c.npcData[index].preferredItems) {
                                    if (table.type1 > 0) {
                                        if (table.type1 == (int)t && Random.value < .5f || Random.value < .075f) {
                                            table.BuyItem(0,this);
                                        }
                                    }
                                    if (table.type2 > 0) {
                                        if (table.type2 == (int)t && Random.value < .5f || Random.value < .075f) {
                                            table.BuyItem(1, this);
                                        }
                                    }
                                }
                            } else { //no preferred items 30%
                                if (table.type1 > 0 && Random.value < .3f) {
                                    table.BuyItem(0, this);
                                }
                                if (table.type2 > 0 && Random.value < .3f) {
                                    table.BuyItem(1, this);
                                }
                            }
                        }
                    }
                    if (Random.value < .1f) { //go to new table
                        target = null;
                    }
                }
            }

            yield return new WaitForSeconds(.25f);
        }
    }

    public IEnumerator Navigate() {
        while (true) {

            if (C.c.itemTableList.Count != 0) {
                //Find Merchant Table
                if (target == null) {
                    repath = true;
                    target = C.c.itemTableList[Random.Range(0, C.c.itemTableList.Count)].transform;
                }

                if (repath) {
                    agent.SetDestination(target.position + new Vector3(.5f, -1f));
                    repath = false;
                }
                if (Random.value < .2f && !isLeaving) target = null;
            }
            yield return new WaitForSeconds(3f);
        }
    }

}
