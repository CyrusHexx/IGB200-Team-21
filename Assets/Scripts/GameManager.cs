using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; 
    public Appliance[] appliances;
    public PowerFuseBox[] fuseBoxes; 
    public Slider loadSlider; 
    public Light mainLight;
    public float maxLoad = 100f; 
    public bool overload = false;
    public bool poweredDown = false;
    public float currentLoad;

    public GameObject tutorialPanel;
    public Timer timerScript;
    public AMPM ampmScript;
    public MonoBehaviour ghostScript;
    public Ghost[] ghostScripts;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        ShowTutorial();

        loadSlider.maxValue = maxLoad;

        foreach (Appliance appliance in appliances)
        {
            if (appliance.IsOn())
            {
                currentLoad += appliance.loadContribution;
            }
        }

        loadSlider.value = currentLoad;
    }

    private void Update()
    {
        ///Debug.Log(overload);
        ///Debug.Log(currentLoad);
        if (tutorialPanel.activeSelf == false && overload == false)
        {
            UpdateAppliance();
        }
    }

    private void UpdateAppliance()
    {
        List<Appliance> offAppliances = new List<Appliance>();
        foreach (var appliance in appliances)
        {
            bool inGhostRange = appliance.GetComponent<Appliance>().inGhostRange;
            if (inGhostRange == true && overload == false && !appliance.IsOn())
            {
                offAppliances.Add(appliance);
            }
        }
        for(int i = 0; i < offAppliances.Count; i++)
        {
            Appliance turnedONAppliance = offAppliances[i];
            turnedONAppliance.ToggleState(false);
        }
    }
    
   public void resetLoad()
    {
        overload = false;
        currentLoad = 0;
        loadSlider.value = currentLoad;
        mainLight.intensity = 1f;
    }
    
    public void UpdateLoadMeter(float loadChange)
    {
        currentLoad += loadChange;
        currentLoad = Mathf.Clamp(currentLoad, 0, maxLoad);

        loadSlider.value = currentLoad;

        ///Debug.Log("Current Load: " + currentLoad);


        if (currentLoad >= maxLoad)
        {
            overload = true;
            mainLight.intensity = 0.1f;

            List<PowerFuseBox> onPowerBox = new List<PowerFuseBox>();
            foreach (var fuses in fuseBoxes)
            {
                if (!fuses.IsOn())
                {
                    onPowerBox.Add(fuses);
                }
            }
            for (int i = 0; i < onPowerBox.Count; i++)
            {

                PowerFuseBox turnedOFFPowerBox = onPowerBox[i];
                turnedOFFPowerBox.powerRestart();
                
            }

  
            List<Appliance> onAppliances = new List<Appliance>();
            foreach (var appliance in appliances)
            {
                if (appliance.IsOn())
                {
                    onAppliances.Add(appliance);
                }
            }
            for (int i = 0; i < onAppliances.Count; i++)
            {
                
                Appliance turnedOFFAppliance = onAppliances[i];
                turnedOFFAppliance.ToggleState(true);
            }
        }
    }

    public void ShowTutorial()
    {
        tutorialPanel.SetActive(true); 

        timerScript.enabled = false;
        ampmScript.enabled = false;

        foreach (var ghost in ghostScripts)
        {
            ghost.enabled = false;
        }

    }

    public void HideTutorial()
    {
        tutorialPanel.SetActive(false); 

        timerScript.enabled = true;
        ampmScript.enabled = true;

        foreach (var ghost in ghostScripts)
        {
            ghost.enabled = true;
        }

    }
}