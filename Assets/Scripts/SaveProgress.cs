using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveProgress : MonoBehaviour
{
    [System.Serializable]
    public struct CharacterRelationship
    {
        public string character;
        public Relationship relationship;

    }

    public string nextScene;

    [SerializeField]
    public CharacterRelationship[] relationships;


    public void StartNewGame(GameObject newGamePanelOverride)
    {

        //TODO: To determine whether or not a game has been started, we look for the first PlayerPref that has been saved. In the original version, Leilani joined
        //our team in the first scene, so we check her key: PlayerPrefs.HasKey("Leilani")
        
        if (PlayerPrefs.HasKey("First Saved Preference")) { 
            newGamePanelOverride.SetActive(true);
        }
        else
        {
            Debug.Log("Reseting player prefs...");
            Reset();
            Debug.Log("Saving friendship player prefs...");
            Save();
            Debug.Log("Loading Cutscene...");
            GetComponent<MenuOptions>().LoadScene(GetComponent<MenuOptions>().sceneToLoad);
           
        }
    }
    public void SetNextScene()
    {
        PlayerPrefs.SetString("Next Scene", nextScene);
        Debug.Log("Next Scene: " + nextScene);
    }

    public void UpdateFriendship()
    {
        for(int i = 0; i < relationships.Length; i++)
        {
            PlayerPrefs.SetInt(relationships[i].character, (int)relationships[i].relationship);

        }
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();
    }

    public void Save()
    {
        //CALLLED WHEN CLICKING ON CHOICE
        Debug.Log("Saving Progress...");
        UpdateFriendship();
        SetNextScene();
    }
}
