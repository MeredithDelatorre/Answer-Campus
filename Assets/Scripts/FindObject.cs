using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VNEngine;

public class FindObject : MonoBehaviour
{
    public GameObject[] foundObjects;
    public GameObject startingLocation;
    public NodeSearch search;
    // Start is called before the first frame update

    public void Found(ItemOutcome outcome)
    {
        search.Finish_Node();
        Debug.Log("Clicked on " + outcome.title);
        outcome.gameObject.SetActive(false);
        ResetSearch();
        gameObject.SetActive(false);

    }

    public void ResetSearch()
    {
        for(int i = 0; i < GetComponentsInChildren<VNEngine.ClickToStartConversation>().Length; i++)
        {
            GetComponentsInChildren<VNEngine.ClickToStartConversation>()[i].gameObject.SetActive(false);
        } 
        startingLocation.SetActive(true);
    }

}
