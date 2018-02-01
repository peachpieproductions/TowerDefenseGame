using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C : MonoBehaviour {

    public Transform[] player;
    public Player[] playerScript;
    public static AudioManager am;
    public static C c;
    public Transform enemySpawnPoint;
    public Transform enemyGoal;
    public GameObject[] prefabs;
    public static Vector2 mouseWorldPos;
    public TurretInfo[] turrentData;
    public ItemArray[] itemData;
    public static BensUtil ben;
    public List<Enemy> enemyList = new List<Enemy>();

	// Use this for initialization
	void Start () {
        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        c = GameObject.Find("C").GetComponent<C>();
        ben = gameObject.AddComponent<BensUtil>();
        StartCoroutine(SpawnEnemies());
	}
	
	// Update is called once per frame
	void Update () {
        Camera.main.transform.position = player[0].position + Vector3.back;

        mouseWorldPos.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        mouseWorldPos.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

    }

    public IEnumerator SpawnEnemies() {
        yield return new WaitForSeconds(0f);
        while (true) {
            var inst = Instantiate(prefabs[0], enemySpawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(2f);
        }
    }

    public void SetDepth(Transform t, float yoffset = 0) {
        var pos = t.position;
        pos.z = (pos.y + yoffset + 100) * .001f;
        t.position = pos;
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

}

