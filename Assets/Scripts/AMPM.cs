using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AMPM : MonoBehaviour
{
    private float waitTime = 120.0f;
    private float timer = 0.0f;
    private TextMeshProUGUI AMPMtext; //Whoever use Text instead of TMPro your an fools. @Nathan3197, jk. TMPRO has more cools functions that you can do with text
    // Start is called before the first frame update
    void Start()
    {
        AMPMtext = gameObject.GetComponent<TextMeshProUGUI>(); // this script already has the text, may aswell grab it in code instead of the editor
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
