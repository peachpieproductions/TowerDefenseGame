using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTable : MonoBehaviour {

    public SpriteRenderer item1spr;
    public SpriteRenderer item2spr;
    public ParticleSystem particleStream1;
    public ParticleSystem particleStream2;
    Transform playerNearby;
    public int slotSelected;
    public int type1;
    public int type2;
    public int index1;
    public int index2;
    public bool placed;
    public Player owner;

    private void Start() {
        C.c.SetDepth(transform);
        C.c.itemTableList.Add(this);
    }

    private void OnDestroy() {
        C.c.itemTableList.Remove(this);
    }

    private void Update() {
        if (playerNearby != null) {
            if (Vector2.Distance(transform.position + Vector3.down * .5f, playerNearby.position) > 1.4f) {
                particleStream2.Play();
                slotSelected = 1;
            } else {
                particleStream1.Play();
                slotSelected = 0;
            }
        }
    }

    public void SetItem(int type, int index) {
        if (slotSelected == 0) {
            item1spr.enabled = true;
            item1spr.sprite = C.c.itemData[type].itemData[index].sprite;
            index1 = index;
            type1 = type;
        } else {
            item2spr.enabled = true;
            item2spr.sprite = C.c.itemData[type].itemData[index].sprite;
            index2 = index;
            type2 = type;
        }
    }

    public void OnPlaced(int p) {
        C.c.SetDepth(transform);
        owner = C.c.playerScript[p];
        owner.itemTables.Add(this);
        placed = true;
        particleStream1.Play();
        particleStream2.Play();
        foreach (Collider2D coll in GetComponents<Collider2D>()) {
            coll.enabled = true;
        }
    }

    public void BuyItem(int slot, NPC npc) {
        C.am.PlaySound(Random.Range(1, 3));
        npc.itemsBoughtThisFrame++;
        if (slot == 0) {
            owner.gold += C.c.itemData[type1].itemData[index1].cost;
            C.c.SpawnTextPopup(npc.transform.position + Vector3.down * .5f * npc.itemsBoughtThisFrame, "+ $" + C.c.itemData[type1].itemData[index1].cost);
            particleStream1.Play();
            npc.AddHeldItemSprite(type1,index1);
            type1 = 0;
            index1 = 0;
            item1spr.enabled = false;
        } else {
            owner.gold += C.c.itemData[type2].itemData[index2].cost;
            C.c.SpawnTextPopup(npc.transform.position + Vector3.down * .5f * npc.itemsBoughtThisFrame, "+ $" + C.c.itemData[type2].itemData[index2].cost);
            particleStream2.Play();
            npc.AddHeldItemSprite(type2,index2);
            type2 = 0;
            index2 = 0;
            item2spr.enabled = false;
        }
        owner.goldText.text = owner.gold.ToString();
    }

    public void PickupItem() {
        if (slotSelected == 0) {
            if (type1 == 0) return;
            var p = playerNearby.GetComponent<Player>();
            p.AddItemToInventory(type1, index1);
            type1 = 0;
            index1 = 0;
            item1spr.enabled = false;
        } else {
            if (type2 == 0) return;
            var p = playerNearby.GetComponent<Player>();
            p.AddItemToInventory(type2, index2);
            type2 = 0;
            index2 = 0;
            item2spr.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            playerNearby = collision.transform;
            collision.GetComponent<Player>().nearbyTable = this;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            playerNearby = null;
            if (collision.GetComponent<Player>().nearbyTable == this) collision.GetComponent<Player>().nearbyTable = null;
        }
    }


}
