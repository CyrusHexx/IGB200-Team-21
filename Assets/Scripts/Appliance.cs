using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Appliance : MonoBehaviour
{
    public float loadContribution = 5.0f; // Load added to the meter when this appliance is on
    public Color onColor = Color.red; // Color when the appliance is on
    public Color offColor = Color.green; // Color when the appliance is off
    private bool isOn;
    public bool inGhostRange;

    private GameObject player;
    public GameObject infoText;
    private bool infoShown = false;
    private GameObject appName;
    private GameObject appInfo;
    private GameObject appStatus;

    private Renderer rend; // Reference to the Renderer component

    private void Start()
    {
        player = GameObject.Find("Player");
        infoText = GameObject.Find("Appliance Info");
        rend = GetComponent<Renderer>(); // Get the Renderer component

        // set initial state
        isOn = false;

        // Update the load meter based on the initial state
        if (isOn)
        {
            GameManager.instance.UpdateLoadMeter(loadContribution);
        }

        UpdateColor(); // Set the initial color
    }

    private void Update()
    {
        CheckPlayerDistance();
    }

    
    private void CheckPlayerDistance()
    {
        if (infoShown == false)
        {
            if (Vector3.Distance(this.gameObject.transform.position, player.transform.position) <= 15)
            {
                appName = Instantiate(infoText, this.gameObject.transform);
                appInfo = Instantiate(infoText, this.gameObject.transform);
                appStatus = Instantiate(infoText, this.gameObject.transform);

                appName.transform.position += new Vector3(-5, 0, 10);
                appInfo.transform.position += new Vector3(-5, 0, 8);
                appStatus.transform.position += new Vector3(-5, 0, 6);

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
                
                infoShown = true;
            }
        }
        else
        {
            if (Vector3.Distance(this.gameObject.transform.position, player.transform.position) > 15)
            {
                Destroy(appName);
                Destroy(appInfo);
                Destroy(appStatus);
                infoShown = false;
            }
        }
        
    }


    public void ToggleState(bool overloaded)
    {
        if (overloaded == false)
        {
            isOn = !isOn;
            UpdateColor(); // Update the color based on the new state
            GameManager.instance.UpdateLoadMeter(isOn ? loadContribution : -loadContribution);
        }
        else if (overloaded == true)
        {
            isOn = !isOn;
            UpdateColor(); // Update the color based on the new state
        } 
    }

    private void UpdateColor()
    {
        rend.material.color = isOn ? onColor : offColor; // Set the color based on the state
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