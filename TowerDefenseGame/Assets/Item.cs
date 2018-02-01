using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    Gold, Food, Novelty, Material, Consumables, Junk, Weapon, Shield, Helm, Armor, Gloves, Boots
}

[System.Serializable]
public struct ItemInfo {

    public string itemName;
    public Sprite sprite;
    public ItemType type;
    internal int index;
    public int rarity;
    public int cost;
    public float imageScale;
    public bool fixedRotation;
    public AudioClip[] ItemPickupSounds;

}

[System.Serializable]
public class ItemArray { //seperates iteminfos into types
    public string arrayName;
    public ItemInfo[] itemData;
}

public class Item : MonoBehaviour {

    internal SpriteRenderer spr;
    internal Rigidbody2D rb;
    internal float pickupDelay = 1f;
    internal ItemInfo info;

	// Use this for initialization
	void Awake () {
        spr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(PickupDelay());
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            StartCoroutine(AttractTo(collision.transform));
        }
    }

    public void SetItem(int type, int index) {
        var data = C.c.itemData[type].itemData[index];
        if (data.fixedRotation) rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        spr.sprite = data.sprite;
        info.index = index;
        info.type = (ItemType)type;
        transform.GetChild(0).localScale = Vector3.one * data.imageScale;
    }

    public IEnumerator PickupDelay() {
        while (true) {
            pickupDelay -= Time.deltaTime;
            if (pickupDelay <= 0) yield break;
            yield return null;
        }
    }

    public IEnumerator AttractTo(Transform t) {
        while (true) {
            if (pickupDelay <= 0) {
                var dist = Vector3.Distance(transform.position, t.position);
                if (dist < 8) {
                    if (dist < 1) { //picked up
                        var p = t.GetComponent<Player>();
                        if (p != null) {
                            p.AddItemToInventory((int)info.type, info.index);
                        }
                        var sounds = C.c.itemData[(int)info.type].itemData[info.index].ItemPickupSounds;
                        C.am.PlaySound(0, sounds[Random.Range(0, sounds.Length)]);
                        Destroy(gameObject);
                        yield break;
                    }
                    rb.velocity += (Vector2)(t.position - transform.position).normalized * .5f;
                } else yield break;
            }
            yield return null;

        }
    }
}
