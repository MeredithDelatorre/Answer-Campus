using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateCalendar : MonoBehaviour
{
    public GameObject checkmark;
    public GameObject deadline;

    // Start is called before the first frame update

    void Start()
    {
        int currentDay = PlayerPrefs.GetInt("current date", 2);
        for(int i = 0; i < currentDay; i++)
        {
            Instantiate(checkmark, transform);
        }

        if(PlayerPrefs.GetInt("Competition", 0) == 1)
        {
            deadline.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
