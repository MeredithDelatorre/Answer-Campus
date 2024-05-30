using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VNEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

public class ItemOutcome : MonoBehaviour
{

    public string title; //name of location, clue or any other item
    public string description; //only for descriptive purposes in the editor, not in use
    public float GPA = 4.0f; //not in use
    public FriendRelationship[] friendRelationships; //changes in friend relationships
    public int minutesElapsed; //time added to the game clock
    public FindObject searchLocation; //the object to activate when this one hides itself
    public bool collectable = false;

    private Button button;
    // Called initially when the node is run, put most of your logic here
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);

    }
    public void OutcomeAchieved()
    {
        Debug.Log("Clicked on Outcome Item: " + title);

        int minutesPassed = PlayerPrefs.GetInt("Minutes Passed", 0);
        minutesPassed += minutesElapsed;
        PlayerPrefs.SetInt("Minutes Passed", minutesPassed);
        if(collectable)
        {
            if (StatsManager.Numbered_Stat_Exists("Items Collected"))
            {
                StatsManager.Add_To_Numbered_Stat("Items Collected", 1);
            }
            else
            {
                StatsManager.Set_Numbered_Stat("Items Collected", 1);
            }

            if (title.Length > 0)
            {
                //Can be checked to see if it's been collected.
                StatsManager.Set_Boolean_Stat(title, true);
                Debug.Log(title + " stat set to " + StatsManager.Get_Boolean_Stat(title));
            }

        }
        if(StatsManager.Numbered_Stat_Exists("Minutes Passed"))
        {
            StatsManager.Add_To_Numbered_Stat("Minutes Passed", minutesPassed);
        }
        else
        {
            StatsManager.Set_Numbered_Stat("Minutes Passed", minutesPassed);
        }

        for (int i = 0; i < friendRelationships.Length; i++)
        {
            StatsManager.Set_Numbered_Stat(friendRelationships[i].friend, (int)friendRelationships[i].relationship);
        }
        Next();

    }

    public void Next()
    {

        if (searchLocation)
        {
            searchLocation.Found(this);
        }
    }

    private void OnButtonClick()
    {
        OutcomeAchieved();
        
    }
}
