using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [Header("Clock")]
    public Transform[] clockHands;

    [Header("Building")]
    public GameObject[] buildPanel;
    public Text[] buildName;
    public Text[] buildCost;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (buildPanel[0].activeSelf) {
            if (C.c.playerScript[0].buildMode == 1) {
                buildName[0].text = C.c.turrentData[C.c.playerScript[0].buildID].turretName;
                buildCost[0].text = "$" + C.c.turrentData[C.c.playerScript[0].buildID].cost.ToString();
            }
        }

        //clock
        clockHands[1].eulerAngles = new Vector3(0, 0, (-C.c.clockTimer % 60) * 6);
        clockHands[0].eulerAngles = new Vector3(0, 0, (-C.c.clockTimer / 60) * 30);

    }
}
