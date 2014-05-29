using UnityEngine;
using System.Collections;

public class DisableElem : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        foreach (Transform child in transform) {
            child.renderer.enabled = false;
        }
	}
}
