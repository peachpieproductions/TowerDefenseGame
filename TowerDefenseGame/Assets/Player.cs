using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct InvSlot {
    public int type;
    public int index;
    public int stack;
}

public class Player : MonoBehaviour {

    public Rigidbody2D rb;
    public SpriteRenderer spr;
    float attackTimer;
    public bool buildMode;
    public GameObject buildObject;
    public int buildID;
    public Transform invSlotContainer;
    public RectTransform inventoryPanel;
    public RectTransform hpMask;
    public InvSlot[] inventory;
    public InvSlotUI[] inventoryUI;
    public bool inventoryOpen;
    public int p;
    public int gold;
    public Text goldText;
    public float maxHp;
    public float hp;
    public float hit;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        hp = maxHp;

        //get UI inventory slots
        var i = 0; foreach (InvSlotUI slot in invSlotContainer.GetComponentsInChildren<InvSlotUI>()) {
            inventoryUI[i] = slot; i++;
        }
	}
	
	// Update is called once per frame
	void Update () {

        //UI
        if (Input.GetKeyDown(KeyCode.Tab)) inventoryOpen = !inventoryOpen;
        var pos = inventoryPanel.position;
        if (inventoryOpen) {
            pos.y = Mathf.Lerp(pos.y, 340, .2f);
        } else {
            pos.y = Mathf.Lerp(pos.y, -192, .2f); 
        } inventoryPanel.position = pos;
        //Hp UI
        hpMask.localPosition = new Vector3(0, (hp / maxHp) * 100);

        //hit
        if (hit > 0) {
            hit -= Time.deltaTime;
            if (hit <= 0) C.ben.SetColor(spr, Color.white);
        }

        //depth
        C.c.SetDepth(transform);

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
                    foreach (Enemy e in C.c.enemyList) e.repath = true;
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
                        var enemy = coll.GetComponent<Enemy>();
                        enemy.EnemyHit(3f);
                        enemy.fightingPlayer = true;
                        enemy.StopCoroutine(enemy.Navigate());
                        enemy.StartCoroutine(enemy.Navigate());
                        enemy.agent.maxSpeed = 3.5f;
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

    public void AddItemToInventory(int type, int index) {

        //check if gold
        if (type == 0) {
            gold += (index + 1) * 2;
            goldText.text = gold.ToString();
            return;
        }

        //check for existing stack
        for (var i = 0; i < inventory.Length; i++) { 
            if (inventory[i].stack != 0 && inventory[i].type == type && inventory[i].index == index) { //found existing stack
                inventory[i].stack++;
                inventoryUI[i].stackSizeText.text = inventory[i].stack.ToString();
                return;
            }
        }

        //else check for free slot
        for (var i = 0; i < inventory.Length; i++) {
            if (inventory[i].stack == 0) {
                inventory[i].type = type;
                inventory[i].index = index;
                inventory[i].stack++;
                inventoryUI[i].itemSprite.enabled = true;
                inventoryUI[i].stackSizeText.enabled = true;
                inventoryUI[i].itemSprite.sprite = C.c.itemData[type].itemData[index].sprite;
                inventoryUI[i].stackSizeText.text = inventory[i].stack.ToString();
                return;
            }
        }
    }

}
