using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateCalendar : MonoBehaviour
{
    public GameObject checkmark;
    // Start is called before the first frame update

    void Start()
    {
        int currentDay = PlayerPrefs.GetInt("current date", 25);
        for(int i = 0; i < currentDay; i++)
        {
            Instantiate(checkmark, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
