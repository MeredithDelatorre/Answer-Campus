using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VNEngine;

public class Search : MonoBehaviour
{
    public GameObject map;
    // Start is called before the first frame update
    void Start()
    {
//        string conversation = PlayerPrefs.GetString("Current Conversation");
        float minutesPassed = StatsManager.Get_Numbered_Stat("Minutes Passed");
        if(StatsManager.Numbered_Stat_Exists(SceneManager.GetActiveScene().name + " Visits"))
        {
            StatsManager.Add_To_Numbered_Stat(SceneManager.GetActiveScene().name + " Visits", 1);
        }
        else
        {
            StatsManager.Set_Numbered_Stat(SceneManager.GetActiveScene().name + " Visits", 1);
        }
        Debug.Log("Visits to " + SceneManager.GetActiveScene().name + ": " + StatsManager.Get_Numbered_Stat(SceneManager.GetActiveScene().name + " Visits"));
    }

}
