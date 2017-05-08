using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiScript : MonoBehaviour {

    [SerializeField]
    private Transform SteamCamera;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(2 * transform.position - SteamCamera.position);
    }
}
