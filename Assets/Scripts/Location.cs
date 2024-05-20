using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using VNEngine;

public class Location : MonoBehaviour
{
    public string scene;
    public string conversation;
    public string timestamp;
    public int minutes;
    public bool played = false;

    public void GoToLocation()
    {
//        PlayerPrefs.SetString("Current Conversation", conversation);
        StatsManager.Add_To_Numbered_Stat("Minutes Passed", minutes);
        SceneManager.LoadScene(scene);

    }
    
    public void SetTimestamp()
    {
        timestamp = DateTime.Now.ToString("f");
    }

}
