using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AMPM : MonoBehaviour
{
    private float waitTime = 120.0f;
    private float timer = 0.0f;
    public Text AMPMtext;
    // Start is called before the first frame update
    void Start()
    {
        AMPMtext.text = "PM"; 
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > waitTime)
        {
            AMPMtext.text = "AM";
        }
    }
}
