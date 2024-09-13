using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum Relationship { NONE, NEGATIVE, POSITIVE, FRIEND };
[System.Serializable]
public enum Character { NONE, LEILANI, DEEPAK, SOFIA, BREANNA, MATTHEW, BEAU, ERIC, JIAH, CHARLI, JOSE, BRAD}

[System.Serializable]
public struct FriendRelationship
{
    public string friend;
    public Relationship relationship;
}

[System.Serializable]
public struct TextMessage : System.IEquatable<TextMessage>
{
    public Character from;
    public string message;
    public string location;

    public bool Equals(TextMessage other)
    {
        // Compare all fields
        return from.Equals(other.from) &&
               message == other.message &&
               location == other.location;
    }
    // Override Equals method
    public override bool Equals(object obj)
    {
        if (obj is TextMessage otherMessage)
            return Equals(otherMessage);

        return false;
    }

    // Override GetHashCode method
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + from.GetHashCode();
            hash = hash * 23 + (message?.GetHashCode() ?? 0);
            hash = hash * 23 + (location?.GetHashCode() ?? 0);
            return hash;
        }
    }

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
