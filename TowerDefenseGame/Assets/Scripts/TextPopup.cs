using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPopup : MonoBehaviour {

    private void Start() {
        Destroy(gameObject, 2f);
    }

    private void Update() {
        transform.position += new Vector3(0, .003f, 0);
    }

}
