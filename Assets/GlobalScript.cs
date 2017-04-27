using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalScript : MonoBehaviour {

    //public GameObject emojis;
    public GameObject SteamCamera;
    public GameObject selectedObject;

    private string message = "";

	// Use this for initialization
	void Start () {
        message = "Started";
        if (!SteamCamera) throw new UnityException("No camera attached to GlobalController!");

        //if (emojis)
        //{
        //    for (int childIndex = 0; childIndex < emojis.transform.childCount; childIndex++)
        //    {
        //        GameObject emoji = emojis.transform.GetChild(childIndex).gameObject;
        //        Color c = emoji.GetComponent<Renderer>().material.color;
        //        c.a = 0.9f;
        //        emoji.GetComponent<Renderer>().material.SetColor("_Color", c);
        //    }
        //}
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Ray rayDirection = new Ray(SteamCamera.transform.position, SteamCamera.transform.TransformDirection(Vector3.forward));

        Debug.DrawRay(SteamCamera.transform.position, SteamCamera.transform.TransformDirection(Vector3.forward), Color.green);

        if (Physics.Raycast(rayDirection, out hit))
        {
            GameObject hitObject = hit.collider.transform.gameObject;
            message = hitObject.name;
            if (hitObject.GetComponent<Collider>().tag == "Emoji")
            {
                message += " EMOJI!";
                if (selectedObject != hitObject)
                {
                    selectedObject = hitObject;
                }
            }
        }
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 6 / 100;
        style.normal.textColor = new Color(0, 0, 0, 1.0f);
        string text = message;
        GUI.Label(rect, text, style);
    }
}
