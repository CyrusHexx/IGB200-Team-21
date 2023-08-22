using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public string[] secTimer = { "00", "10", "20", "30", "40", "50" };
    public string[] hourTimer = { "08", "09", "10", "11", "12", "01", "02", "03", "04", "05", "06" };

    public Text timerText;
    private float timer = 0.0f;
    private float waitTime = 5.0f;
    private int secCounter = 0;
    private int hourCounter = 0;


    // Start is called before the first frame update
    void Start()
    {
        timerText.text = hourTimer[hourCounter] + ":" + secTimer[secCounter];
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > waitTime)
        {
            if (secCounter == 5)
            {
                hourCounter = hourCounter + 1;
                secCounter = 0;
                timerText.text = hourTimer[hourCounter] + ":" + secTimer[secCounter];
                timer = timer - waitTime;
            }
            else
            {
                secCounter = secCounter + 1;
                timerText.text = hourTimer[hourCounter] + ":" + secTimer[secCounter];
                timer = timer - waitTime;
            }
        }
    }
}
