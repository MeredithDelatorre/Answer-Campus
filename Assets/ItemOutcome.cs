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
    public GameObject nextLocation; //the object to activate when this one hides itsel

    private Button button;
    // Called initially when the node is run, put most of your logic here
    public void OutcomeAchieved()
    {
        Debug.Log("Clicked on Outcome Item");
        int minutesPassed = PlayerPrefs.GetInt("Minutes Passed", 0);
        minutesPassed += minutesElapsed;
        PlayerPrefs.SetInt("Minutes Passed", minutesPassed);
        StatsManager.Add_To_Numbered_Stat("Items Collected", 1);
        StatsManager.Add_To_Numbered_Stat("Minutes Passed", minutesPassed);
        if (title.Length > 0)
        {
            //LOCATION as title for search
            StatsManager.Set_Boolean_Stat(title, true);

        }
        for (int i = 0; i < friendRelationships.Length; i++)
        {
            StatsManager.Set_Numbered_Stat(friendRelationships[i].friend, (int)friendRelationships[i].relationship);
        }

        if(nextLocation)
        {
            nextLocation.SetActive(true);
            Next();
        }


    }

    public void Next()
    {
        if(transform.parent.parent.GetComponentInParent<FindObject>())
        {
            transform.parent.parent.GetComponentInParent<FindObject>().Found(this);
        }
    }

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }
    private void OnButtonClick()
    {
        OutcomeAchieved();
        
    }
}
