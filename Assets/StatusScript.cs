using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusScript : MonoBehaviour {

    [SerializeField]
    private Transform SteamCamera;

	// Use this for initialization
	void Start () {
        foreach (Renderer child in gameObject.GetComponentsInChildren<Renderer>())
        {
            Color c = child.material.color;
            c.a = 0.0f;
            child.material.color = c;
        }

    }

    // Update is called once per frame
    void Update () {
        transform.LookAt(2 * transform.position - SteamCamera.position);
    }
}
