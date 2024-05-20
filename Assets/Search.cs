using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VNEngine;

public class Search : MonoBehaviour
{
    public ConversationManager correctLocation;
    public ConversationManager incorrectLocation;
    public GameObject map;
    // Start is called before the first frame update
    void Start()
    {
//        string conversation = PlayerPrefs.GetString("Current Conversation");
        float minutesPassed = StatsManager.Get_Numbered_Stat("Minutes Passed");
        if(StatsManager.Numbered_Stat_Exists(SceneManager.GetActiveScene().name))
        {
            StatsManager.Add_To_Numbered_Stat(SceneManager.GetActiveScene().name + " Visits", 1);
        }
        else
        {
            StatsManager.Set_Numbered_Stat(SceneManager.GetActiveScene().name + " Visits", 1);
        }
        /*
        if (StatsManager.Get_Boolean_Stat(SceneManager.GetActiveScene().name))
        {
            //Search Successful
            //remove this location as a good place to search
            StatsManager.Set_Boolean_Stat(SceneManager.GetActiveScene().name, false);
            VNSceneManager.scene_manager.Start_Conversation(correctLocation);
        }
        else
        {
            //Wrong Location, time added regardless of choice
            VNSceneManager.scene_manager.Start_Conversation(incorrectLocation);

        }
        */

    }

}
