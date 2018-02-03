using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C : MonoBehaviour {

    public Transform[] player;
    public Player[] playerScript;
    public static AudioManager am;
    public static UIManager ui;
    public static C c;
    public Transform enemySpawnPoint;
    public Transform customerSpawnPoint;
    public Transform enemyGoal;
    public GameObject[] prefabs;
    public static Vector2 mouseWorldPos;
    public TurretInfo[] turrentData;
    public NPCInfo[] npcData;
    public ItemArray[] itemData;
    public static BensUtil ben;
    public List<Enemy> enemyList = new List<Enemy>();
    public List<ItemTable> itemTableList = new List<ItemTable>();
    public List<NPC> npcList = new List<NPC>();
    public List<int> npcIdCanSpawnList = new List<int>();
    public float clockTimer;

    

    [Header("Debug Options")]
    public bool debugNoEnemies;
    public float debugTimeScale = 1;


	// Use this for initialization
	void Start () {
        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        ui = GameObject.Find("UIManager").GetComponent<UIManager>();
        c = GameObject.Find("C").GetComponent<C>();
        ben = gameObject.AddComponent<BensUtil>();
        var i = 0; foreach(NPCInfo info in npcData) { npcIdCanSpawnList.Add(i); i++; }
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnCustomers());
    }

    // Update is called once per frame
    void Update () {

        Time.timeScale = debugTimeScale;

        clockTimer += Time.deltaTime * 2; if (clockTimer > 60 * 24) clockTimer = 0;
        Camera.main.transform.position = player[0].position + Vector3.back;

        mouseWorldPos.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        mouseWorldPos.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

    }

    public IEnumerator SpawnEnemies() {
        yield return new WaitForSeconds(0f);
        while (true) {
            if (!debugNoEnemies) {
                var inst = Instantiate(prefabs[0], enemySpawnPoint.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(4f);
        }
    }

    public IEnumerator SpawnCustomers() {
        yield return new WaitForSeconds(5f);
        while (true) {
            if (Random.value < .2f && npcIdCanSpawnList.Count > 0) {
                var inst = Instantiate(prefabs[6], customerSpawnPoint.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(3f);
        }
    }

    public void SetDepth(Transform t, float yoffset = 0) {
        var pos = t.position;
        pos.z = (pos.y + yoffset + 100) * .001f;
        t.position = pos;
    }

    public void SpawnItem(bool gold, Vector3 pos) {
        var inst = Instantiate(C.c.prefabs[4], pos, Quaternion.identity);
        if (gold) {
            inst.GetComponent<Item>().SetItem(0, Random.Range(0, C.c.itemData[0].itemData.Length));
            inst.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
        } else {
            var typeInt = Random.Range(1, C.c.itemData.Length);
            inst.GetComponent<Item>().SetItem(typeInt, Random.Range(0, C.c.itemData[typeInt].itemData.Length));
            inst.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
        }
    }

    public void SpawnTextPopup(Vector2 pos, string str) {
        var inst = Instantiate(prefabs[7], pos, Quaternion.identity);
        inst.transform.GetChild(0).GetComponent<TextMesh>().text = str;
        inst.transform.position += Vector3.back * .5f;
    }

}

public class BensUtil : MonoBehaviour {

    public void SetColor(SpriteRenderer spr, Color color, float alpha = 1) {
        var col = spr.color;
        col = color;
        col.a = alpha;
        spr.color = col;
    }

    public void SetAlpha(SpriteRenderer spr,float alpha) {
        var col = spr.color;
        col.a = alpha;
        spr.color = col;
    }

    public float Osc(float multiplier) {
        return Mathf.Sin(Time.time * multiplier);
    }

}

