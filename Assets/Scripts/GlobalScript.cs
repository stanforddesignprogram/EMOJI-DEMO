using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalScript : MonoBehaviour {

    [SerializeField] private Transform SteamCamera;
    [SerializeField] private Transform Status;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Status.LookAt(2 * Status.position - SteamCamera.position);
    }
}
