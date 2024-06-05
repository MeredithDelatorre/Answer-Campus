using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum Relationship { NONE, NEGATIVE, POSITIVE, FRIEND };
[System.Serializable]
public enum Character { LEILANI, DEEPAK, SOFIA, BREANNA, MATTHEW, BEAU, ERIC, JIAH, CHARLI, JOSE, BRAD}

[System.Serializable]
public struct FriendRelationship
{
    public string friend;
    public Relationship relationship;
}

[System.Serializable]
public struct TextMessage
{
    public Character from;
    public string message;
    public string location;
}

[System.Serializable]
public class Friend : MonoBehaviour
{

    public Relationship relationship;
    public string characterName;
    public string scene;
    public Image status;
    public Sprite[] statusTypes;


    // Start is called before the first frame update
    void Start()
    {
        int current_status = PlayerPrefs.GetInt(characterName, 0);
        Debug.Log(characterName + ": " + current_status);
        status.sprite = statusTypes[current_status];        
    }

}
