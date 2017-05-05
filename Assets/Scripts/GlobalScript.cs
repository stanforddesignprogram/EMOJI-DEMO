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
        //Status.eulerAngles = new Vector3(STATUS_DEFAULT_ROTATION[0], SteamCamera.eulerAngles[1], STATUS_DEFAULT_ROTATION[2]);
        Status.LookAt(2 * Status.position - SteamCamera.position);
    }
}
