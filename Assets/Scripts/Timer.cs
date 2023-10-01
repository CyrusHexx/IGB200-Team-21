using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Timer : MonoBehaviour
{
    public string[] secTimer = { "00", "10", "20", "30", "40", "50" };
    public string[] hourTimer = { "08", "09", "10", "11", "12", "01", "02", "03", "04", "05", "06" };

    public Text timerText;
    private float timer = 0.0f;
    private float waitTime = 5.0f;
    private int secCounter = 0;
    private int hourCounter = 0;

    void Start()
    {
        UpdateTimeText();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > waitTime)
        {
            timer -= waitTime;

            secCounter++;
            if (secCounter >= secTimer.Length)
            {
                secCounter = 0;
                hourCounter++;
            }

            if (hourCounter >= hourTimer.Length)
            {
                hourCounter = 0;
            }

            UpdateTimeText();

            if (hourTimer[hourCounter] == "06" && secTimer[secCounter] == "00")
            {
                SceneManager.LoadScene("Win");
            }
        }
    }

    void UpdateTimeText()
    {
        timerText.text = $"{hourTimer[hourCounter]}:{secTimer[secCounter]}";
    }
}
