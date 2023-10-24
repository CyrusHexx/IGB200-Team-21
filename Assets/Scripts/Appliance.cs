using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Appliance : MonoBehaviour
{
    public float loadContribution = 5.0f; // Load added to the meter when this appliance is on

    private bool isOn;
    public bool inGhostRange;

    private GameObject player;
    public GameObject infoText;
    private bool infoShown = false;
    private GameObject appName;
    private GameObject appInfo;
    private GameObject appStatus;

    public WireConnect wireConnectGame;

    private Renderer rend; // Reference to the Renderer component

    private void Start()
    {
        player = GameObject.Find("Player");
        infoText = GameObject.Find("Appliance Info");
        rend = GetComponent<Renderer>(); // Get the Renderer component

        // set initial state
        isOn = false;

        wireConnectGame.OnGameCompleted += HandleGameCompleted;
        ///Debug.Log("Subscribed to OnGameCompleted event!");

    }

    private void Update()
    {
        CheckPlayerDistance();
    }

    
    private void CheckPlayerDistance()
    {
        if (Vector3.Distance(this.gameObject.transform.position, player.transform.position) <= 8)
        {
            if (infoShown == false)
            {
                appName = Instantiate(infoText, this.gameObject.transform);
                appInfo = Instantiate(infoText, this.gameObject.transform);
                appStatus = Instantiate(infoText, this.gameObject.transform);

                var appNameMesh = appName.GetComponent<MeshRenderer>();
                appNameMesh.enabled = true;

                var appInfoMesh = appInfo.GetComponent<MeshRenderer>();
                appInfoMesh.enabled = true;

                var appStatusMesh = appStatus.GetComponent<MeshRenderer>();
                appStatusMesh.enabled = true;

                appName.transform.position += new Vector3(-5, 0, 10);
                appInfo.transform.position += new Vector3(-5, 0, 8);
                appStatus.transform.position += new Vector3(-5, 0, 6);

                infoShown = true;
            }

            TextMesh nameInfo = appName.GetComponent<TextMesh>();
            TextMesh costInfo = appInfo.GetComponent<TextMesh>();
            TextMesh statusInfo = appStatus.GetComponent<TextMesh>();


            nameInfo.text = this.gameObject.name;
            costInfo.text = "Energy Cost: " + loadContribution;
            if (isOn == false)
            {
                statusInfo.text = "Status: OFF";
            }
            else if (isOn == true)
            {
                if(loadContribution >= 10f)
                {
                    statusInfo.text = "Status: DEFECTIVE";
                }
                else
                {
                    statusInfo.text = "Status: ON";
                }
            }
        }
        else
        {
            Destroy(appName);
            Destroy(appInfo);
            Destroy(appStatus);
            infoShown = false;
        }  
    }

    public void ToggleState(bool overloaded)
    {
        if (overloaded == false)
        {
            isOn = !isOn;
            ///turn vfx on/off with command
            
            if (isOn == false) // If the appliance is switched off
            {
                Debug.Log("hit");
                TryTriggerWireGame(); // Try to trigger the wire mini-game
            }
            GameManager.instance.UpdateLoadMeter(isOn ? loadContribution : -loadContribution);
        }
        else if (overloaded == true)
        {
            isOn = !isOn;
            ///turn vfx on/off with command
        }
    }

    void TryTriggerWireGame()
    {

        if (loadContribution >= 10)
        {
            wireConnectGame.ShowGame();
        }
    }

    private void HandleGameCompleted()
    {
        Debug.Log("HandleGameCompleted called!");
        wireConnectGame.HideGame();
    }

    public bool IsOn() => isOn;


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Ghost")
        {
            inGhostRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Ghost")
        {
            inGhostRange = false;
        }
    }
}