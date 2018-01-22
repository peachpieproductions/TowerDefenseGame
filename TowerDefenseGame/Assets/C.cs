using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C : MonoBehaviour {

    public Transform player;
    public static AudioManager am;
    public static C c;
    public Transform enemySpawnPoint;
    public GameObject[] prefabs;

	// Use this for initialization
	void Start () {
        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        c = GameObject.Find("C").GetComponent<C>();

        StartCoroutine(SpawnEnemies());
	}
	
	// Update is called once per frame
	void Update () {
        Camera.main.transform.position = player.position + Vector3.back;
	}

    public IEnumerator SpawnEnemies() {
        while (true) {
            var inst = Instantiate(prefabs[0], enemySpawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(2f);
        }
    }
}
