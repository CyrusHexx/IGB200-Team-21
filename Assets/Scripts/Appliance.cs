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
    private GameObject ApplianceUI;
    private GameObject appInfo;
    private bool infoShown = false;
    private Text appName;
    private Text appCost;
    private Text appStatus;

    public WireConnect wireConnectGame;

    private GameObject electricParticle;
    private GameObject particleEffect;

    private Renderer rend; // Reference to the Renderer component

    private void Start()
    {
        player = GameObject.Find("Player");
        ApplianceUI = GameObject.Find("App UI");
        electricParticle = GameObject.Find("CFX_ElectricityBall");
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
        if (Vector3.Distance(this.gameObject.transform.position, player.transform.position) <= 10)
        {
            if (infoShown == false)
            {
                appInfo = Instantiate(ApplianceUI, this.gameObject.transform);
                appName = appInfo.transform.GetChild(1).gameObject.GetComponent<Text>();
                appCost = appInfo.transform.GetChild(2).gameObject.GetComponent<Text>();
                appStatus = appInfo.transform.GetChild(3).gameObject.GetComponent<Text>();

                appInfo.SetActive(true);
                appInfo.transform.position += new Vector3(-55, 13, -75);
                infoShown = true;
            }

            
            
            appName.text = this.gameObject.name;
            appCost.text = "Energy Cost: " + loadContribution;
            if (isOn == false)
            {
                appStatus.text = "Status: OFF";
            }
            else if (isOn == true)
            {
                if(loadContribution >= 10f)
                {
                    appStatus.text = "Status: BROKEN";
                }
                else
                {
                    appStatus.text = "Status: ON";
                }
            }
        }
        else
        {
            Destroy(appInfo);
            infoShown = false;
        }  
    }

    public void ToggleState(bool overloaded)
    {
        if (overloaded == false)
        {
            isOn = !isOn;
            ///turn vfx on/off with command
            if (isOn == true)
            {
                particleEffect = Instantiate(electricParticle, this.gameObject.transform);
                particleEffect.transform.localScale = new Vector3(6, 6, 6);
            }

            if (isOn == false) // If the appliance is switched off
            {
                TryTriggerWireGame(); // Try to trigger the wire mini-game
                Destroy(particleEffect);
            }
            GameManager.instance.UpdateLoadMeter(isOn ? loadContribution : -loadContribution);
        }
        else if (overloaded == true)
        {
            isOn = !isOn;
            Destroy(particleEffect);
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