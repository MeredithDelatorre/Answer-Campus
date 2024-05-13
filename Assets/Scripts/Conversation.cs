
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct ConversationOutcome
{
    public string description;
    public FriendRelationship[] FriendRelationships;
    public int minutesElapsed;
}

public class Conversation : MonoBehaviour
{
    public ConversationOutcome[] outcomes;
    public int chosenOutcome;
    void Start()
    {
        string conversation = PlayerPrefs.GetString("Current Conversation", "Default");
        //     PlayerPrefs.GetInt("Minutes Passed", outcomes[0].minutesElapsed);
    }


    public void SetOutcome(int outcomeIndex)

    {
        if(chosenOutcome < outcomes.Length) {
            chosenOutcome = outcomeIndex;
        }
        Debug.Log("Outcome: " + outcomes[outcomeIndex].description);
        //   Debug.Log("Setting Outcome " + outcomes[outcomeIndex].description);
    }
}
