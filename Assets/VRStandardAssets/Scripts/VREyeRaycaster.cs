using System;
using System.Collections;
using UnityEngine;

namespace VRStandardAssets.Utils
{
    // In order to interact with objects in the scene
    // this class casts a ray into the scene and if it finds
    // a VRInteractiveItem it exposes it for other classes to use.
    // This script should be generally be placed on the camera.
    public class VREyeRaycaster : MonoBehaviour
    {
        public event Action<RaycastHit> OnRaycasthit;                   // This event is called every frame that the user's gaze is over a collider.


        [SerializeField] private Transform m_Camera;
        [SerializeField] private LayerMask m_ExclusionLayers;           // Layers to exclude from the raycast.
        [SerializeField] private Reticle m_Reticle;                     // The reticle, if applicable.
        [SerializeField] private VRInput m_VrInput;                     // Used to call input based events on the current VRInteractiveItem.
        [SerializeField] private bool m_ShowDebugRay;                   // Optionally show the debug ray.
        [SerializeField] private float m_DebugRayLength = 5f;           // Debug ray length.
        [SerializeField] private float m_DebugRayDuration = 1f;         // How long the Debug ray will remain visible.
        [SerializeField] private float m_RayLength = 500f;              // How far into the scene the ray is cast.

        
        private VRInteractiveItem m_CurrentInteractible;                //The current interactive item
        private VRInteractiveItem m_LastInteractible;                   //The last interactive item

        private GameObject selectedObject;
        private string message = "";
        private Color startColor;
        private Vector3 INITIAL_SCALE_VECTOR = new Vector3(0.5f, 0.5f, 0.5f);

        // Use this for initialization
        void Start()
        {
            message = "Started";
        }

        void StopPulse()
        {
            iTween.Stop();
        }

        void Pulse(GameObject gameObject)
        {
            Hashtable hash = new Hashtable();
            hash.Add("amount", new Vector3(0.25f, 0.25f, 0.25f));
            hash.Add("time", 1.0f);
            iTween.PunchScale(gameObject, hash);
        }

        // Utility for other classes to get the current interactive item
        public VRInteractiveItem CurrentInteractible
        {
            get { return m_CurrentInteractible; }
        }

        
        private void OnEnable()
        {
            //m_VrInput.OnClick += HandleClick;
            //m_VrInput.OnDoubleClick += HandleDoubleClick;
            //m_VrInput.OnUp += HandleUp;
            //m_VrInput.OnDown += HandleDown;
        }


        private void OnDisable ()
        {
            //m_VrInput.OnClick -= HandleClick;
            //m_VrInput.OnDoubleClick -= HandleDoubleClick;
            //m_VrInput.OnUp -= HandleUp;
            //m_VrInput.OnDown -= HandleDown;
        }


        private void Update()
        {
            EyeRaycast();
        }

      
        private void EyeRaycast()
        {
            // Show the debug ray if required
            if (m_ShowDebugRay)
            {
                Debug.DrawRay(m_Camera.position, m_Camera.forward * m_DebugRayLength, Color.blue, m_DebugRayDuration);
            }

            // Create a ray that points forwards from the camera.
            Ray ray = new Ray(m_Camera.position, m_Camera.forward);
            RaycastHit hit;
            
            // Do the raycast forweards to see if we hit an interactive item
            if (Physics.Raycast(ray, out hit, m_RayLength, ~m_ExclusionLayers))
            {
                VRInteractiveItem interactible = hit.collider.GetComponent<VRInteractiveItem>(); //attempt to get the VRInteractiveItem on the hit object
                m_CurrentInteractible = interactible;

                // If we hit an interactive item and it's not the same as the last interactive item, then call Over
                if (interactible && interactible != m_LastInteractible)
                    interactible.Over(); 

                // Deactive the last interactive item 
                if (interactible != m_LastInteractible)
                    DeactiveLastInteractible();

                m_LastInteractible = interactible;

                // Something was hit, set at the hit position.
                if (m_Reticle)
                    m_Reticle.SetPosition(hit);

                if (OnRaycasthit != null)
                    OnRaycasthit(hit);

                if (hit.collider.tag == "Actor")
                {
                    GameObject hitObject = hit.collider.transform.gameObject;

                    message = hitObject.name;
                    GameObject emoji = null;
                    foreach (Transform child in hitObject.transform)
                    {
                        if (child.tag == "Emoji")
                        {
                            emoji = child.gameObject;
                        }
                    }

                    if (selectedObject != hitObject)
                    {
                        // Don't wait for pulsing to end
                        StopPulse();

                        // Reset pulsing object scale AND object color
                        emoji.transform.localScale = INITIAL_SCALE_VECTOR;
                        //if (selectedObject != null && startColor != null) selectedObject.GetComponent<Renderer>().material.color = startColor;

                        selectedObject = hitObject;
                    }

                    // Highlight object
                    //startColor = hitObject.GetComponent<Renderer>().material.color;
                    //hitObject.GetComponent<Renderer>().material.color = Color.yellow;

                    // Pulse object
                    Pulse(emoji);
                }
            }
            else
            {
                // Nothing was hit, deactive the last interactive item.
                DeactiveLastInteractible();
                m_CurrentInteractible = null;

                // Position the reticle at default distance.
                if (m_Reticle)
                    m_Reticle.SetPosition();
            }
        }


        private void DeactiveLastInteractible()
        {
            if (m_LastInteractible == null)
                return;

            m_LastInteractible.Out();
            m_LastInteractible = null;
        }


        private void HandleUp()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Up();
        }


        private void HandleDown()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Down();
        }


        private void HandleClick()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Click();
        }


        private void HandleDoubleClick()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.DoubleClick();

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
}