using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    Gold, Food, Novelty, Material, Junk, Weapon, Helm, Armor, Gloves, Boots
}

[System.Serializable]
public struct ItemInfo {

    public string itemName;
    public Sprite sprite;
    public ItemType type;
    public float imageScale;
    public bool fixedRotation;

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
                    if (dist < 1) {
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
