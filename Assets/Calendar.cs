using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calendar : MonoBehaviour
{
    public int currentDateOnCalendar;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("current date", currentDateOnCalendar);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
